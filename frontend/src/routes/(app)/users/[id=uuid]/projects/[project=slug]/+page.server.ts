import { error } from "@sveltejs/kit";
import type { PageServerLoad } from "./$types";
import { decodeUUID64 } from "$lib/utils";

export const load: PageServerLoad = async ({ locals, params }) => {
	const project = await locals.api.GET("/projects", {
		params: {
			query: {
				"filter[slug]": params.project
			}
		}
	});

	const userProject = await locals.api.GET("/users/projects", {
		params: {
			query: {
				"filter[user_id]": decodeUUID64(params.id),
				"filter[project_id]": project.data?.at(0)?.slug
			}
		}
	})

	const k = locals.api.GET("/projects", {
		// params: {
		// 	query: {
		// 		"filter[slug]": params.project
		// 	}
		// }
	}).then(response => response.data);

	return { project: project.data ?? null, instance: null, test: k };
};
