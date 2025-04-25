import { json, redirect } from "@sveltejs/kit";
import type { RequestHandler } from "./$types";
import { logger } from "$lib/logger";

export const GET: RequestHandler = async ({ locals, url, fetch, request }) => {
	const { data, response } = await locals.api.GET("/users", {
		params: {
			query: {
				"filter[display_name]": url.searchParams.get("name") ?? undefined,
			},
		},
	});

	logger.debug(response.status);
	return json(data);
};
