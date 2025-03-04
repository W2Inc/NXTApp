// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import * as arctic from "arctic";
import { keycloak } from "$lib/oauth";
import type { RequestHandler } from "./$types";
import jwt from "jsonwebtoken";
import { redirect, type Cookies } from "@sveltejs/kit";
import { KC_COOKIE_NAME } from "$env/static/private";
import { dev } from "$app/environment";

// ============================================================================

const C_STATE = "state";
const C_VERIFIER = "verifier";

// ============================================================================

function verifyUrl(url: URL, cookies: Cookies) {
	const code = url.searchParams.get("code");
	const newState = url.searchParams.get("state");
	const oldState = cookies.get(C_STATE);

	if (oldState === null || code === null || newState === null) {
		return {
			code: null,
			codeVerifier: null,
		};
	}

	const verifier = cookies.get(C_VERIFIER);
	if (oldState !== newState || !verifier) {
		return {
			code: null,
			codeVerifier: null,
		};
	}

	cookies.delete(C_STATE, {
		path: "/",
	});
	cookies.delete(C_VERIFIER, {
		path: "/",
	});

	return {
		code,
		verifier,
	};
}

// ============================================================================

export const GET: RequestHandler = async ({ url, cookies }) => {
	const { code, verifier } = verifyUrl(url, cookies);
	if (!code || !verifier) {
		return new Response(null, {
			status: 400,
		});
	}

	try {
		const tokens = await keycloak.validateAuthorizationCode(code, verifier);
		const accessToken = tokens.accessToken();
		const refreshToken = tokens.refreshToken();

		// const accessTokenExpiresAt = tokens.accessTokenExpiresAt();
		// const refreshToken = tokens.refreshToken();

		cookies.set(`${KC_COOKIE_NAME}-a`, accessToken, {
			secure: !dev,
			path: "/",
			httpOnly: true,
			sameSite: 'strict',
			// maxAge: tokens.accessTokenExpiresInSeconds(),
		});

		cookies.set(`${KC_COOKIE_NAME}-r`, refreshToken, {
			secure: !dev,
			path: "/",
			httpOnly: true,
			sameSite: 'strict',
		});

		return new Response(null, {
			status: 302,
			headers: {
				location: "/",
				// "Cache-Control": "no-cache, no-store, must-revalidate, max-age=0",
				// "Pragma": "no-cache",
				"Refresh": "2;url=/",
			},
		});
	} catch (e) {
		if (e instanceof arctic.OAuth2RequestError) {
			// Invalid authorization code, credentials, or redirect URI
			return new Response(e.message, {
				status: 500,
			});
		}
		if (e instanceof arctic.ArcticFetchError) {
			// Failed to call `fetch()`
			return new Response(e.message, {
				status: 500,
			});
		}
		return new Response("Something went wrong! Please report!", {
			status: 500,
		});
	}
};
