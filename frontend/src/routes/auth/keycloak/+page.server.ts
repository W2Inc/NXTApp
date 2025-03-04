// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import * as arctic from "arctic";
import { keycloak } from "$lib/oauth";
import { error, redirect } from "@sveltejs/kit";
import type { Actions, PageServerLoad } from "./$types";
import { dev } from "$app/environment";
import { KC_COOKIE_NAME } from "$env/static/private";

// ============================================================================

export const load: PageServerLoad = async () => redirect(308, "/");
export const actions: Actions = {
	signin: async ({ cookies, setHeaders }) => {
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
	},
	signout: async ({ cookies }) => {
		console.log("ok...?")
		const token = cookies.get(`${KC_COOKIE_NAME}-a`);
		if (token) {
			await keycloak.revokeToken(token);
		}

		cookies.delete(`${KC_COOKIE_NAME}-a`, { path: "/" });
		cookies.delete(`${KC_COOKIE_NAME}-r`, { path: "/" });
		redirect(303, "/");
	},
};
