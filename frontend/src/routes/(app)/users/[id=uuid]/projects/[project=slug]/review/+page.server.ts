import { logger } from "$lib/logger";
import type { PageServerLoad } from "./$types";

export const load: PageServerLoad = async ({ locals, params, parent }) => {
	const parentData = await parent();
	const { response, data, error: err } = await locals.api.GET("/rubrics", {
		params: { query: { "filter[project_slug]": params.project }}
	});

	logger.debug("Rubrics", data)

	return {
		rubrics: data
	}
};
