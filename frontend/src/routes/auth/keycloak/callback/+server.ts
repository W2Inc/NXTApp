// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import type { RequestHandler } from "./$types";
import { KC_COOKIE_NAME } from "$env/static/private";
import { dev } from "$app/environment";
import {
	exchangeCodeForTokens,
	exchangeForUMATicket
} from "$lib/oauth";
import { logger } from "$lib/logger";

// ============================================================================

const STATE_COOKIE = "oauth_state";
const VERIFIER_COOKIE = "oauth_verifier";
const ACCESS_TOKEN_COOKIE = `${KC_COOKIE_NAME}-a`;
const REFRESH_TOKEN_COOKIE = `${KC_COOKIE_NAME}-r`;

// ============================================================================

export const GET: RequestHandler = async ({ url, cookies }) => {
	const code = url.searchParams.get("code");
	const state = url.searchParams.get("state");
	const storedState = cookies.get(STATE_COOKIE);
	const codeVerifier = cookies.get(VERIFIER_COOKIE);

	// Clean up cookies regardless of outcome
	cookies.delete(STATE_COOKIE, { path: "/" });
	cookies.delete(VERIFIER_COOKIE, { path: "/" });

	// Validate state and code
	if (!code || !state || !storedState || state !== storedState || !codeVerifier) {
		logger.error("Invalid OAuth callback parameters", {
			hasCode: !!code,
			hasState: !!state,
			hasStoredState: !!storedState,
			stateMatch: state === storedState,
			hasVerifier: !!codeVerifier
		});
		return new Response("Invalid request", { status: 400 });
	}

	try {
		const tokens = await exchangeCodeForTokens(code, codeVerifier);
		const umaAccessToken = await exchangeForUMATicket(tokens.accessToken);

		// Store tokens in cookies
		cookies.set(ACCESS_TOKEN_COOKIE, umaAccessToken, {
			secure: !dev,
			path: "/",
			httpOnly: true,
			sameSite: "lax"
		});

		cookies.set(REFRESH_TOKEN_COOKIE, tokens.refreshToken, {
			secure: !dev,
			path: "/",
			httpOnly: true,
			sameSite: "lax"
		});

		return new Response(null, {
			status: 302,
			headers: {
				location: "/",
			},
		});
	} catch (e) {
		logger.error("Authentication error", e);
		return new Response("Authentication failed", {
			status: 500,
		});
	}
};
