import { Constants } from "$lib/utils";
import { error } from "@sveltejs/kit";
import type { PageServerLoad } from "./$types";
import { logger } from "$lib/logger";

async function getUsers(locals: App.Locals, params: URLSearchParams) {
	const name = params.get("search");
	const page = Number(params.get("page") ?? 0);
	const { data, error: err, response } = await locals.api.GET("/users", {
		params: {
			query: {
				Size: Constants.PER_PAGE,
				Page: !isNaN(page) ? page : undefined,
				"filter[display_name]": name || undefined,
			},
		},
	});

	if (err || !data) {
		error(response.status, err?.title ?? "Something went wrong...");
	}

	return data;
}

// ============================================================================

export const load: PageServerLoad = async ({ locals, url, parent }) => {
	return {
		users: await getUsers(locals, url.searchParams)
	}
};
