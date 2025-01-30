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
import { redirect, type Action, type Handle } from "@sveltejs/kit";
import { KeyCloak } from "arctic";
import jsonwebtoken from "jsonwebtoken";
import { dev } from "$app/environment";
import { hasRole, type Role } from "./utils/roles.svelte";

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
	user_id: string;
	access_token: string;
	roles: Role[];
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

export const handle: Handle = ({ event, resolve }) => {
	// TODO: this isn't ideal but it will work for now...
	const jwt = event.cookies.get(KC_COOKIE_NAME);
	event.locals.session = async () => {
		try {
			if (!jwt) {
				return null;
			}

			const token = jsonwebtoken.decode(jwt, { complete: true });
			if (!token || typeof token === "string" || !token.payload) {
				return null;
			}
			const payload = token.payload as jsonwebtoken.JwtPayload;
			if (payload.exp && payload.exp * 1000 < Date.now()) {
				return null;
			}

			const session: Session = {
				email_verified: payload.email_verified || false,
				name: payload.name || "",
				preferred_username: payload.preferred_username || "",
				given_name: payload.given_name || "",
				family_name: payload.family_name || "",
				email: payload.email || "",
				user_id: payload.sub || "",
				// TODO: Get via ENV as this is technically a variable name depending on what the client is named...
				roles: payload.resource_access?.intra?.roles ?? [],
				access_token: jwt,
			};

			return session;
		} catch (error) {
			console.error("Error decoding JWT:", error);
			return null;
		}
	};

	return resolve(event);
};
