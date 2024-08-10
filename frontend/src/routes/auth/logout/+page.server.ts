// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { apiURL } from "$lib/api/api";
import {
	redirect,
	error,
	type Actions,
	type Action,
	type NumericRange,
} from "@sveltejs/kit";
import type { PageServerLoad } from "./$types";

// ============================================================================

export const load: PageServerLoad = async () => redirect(301, "/auth/login");

// ============================================================================

const logout: Action = async ({ cookies, fetch, locals }) => {
	if (locals.session) locals.session = null;
	cookies.delete("SESSION_COOKIE", { path: "/" });

	const response = await fetch(`${apiURL}/logout`);
	if (!response.ok && response.status !== 401)
		throw error(response.status as NumericRange<400, 599>, response.statusText);
	throw redirect(301, "/auth/login");
};

export const actions: Actions = {
	default: logout,
};
