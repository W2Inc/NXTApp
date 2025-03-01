// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { error } from "@sveltejs/kit";
import type { PageServerLoad, RouteParams } from "./$types";
import { decodeID } from "$lib/utils";

// ============================================================================

export const load: PageServerLoad = async ({ locals, params }) => {
	// First try to get the user's project subscription
	const { data: userProjectData, error: userProjectError } = await locals.api.GET("/users/{id}/projects", {
		params: {
			path: { id: decodeID(params.id) },
			query: { "filter[slug]": params.project },
		},
	});

	let userProject = userProjectData?.at(0);
	let project = userProject?.project;

	// const {  } = await locals.api.GET("/users/projects/{id}/reviews", {
	// 	params: {
	// 		query: { "filter[slug]": params.project },
	// 	},
	// });


	// If we get a 404, it means the user isn't subscribed, so let's try to fetch the project directly
	if (userProjectError?.status === 404) {
		const { data: projectData, error: projectError } = await locals.api.GET("/projects", {
			params: {
				query: { "filter[slug]": params.project },
			},
		});

		if (projectError) {
			error(projectError.status || 500, projectError.title || "Something went wrong...");
		}

		project = projectData?.at(0);
	} else if (userProjectError) {
		error(userProjectError.status || 500, userProjectError.title || "Something went wrong...");
	}

	return {
		userProject,
		project: project!
	};
};
