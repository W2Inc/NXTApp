import { error } from "@sveltejs/kit";
import type { PageServerLoad } from "./$types";
import { decodeUUID64 } from "$lib/utils";

export const load: PageServerLoad = async ({ locals, params }) => {
	const fetchProjects = async () => {
		const { data, error: err } = await locals.api.GET("/projects", {
			params: {
				query: {
					"filter[slug]": params.project,
				},
			},
		});

		if (err) error(err.status!, err.title ?? "Something went wrong...");
		return {
			data: data!,
		};
	};

	return { project: fetchProjects() };
};
