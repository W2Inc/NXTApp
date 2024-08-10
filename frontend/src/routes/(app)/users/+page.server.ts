import { redirect } from "@sveltejs/kit";
import type { PageServerLoad } from "./$types";
import api, { verify } from "$lib/api/api";

export const load: PageServerLoad = async ({ fetch, depends }) => {
	depends("uses:page");

	const { response, data, error: err } = await api.GET("/users/", {
		fetch,
		params: {
			query: {
				"page": 1,
				"per_page": 10,
			}
		}
	});

	const users = verify(response, data, err);
	return {
		users,
	};
}
