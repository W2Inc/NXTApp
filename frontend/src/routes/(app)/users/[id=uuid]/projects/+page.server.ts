// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { error } from "@sveltejs/kit";
import type { PageServerLoad, RouteParams } from "./$types";
import { Constants, decodeID } from "$lib/utils";
import { logger } from "$lib/logger";

// ============================================================================

async function fetchProjects(locals: App.Locals, url: URL, id: string) {
	const page = Number(url.searchParams.get("page") ?? 0);
	const slug = url.searchParams.get("slug");
	const name = url.searchParams.get("search");
	const subscribed = url.searchParams.get("subscribed") === "true";

	if (!subscribed) {
		const {
			data,
			error: err,
			response,
		} = await locals.api.GET("/projects", {
			params: {
				query: {
					Size: Constants.PER_PAGE,
					Page: !isNaN(page) ? page : undefined,
					"filter[slug]": slug || undefined,
					"filter[name]": name || undefined,
				},
			},
		});

		logger.debug(`Response => ${response.url}`, { data });
		return data!;
	} else {
		const {
			data,
			error: err,
			response,
		} = await locals.api.GET("/users/{id}/projects", {
			params: {
				path: {
					id,
				},
				query: {
					Size: Constants.PER_PAGE,
					Page: !isNaN(page) ? page : undefined,
					"filter[name]": name || undefined,
				},
			},
		});

		logger.debug(`Response => ${response.url}`, { data });
		return data;
	}
}

// ============================================================================

export const load: PageServerLoad = async ({ locals, url, params }) => {
	return {
		projects: fetchProjects(locals, url, decodeID(params.id)),
	};
};
