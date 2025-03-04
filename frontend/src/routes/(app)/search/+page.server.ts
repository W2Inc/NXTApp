// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { logger } from "$lib/logger";
import type { PageServerLoad } from "./$types";

// ============================================================================

async function searchUsers(locals: App.Locals, query: string) {
	const { data } = await locals.api.GET("/users", {
		params: { query: { "filter[display_name]": query } },
	});

	return data ?? [];
}

// ============================================================================

export const load: PageServerLoad = async ({ url, locals }) => {
	const query = url.searchParams.get("q");
	const type = url.searchParams.get("type");
	logger.debug("Searching...", { query, type });

	if (!query) {
		return {
			query,
			type,
			results: [],
		};
	}

	switch (type) {
		case "user":
			return {
				users: searchUsers(locals, query),
			};
		case "project":
			break;
		case "cursus":
			break;
		case "goal":
			break;
		default:
			break;
	}

	return { query, type, users: [] };
};
