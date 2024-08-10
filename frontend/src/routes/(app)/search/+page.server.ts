// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import api from "$lib/api/api";
import type { PageServerLoad } from "./$types";
import { error, type NumericRange } from "@sveltejs/kit";

// ============================================================================

export const load: PageServerLoad = async ({ url, fetch, depends }) => {
	if (!url.searchParams.has("q"))
		return error(400, "No search query");

	const query = url.searchParams.get("q");
	if (!query)
		return {
			valid: false,
			results: [],
		};

	const {
		response,
		data,
		error: err,
	} = await api.GET("/search/", {
		params: { query: { query } },
		fetch,
	});

	if (!response.ok || err) {
		const status = response.status as NumericRange<400, 500>;
		return error(status, response.statusText);
	}
	return {
		valid: true,
		results: data,
	};
};
