// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { redirect } from "@sveltejs/kit";
import type { Actions, PageServerLoad } from "./$types";
import { dev } from "$app/environment";
import { KC_COOKIE_NAME } from "$env/static/private";
import {
	generateState,
	generateCodeVerifier,
	createAuthorizationURL,
	revokeToken
} from "$lib/oauth";

// ============================================================================

// Cookie names
const STATE_COOKIE = "oauth_state";
const VERIFIER_COOKIE = "oauth_verifier";
const ACCESS_TOKEN_COOKIE = `${KC_COOKIE_NAME}-a`;
const REFRESH_TOKEN_COOKIE = `${KC_COOKIE_NAME}-r`;

// ============================================================================

export const load: PageServerLoad = async () => redirect(308, "/");

export const actions: Actions = {
	signin: async ({ cookies }) => {
		const state = generateState();
		const codeVerifier = generateCodeVerifier();

		cookies.set(STATE_COOKIE, state, {
			secure: !dev,
			path: "/",
			httpOnly: true,
			maxAge: 60 * 10, // 10 min
		});

		cookies.set(VERIFIER_COOKIE, codeVerifier, {
			secure: !dev,
			path: "/",
			httpOnly: true,
			maxAge: 60 * 10, // 10 min
		});

		redirect(303, await createAuthorizationURL(state, codeVerifier, ["email", "roles", "openid", "profile"]));
	},

	signout: async ({ cookies }) => {
		const token = cookies.get(ACCESS_TOKEN_COOKIE);
		if (token) {
			await revokeToken(token);
		}

		cookies.delete(ACCESS_TOKEN_COOKIE, { path: "/" });
		cookies.delete(REFRESH_TOKEN_COOKIE, { path: "/" });
		redirect(303, "/");
	},
};
