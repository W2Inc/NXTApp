// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { error } from "@sveltejs/kit";
import type { PageServerLoad, RouteParams } from "./$types";
import { Constants, decodeUUID64 } from "$lib/utils";

// ============================================================================

async function fetchProjects(locals: App.Locals, url: URL, id: string) {
	const page = Number(url.searchParams.get("page") ?? 0);
	const slug = url.searchParams.get("slug");
	const name = url.searchParams.get("search");
	const subscribed = url.searchParams.get("subscribed") === "true";

	if (!subscribed) {
		const { data, error: err } = await locals.api.GET("/projects", {
			params: {
				query: {
					Size: Constants.PER_PAGE,
					Page: !isNaN(page) ? page : undefined,
					"filter[slug]": slug || undefined,
					"filter[name]": name || undefined,
				},
			},
		});

		if (err) throw new Error(err.title || "Something went wrong...");
		return data!;
	} else {
		const { data, error: err, response } = await locals.api.GET("/users/{id}/projects", {
			params: {
				path: {
					id
				},
				query: {
					Size: Constants.PER_PAGE,
					Page: !isNaN(page) ? page : undefined,
					"filter[name]": name || undefined,
				},
			},
		});


		if (err) throw new Error(err.title || "Something went wrong...");
		return data;
	}
}

// ============================================================================

export const load: PageServerLoad = async ({ locals, url, params }) => {
	return {
		projects: fetchProjects(locals, url, decodeUUID64(params.id)),
	};
};
