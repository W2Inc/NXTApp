// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { error } from "@sveltejs/kit";
import type { PageServerLoad, RouteParams } from "./$types";
import { Constants, decodeID } from "$lib/utils";
import { logger } from "$lib/logger";
import type { FetchResponse } from "openapi-fetch";
import { check } from "$lib/utils/check.svelte";

// ============================================================================

async function getUserProjects(
	locals: App.Locals,
	params: URLSearchParams,
	userID: string,
) {
	const name = params.get("search");
	const page = Number(params.get("page") ?? 0);
	const { data } = await check(locals.api.GET("/users/{id}/projects", {
		params: {
			path: { id: userID },
			query: {
				Size: 10,
				Page: !isNaN(page) ? page : undefined,
				"filter[name]": name || undefined,
			},
		},
	}))

	return data!;
}

async function getProjects(locals: App.Locals, params: URLSearchParams) {
	const name = params.get("search");
	const page = Number(params.get("page") ?? 0);
	const slug = params.get("slug");
	const { data } = await check(locals.api.GET("/projects", {
		params: {
			query: {
				Size: Constants.PER_PAGE,
				Page: !isNaN(page) ? page : undefined,
				"filter[slug]": slug || undefined,
				"filter[name]": name || undefined,
			},
		},
	}));

	return data!;
}


// ============================================================================

export const load: PageServerLoad = async ({ locals, url, params, parent }) => {
	const parentData = await parent();
	const subscribed = url.searchParams.get("subscribed") === "true";
	if (subscribed || !parentData.isCurrentUser) {
		return {
			projects: await getUserProjects(locals, url.searchParams, decodeID(params.id)),
		};
	} else {
		return {
			projects: await getProjects(locals, url.searchParams),
		};
	}
};
