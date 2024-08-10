// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import api from "$lib/api/api";
import type { RequestHandler } from "@sveltejs/kit";

// ============================================================================

/** Query the search API for results. */
export const GET: RequestHandler = async ({ request, url, fetch }) => {
	const query = url.searchParams.get("q");
	if (!query || !url.searchParams.has("q"))
		return new Response("No search query", { status: 400 });

	const filter = url.searchParams.get("filter");

	const { } = await api.GET("/search/", {
		params: {
			query: { query }
		},
		fetch,
	});



	return new Response();
};
