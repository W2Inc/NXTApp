// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { decodeID } from "$lib/utils";
import { error } from "@sveltejs/kit";
import type { PageServerLoad } from "./$types";
import { logger } from "$lib/logger";

// ============================================================================

export const load: PageServerLoad = async ({ locals, params, url }) => {
	const userId = decodeID(params.id);
	const { response, data, error: err } = await locals.api.GET("/users/{id}/goals", {
		params: { path: { id: userId }}
	});

	logger.debug(`Response => ${response.url}`,{ data });

	// if (!data || err) {
	// 	error(500, "Something went wrong...");
	// }

	return {
		goals: data
	}
};
