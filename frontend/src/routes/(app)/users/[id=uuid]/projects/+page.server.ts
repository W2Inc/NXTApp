// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { error } from "@sveltejs/kit";
import type { PageServerLoad, RouteParams } from "./$types";
import { Constants } from "$lib/utils";

// ============================================================================

export const load: PageServerLoad = async ({ locals, url }) => {
	const fetchProjects = async (url: URL) => {
		const page = Number(url.searchParams.get("page") ?? undefined);
		const {
			response,
			data,
			error: err,
		} = await locals.api.GET("/projects", {
			params: {
				query: {
					Size: Constants.PER_PAGE,
					Page: Number.isNaN(page) ? undefined : page,
					"filter[slug]": url.searchParams.get("slug") ?? undefined,
					"filter[name]": url.searchParams.get("qq") ?? undefined,
				},
			},
		});

		if (err) error(err.status!, err.title ?? "Something went wrong...");
		return {
			data: data!,
			pagination: {
				pages: Number(response.headers.get("X-Pages")!),
				perPage: Constants.PER_PAGE,
			} as PaginationMeta,
		};
	};

	return {
		projects: fetchProjects(url),
	};
};
