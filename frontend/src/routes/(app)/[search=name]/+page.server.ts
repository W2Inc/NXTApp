// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import api from "$lib/api/api";
import { error, redirect, type NumericRange } from "@sveltejs/kit";
import type { PageServerLoad } from "./$types";

// ============================================================================

// Allow for quickly searching by name by prefixing the search with @<name>
export const load: PageServerLoad = async ({ params }) => {
	const login = params.search;
	const { response, data, error: err } = await api.GET("/users/");
	if (!response.ok || err) {
		const status = response.status as NumericRange<400, 599>;
		return error(status, err?.message ?? "Error loading user");
	}
	if (data.length === 0) {
		return error(404, "User not found");
	}

	redirect(301, `/users/${data[0]!.id}`);
};
