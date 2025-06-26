// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import {
	KC_CLIENT_ID,
	KC_CLIENT_SECRET,
	KC_COOKIE_NAME,
	KC_ISSUER,
} from "$env/static/private";
import { error, redirect, type Action, type Handle } from "@sveltejs/kit";
import { dev } from "$app/environment";
import { logger } from "./logger";
import { PUBLIC_S3_BUCKET, PUBLIC_S3_ENDPOINT } from "$env/static/public";
import type { Role } from "./utils/roles.svelte";
import { decode, type JwtPayload } from "jsonwebtoken";

// ============================================================================

// Constants for authentication
const AUTH_BASE_URL = KC_ISSUER;
const REDIRECT_URI = "http://localhost:5173/auth/keycloak/callback";
const TOKEN_ENDPOINT = `${AUTH_BASE_URL}/protocol/openid-connect/token`;
const AUTH_ENDPOINT = `${AUTH_BASE_URL}/protocol/openid-connect/auth`;
const LOGOUT_ENDPOINT = `${AUTH_BASE_URL}/protocol/openid-connect/logout`;

// Cookie names
const STATE_COOKIE = "oauth_state";
const VERIFIER_COOKIE = "oauth_verifier";
const ACCESS_TOKEN_COOKIE = `${KC_COOKIE_NAME}-a`;
const REFRESH_TOKEN_COOKIE = `${KC_COOKIE_NAME}-r`;

// ============================================================================

/**
 * Generate a cryptographically secure random string
 */
export function generateRandomString(): string {
	return Bun.CSRF.generate();
}

/**
 * Generate a code verifier for PKCE
 */
export function generateCodeVerifier(): string {
	return Bun.CSRF.generate();
}

/**
 * Generate code challenge from verifier using SHA-256
 */
export async function generateCodeChallenge(verifier: string): Promise<string> {
	const encoder = new TextEncoder();
	const data = encoder.encode(verifier);
	const digest = await crypto.subtle.digest('SHA-256', data);

	return btoa(String.fromCharCode(...new Uint8Array(digest)))
		.replace(/\+/g, '-')
		.replace(/\//g, '_')
		.replace(/=+$/, '');
}

/**
 * Generate state parameter for OAuth flow
 */
export function generateState(): string {
	return Bun.CSRF.generate();
}

/**
 * Create an authorization URL for initiating the OAuth flow
 */
export async function createAuthorizationURL(
	state: string,
	codeVerifier: string,
	scopes: string[] = ["email", "roles", "openid"]
): Promise<string> {
	const codeChallenge = await generateCodeChallenge(codeVerifier);

	const params = new URLSearchParams({
		client_id: KC_CLIENT_ID,
		redirect_uri: REDIRECT_URI,
		response_type: 'code',
		scope: scopes.join(' '),
		state: state,
		code_challenge: codeChallenge,
		code_challenge_method: 'S256'
	});

	return `${AUTH_ENDPOINT}?${params.toString()}`;
}

/**
 * Exchange authorization code for access and refresh tokens
 */
export async function exchangeCodeForTokens(
	code: string,
	codeVerifier: string
): Promise<{ accessToken: string, refreshToken: string }> {
	const params = new URLSearchParams({
		grant_type: 'authorization_code',
		client_id: KC_CLIENT_ID,
		client_secret: KC_CLIENT_SECRET,
		code: code,
		redirect_uri: REDIRECT_URI,
		code_verifier: codeVerifier
	});

	const response = await fetch(TOKEN_ENDPOINT, {
		method: 'POST',
		headers: {
			'Content-Type': 'application/x-www-form-urlencoded'
		},
		body: params.toString()
	});

	if (!response.ok) {
		logger.error('Failed to exchange code for tokens', await response.text());
		throw error(500, 'Failed to authenticate with identity provider');
	}

	const data = await response.json();
	return {
		accessToken: data.access_token,
		refreshToken: data.refresh_token
	};
}

/**
 * Exchange a standard access token for a UMA ticket token
 */
export async function exchangeForUMATicket(accessToken: string): Promise<string> {
	const params = new URLSearchParams({
		grant_type: 'urn:ietf:params:oauth:grant-type:uma-ticket',
		audience: 'intra'
	});

	const response = await fetch(TOKEN_ENDPOINT, {
		method: 'POST',
		headers: {
			'Content-Type': 'application/x-www-form-urlencoded',
			'Authorization': `Bearer ${accessToken}`
		},
		body: params.toString()
	});

	if (!response.ok) {
		logger.error(`Failed to exchange for UMA ticket: ${await response.text()}`);
		error(500, 'Failed to obtain authorization ticket');
	}

	const data = await response.json();
	return data.access_token;
}

/**
 * Refresh an access token using a refresh token
 */
export async function refreshTokens(
	refreshToken: string
): Promise<{ accessToken: string, refreshToken: string }> {
	const params = new URLSearchParams({
		grant_type: 'refresh_token',
		client_id: KC_CLIENT_ID,
		client_secret: KC_CLIENT_SECRET,
		refresh_token: refreshToken
	});

	const response = await fetch(TOKEN_ENDPOINT, {
		method: 'POST',
		headers: {
			'Content-Type': 'application/x-www-form-urlencoded'
		},
		body: params.toString()
	});

	if (!response.ok) {
		logger.error('Failed to refresh tokens', await response.text());
		throw error(500, 'Failed to refresh authentication');
	}

	const data = await response.json();
	return {
		accessToken: data.access_token,
		refreshToken: data.refresh_token
	};
}

/**
 * Revoke a token
 */
export async function revokeToken(token: string): Promise<void> {
	const params = new URLSearchParams({
		client_id: KC_CLIENT_ID,
		client_secret: KC_CLIENT_SECRET,
		token: token
	});

	await fetch(`${AUTH_BASE_URL}/protocol/openid-connect/revoke`, {
		method: 'POST',
		headers: {
			'Content-Type': 'application/x-www-form-urlencoded'
		},
		body: params.toString()
	});
}

/**
 * Check if a token is expired
 */
export function isTokenExpired(token: string): boolean {
	try {
		const decoded = decode(token) as JwtPayload;
		if (!decoded || !decoded.exp) return true;
		return decoded.exp < Math.floor(Date.now() / 1000);
	} catch {
		return true;
	}
}

// ============================================================================

/**
 * Permission scope from the token
 */
export interface Permission {
	scopes?: string[];
	rsid: string;
	rsname: string;
}

/**
 * User session information
 */
export interface Session {
	email_verified: boolean;
	name: string;
	preferred_username: string;
	given_name: string;
	family_name: string;
	email: string;
	user_id: GUID;
	roles: Role[];
	permissions: Permission[];
	scopes: string[];
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

export const handle: Handle = async ({ event, resolve }) => {
	const accessToken = event.cookies.get(ACCESS_TOKEN_COOKIE);

	const deleteCookies = () => {
		logger.debug(`Deleting cookies: ${KC_COOKIE_NAME}`);
		event.cookies.delete(ACCESS_TOKEN_COOKIE, { path: "/" });
		event.cookies.delete(REFRESH_TOKEN_COOKIE, { path: "/" });
	};

	if (accessToken && isTokenExpired(accessToken)) {
		const refreshToken = event.cookies.get(REFRESH_TOKEN_COOKIE);
		if (refreshToken) {
			try {
				logger.debug("New tokens are required, requesting new token...");
				const tokens = await refreshTokens(refreshToken);

				// Exchange for UMA ticket
				const umaAccessToken = await exchangeForUMATicket(tokens.accessToken);

				event.cookies.set(ACCESS_TOKEN_COOKIE, umaAccessToken, {
					secure: !dev,
					path: "/",
					httpOnly: true,
					sameSite: "strict",
				});

				event.cookies.set(REFRESH_TOKEN_COOKIE, tokens.refreshToken, {
					secure: !dev,
					path: "/",
					sameSite: "strict",
					httpOnly: true,
				});
			} catch (e) {
				logger.error("Failed to refresh tokens", e);
				deleteCookies();
			}
		} else {
			// No refresh token available; clear the expired access token
			deleteCookies();
		}
	}

	// Provide session data for SvelteKit hooks
	event.locals.session = async () => {
		const currentAccessToken = event.cookies.get(ACCESS_TOKEN_COOKIE);
		if (!currentAccessToken) {
			logger.debug("No session");
			return null;
		}

		try {
			const decoded = decode(currentAccessToken, { complete: true });
			if (!decoded || typeof decoded === "string" || !decoded.payload) {
				logger.error("Bad JWT")
				return null;
			}

			const payload = decoded.payload as JwtPayload & {
				authorization?: { permissions: Permission[] };
				resource_access?: { intra?: { roles: Role[] } };
			};

			// Extract permissions and flatten scopes
			const permissions = payload.authorization?.permissions || [];
			const scopes = permissions
				.flatMap(permission => permission.scopes || [])
				.filter((scope): scope is string => Boolean(scope));

			const session: Session = {
				user_id: payload.sub || "",
				email: payload.email as string,
				email_verified: payload.email_verified as boolean,
				name: payload.name as string,
				preferred_username: payload.preferred_username as string,
				given_name: payload.given_name as string,
				family_name: payload.family_name as string,
				roles: payload.resource_access?.intra?.roles || [],
				permissions,
				scopes,
				avatarUrl: `${PUBLIC_S3_ENDPOINT}/${PUBLIC_S3_BUCKET}/${payload.sub}.png`,
			};

			return session;
		} catch (e) {
			logger.error("Error decoding access token:", e);
			return null;
		}
	};

	// Proceed with the response
	return resolve(event);
};
