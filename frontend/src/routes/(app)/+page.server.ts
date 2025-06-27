// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import type { PageServerLoad } from "./$types";
import { check } from "$lib/utils/check.svelte";
import { Constants } from "$lib/utils";
import { stream } from "$lib/utils/stream.svelte";

// ============================================================================

/** Fetch user feed */
async function fetchFeed(locals: App.Locals) {
	const { data, response } = await check(
		locals.api.GET("/users/current/feed", {
			params: {
				query: {
					"page[index]": 0,
					"page[size]": Constants.PER_PAGE,
					sort: "Descending",
				},
			},
		}),
	);

	return {
		items: data ?? [],
		page: Number(response.headers.get("x-page") ?? 1),
		pages: Number(response.headers.get("x-pages") ?? 1),
	}
}

/** Fetch Spotlights */
async function feetchSpotlights(locals: App.Locals) {
	const lel =  await check(locals.api.GET("/users/current/spotlights")).then(r => r.data ?? []);
	console.log(lel)
	return lel;
}

export const load: PageServerLoad = async ({ locals, cookies }) => {
	return {
		feed: stream(() => fetchFeed(locals)),
		spotlights: stream(() => feetchSpotlights(locals)),
	};
};

// export const actions: Actions = {
// 	search: async ({ request }) => {
// 		console.log("qwdqwd");
// 		const form = await request.formData();
// 		const query = form.get("query")?.toString();
// 		if (!query) return {};
// 		console.log("SEARCH", query);
// 		//todo: searhc api
// 		return {};
// 	},
// };
