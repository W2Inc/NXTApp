import { error } from "@sveltejs/kit";
import type { PageServerLoad } from "./$types";

export const load: PageServerLoad = async ({ locals, params }) => {
	const project = await locals.api.GET("/projects", {
		params: {
			query: {
				"filter[slug]": params.project
			}
		}
	});

	console.log(project.data)

	return { project: project.data ?? null };
};
