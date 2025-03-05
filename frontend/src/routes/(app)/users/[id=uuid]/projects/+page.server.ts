// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { error } from "@sveltejs/kit";
import type { PageServerLoad, RouteParams } from "./$types";
import { Constants, decodeID } from "$lib/utils";
import { logger } from "$lib/logger";

// ============================================================================

// async function fetchProjects(locals: App.Locals, url: URL, id: string) {
// 	const page = Number(url.searchParams.get("page") ?? 0);
// 	const slug = url.searchParams.get("slug");
// 	const name = url.searchParams.get("search");
// 	const subscribed = url.searchParams.get("subscribed") === "true";

// 	if (!subscribed) {
// 		const {
// 			data,
// 			error: err,
// 			response,
// 		} = await locals.api.GET("/projects", {
// 			params: {
// 				query: {
// 					Size: Constants.PER_PAGE,
// 					Page: !isNaN(page) ? page : undefined,
// 					"filter[slug]": slug || undefined,
// 					"filter[name]": name || undefined,
// 				},
// 			},
// 		});

// 		logger.debug(`Response => ${response.url}`, { data });
// 		return data!;
// 	} else if (!) {
// 		const {
// 			data,
// 			error: err,
// 			response,
// 		} = await locals.api.GET("/users/{id}/projects", {
// 			params: {
// 				path: {
// 					id,
// 				},
// 				query: {
// 					Size: Constants.PER_PAGE,
// 					Page: !isNaN(page) ? page : undefined,
// 					"filter[name]": name || undefined,
// 				},
// 			},
// 		});

// 		logger.debug(`Response => ${response.url}`, { data });
// 		return data;
// 	}
// }

async function getUserProjects(locals: App.Locals, params: URLSearchParams, userID: string) {
	const name = params.get("search");
	const page = Number(params.get("page") ?? 0);
	const { data, error: err, response } = await locals.api.GET("/users/{id}/projects", {
		params: {
			path: { id: userID },
			query: {
				Size: 10,
				Page: !isNaN(page) ? page : undefined,
				"filter[name]": name || undefined,
			},
		},
	});

	if (err || !data) {
		error(response.status, err?.title ?? "Something went wrong...");
	}

	return data;
}

async function getProjects(locals: App.Locals, params: URLSearchParams) {
	const name = params.get("search");
	const page = Number(params.get("page") ?? 0);
	const slug = params.get("slug");
	const { data, error: err, response } = await locals.api.GET("/projects", {
		params: {
			query: {
				Size: Constants.PER_PAGE,
				Page: !isNaN(page) ? page : undefined,
				"filter[slug]": slug || undefined,
				"filter[name]": name || undefined,
			},
		},
	});

	if (err || !data) {
		error(response.status, err?.title ?? "Something went wrong...");
	}

	return data;
}

// ============================================================================

export const load: PageServerLoad = async ({ locals, url, params, parent }) => {
	const parentData = await parent();
	const subscribed = url.searchParams.get("subscribed") === "true";
	if (subscribed || !parentData.isCurrentUser) {
		return {
			projects: getUserProjects(locals, url.searchParams, decodeID(params.id))
		}
	} else {
		return {
			projects: getProjects(locals, url.searchParams)
		}
	}
};
