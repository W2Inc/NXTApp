import { logger } from "$lib/logger";
import type { Actions, PageServerLoad } from "./$types";

export const load: PageServerLoad = async ({ locals }) => {
	const session = await locals.session();
	if (!session) {
		return { feed: [] }
	}
	const { data } = await locals.api.GET("/users/current/feed", {
		params: { query: { "Size": 15 } }
	});

	return {
		feed: data ?? []
	}
};

export const actions: Actions = {
	search: async ({ request }) => {
		console.log("qwdqwd")
		const form = await request.formData();
		const query = form.get("query")?.toString()
		if (!query) return { };
		console.log("SEARCH", query)
		//todo: searhc api
		return {}
	}
};
