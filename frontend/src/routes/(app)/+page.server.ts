import { logger } from "$lib/logger";
import { error } from "@sveltejs/kit";
import type { Actions, PageServerLoad } from "./$types";
import { check } from "$lib/utils/check.svelte";
import { Constants } from "$lib/utils";

export const load: PageServerLoad = async ({ locals }) => {
	return {
		feed: await check(locals.api.GET("/users/current/feed", {
			params: {
				query: {
					"page[index]": 0,
					"page[size]": Constants.PER_PAGE
				}
			}
		}))
	};
};

export const actions: Actions = {
	search: async ({ request }) => {
		console.log("qwdqwd")
		const form = await request.formData();
		const query = form.get("query")?.toString()
		if (!query) return {};
		console.log("SEARCH", query)
		//todo: searhc api
		return {}
	}
};
