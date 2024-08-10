import api from "$lib/api/api";
import { error } from "@sveltejs/kit";
import type { PageServerLoad } from "./$types";

export const load: PageServerLoad = async ({fetch, url}) => {
	let page = 0;
	if (url.searchParams.has("page")) {
		page = Number(url.searchParams.get("page") ?? 0);
	}

	const { response, data, error: err } = await api.GET("/projects/", {
		fetch,
		params: {
			query: {
				page: page,
				per_page: 10,
				"sort[field]": "updated_at",
			}
		}
	})

	if (!response.ok || err) {
		error(response.status, err?.message ?? response.statusText);
	}

	return {
		projects: data,
	}
};
