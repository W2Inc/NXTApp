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
import {z} from "zod/v4";
import {useQuery} from "$lib/utils/url.svelte";

// ============================================================================

const schema = z.object({
	page: z.coerce.number().optional(),
	size: z.coerce.number().optional(),
	search: z.string().optional(),
	filter: z.enum(["available", "subscribed"]).optional(),
});

export type QueryKeys = keyof z.infer<typeof schema>;

export const load: PageServerLoad = async ({ locals, url, params, parent }) => {
	const query = useQuery(url, schema);
	if (query.read("filter") === "subscribed") {
		const { data } = await check(locals.api.GET("/users/{id}/projects", {
			params: {
				path: { id: decodeID(params.id) },
				query: {
					Size: query.read("size"),
					Page: query.read("page"),
					"filter[name]": query.read("search")
				},
			},
		}))

		return {
			projects: data ?? []
		}
	}

	const { data } = await check(locals.api.GET("/projects", {
		params: {
			query: {
				Size: query.read("size"),
				Page: query.read("page"),
				"filter[name]": query.read("search")
			},
		},
	}))

	return {
		projects: data ?? []
	}
};
