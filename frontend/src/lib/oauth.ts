// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import * as arctic from "arctic";
import {
	KC_CLIENT_ID,
	KC_CLIENT_SECRET,
	KC_COOKIE_NAME,
	KC_ISSUER,
} from "$env/static/private";
import { error, redirect, type Action, type Handle } from "@sveltejs/kit";
import { KeyCloak } from "arctic";
import jsonwebtoken from "jsonwebtoken";
import { dev } from "$app/environment";
import { hasRole, type Role } from "./utils/roles.svelte";
import { logger } from "./logger";
import { PUBLIC_S3_BUCKET, PUBLIC_S3_ENDPOINT } from "$env/static/public";

// ============================================================================

export const keycloak = new KeyCloak(
	KC_ISSUER,
	KC_CLIENT_ID,
	KC_CLIENT_SECRET,
	"http://localhost:5173/auth/keycloak/callback",
);

// ============================================================================

export const signIn: Action = ({ cookies }) => {
	const state = arctic.generateState();
	const codeVerifier = arctic.generateCodeVerifier();

	cookies.set("state", state, {
		secure: !dev,
		path: "/",
		httpOnly: true,
		maxAge: 60 * 10, // 10 min
	});

	cookies.set("verifier", codeVerifier, {
		secure: !dev,
		path: "/",
		httpOnly: true,
		maxAge: 60 * 10, // 10 min
	});

	redirect(
		303,
		keycloak.createAuthorizationURL(state, codeVerifier, ["openid", "profile"]),
	);
};

export const signOut: Action = async ({ cookies }) => {
	const token = cookies.get(KC_COOKIE_NAME);
	if (!token) {
		return;
	}

	await keycloak.revokeToken(token);
	cookies.delete(KC_COOKIE_NAME, {
		path: "/",
	});

	redirect(308, "/");
};

// ============================================================================

export interface Session {
	email_verified: boolean;
	name: string;
	preferred_username: string;
	given_name: string;
	family_name: string;
	email: string;
	user_id: GUID;
	roles: Role[];
	avatarUrl: string;
}

declare global {
	namespace App {
		interface Locals {
			session(): Promise<Session | null>;
		}
		interface PageData {
			session: Session | null;
		}
	}
}

// ============================================================================
//
// The handle function checks whether the access token is expired. If it is,
// it attempts to refresh it using the refresh token. Afterward, the user's
// session data is set in event.locals for further use throughout your app.
//
// Note: The handle function must be async to use 'await' when refreshing.
//
// TODO: Storing sessions on the client as a cookie is "fine".
// What we could do is rather hand out a session id and store the tokens on a keyvalue db
// At least that way the user information isn't "directly" accessible and only will be retrieved via the server.
// Also makes storing the session easier.
//
// It won't make things "more secure" as, well, once you have the session id it's game over anyway.
// but they aren't accesible via javascript anyway.

export const handle: Handle = async ({ event, resolve }) => {
	const accessToken = event.cookies.get(`${KC_COOKIE_NAME}-a`);

	const isTokenExpired = (token: string) => {
		try {
			const decoded = jsonwebtoken.decode(token) as jsonwebtoken.JwtPayload;
			if (!decoded || !decoded.exp) return true;
			logger.debug("Keycloak access token is still valid");
			return decoded.exp < Math.floor(Date.now() / 1000);
		} catch {
			return true;
		}
	};

	const deleteCookies = () => {
		logger.debug(`Deleting cookies: ${KC_COOKIE_NAME}`);
		event.cookies.delete(`${KC_COOKIE_NAME}-a`, { path: "/" });
		event.cookies.delete(`${KC_COOKIE_NAME}-r`, { path: "/" });
	};

	if (accessToken && isTokenExpired(accessToken)) {
		const refreshToken = event.cookies.get(`${KC_COOKIE_NAME}-r`);
		if (refreshToken) {
			try {
				logger.debug("New tokens are required, requesting new token...");
				const tokens = await keycloak.refreshAccessToken(refreshToken);
				event.cookies.set(`${KC_COOKIE_NAME}-a`, tokens.accessToken(), {
					secure: !dev,
					path: "/",
					httpOnly: true,
					sameSite: "strict",
				});

				event.cookies.set(`${KC_COOKIE_NAME}-r`, tokens.refreshToken(), {
					secure: !dev,
					path: "/",
					sameSite: "strict",
					httpOnly: true,
				});

				// logger.debug("Session Cookies", { cookies: event.cookies.getAll() });
			} catch (e) {
				if (e instanceof arctic.OAuth2RequestError) {
					// e.g: Invalid grant aka token & refresh token expired
					/** @see https://datatracker.ietf.org/doc/html/rfc6749#section-5.2 */
					logger.debug("Failed to request new tokens", e);
				}
				if (e instanceof arctic.ArcticFetchError) {
					// Failed to call `fetch()`
					logger.error(e);
					error(500, e.message);
				}
				deleteCookies();
			}
		} else {
			// No refresh token available; clear the expired access token
			deleteCookies();
		}
	}

	// Provide session data for use in +layout.server.ts or other SvelteKit hooks
	event.locals.session = async () => {
		const currentAccessToken = event.cookies.get(`${KC_COOKIE_NAME}-a`);
		if (!currentAccessToken) {
			logger.debug("No session");
			return null;
		}

		try {
			const decoded = jsonwebtoken.decode(currentAccessToken, { complete: true });
			if (!decoded || typeof decoded === "string" || !decoded.payload) {
				logger.error("Bad JWT")
				return null;
			}

			const payload = decoded.payload as jsonwebtoken.JwtPayload;

			return {
				user_id: payload.sub || "",
				email: payload.email,
				email_verified: payload.email_verified,
				name: payload.name,
				preferred_username: payload.preferred_username,
				given_name: payload.given_name,
				family_name: payload.family_name,
				roles: payload.resource_access?.intra?.roles ?? [],
				avatarUrl: `${PUBLIC_S3_ENDPOINT}/${PUBLIC_S3_BUCKET}/${payload.sub}.png`,
			};
		} catch (e) {
			logger.error("Error decoding access token:", e);
			return null;
		}
	};

	// Proceed with the response
	return resolve(event);
};
