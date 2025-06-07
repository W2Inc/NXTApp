// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import {Constants, decodeID} from "$lib/utils";
import {error} from "@sveltejs/kit";
import type {PageServerLoad} from "./$types";
import {logger} from "$lib/logger";
import {z} from "zod/v4";
import {useQuery} from "$lib/utils/url.svelte";
import {check} from "$lib/utils/check.svelte";

// ============================================================================

const schema = z.object({
	page: z.coerce.number().optional(),
	size: z.coerce.number().optional(),
	search: z.string().optional(),
	filter: z.enum(["available", "subscribed"]).optional(),
});

export type QueryKeys = keyof z.infer<typeof schema>;

// ============================================================================


export const load: PageServerLoad = async ({locals, params, url}) => {
	const userId = decodeID(params.id);
	const query = useQuery(url, schema);

	if (query.read("filter") === "available") {
		const { response, data } = await check(locals.api.GET("/goals", {
			params: {
				query: {
					"page[size]": query.read("size"),
					"page[index]": query.read("page"),
					"filter[name]": query.read("search"),
				}
			}
		}));

		logger.debug({ lel: Number(response.headers.get("X-Pages")) })

		return {
			size: Number(response.headers.get("X-Pages")),
			goals: data ?? []
		}
	} else {
		const { response, data } = await check(locals.api.GET("/users/{id}/goals", {
			params: {
				path: { id: userId },
				query: {
					"page[size]": query.read("size"),
					"page[index]": query.read("page"),
					"filter[name]": query.read("search"),
				}
			}
		}));

		logger.debug({ lel: Number(response.headers.get("X-Pages")) })

		return {
			size: Number(response.headers.get("X-Pages")),
			goals: data ?? []
		}
	}
};
