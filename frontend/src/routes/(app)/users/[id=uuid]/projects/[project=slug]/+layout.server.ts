import { decodeID } from "$lib/utils";
import { error } from "@sveltejs/kit";
import type { LayoutServerLoad, RouteParams } from "./$types";
import { logger } from "$lib/logger";
import { check } from "$lib/utils/check.svelte";

async function getProject(locals: App.Locals, params: RouteParams) {
	const { data } = await check(
		locals.api.GET("/projects", {
			params: { query: { "filter[slug]": params.project } },
		}),
	);

	return data!.at(0);
}

async function getUserProject(locals: App.Locals, params: RouteParams) {
	const { data} = await check(locals.api.GET("/users/{id}/projects", {
		params: {
			path: { id: decodeID(params.id) },
			query: { "filter[slug]": params.project },
		},
	}));

	return data!.at(0);
}

export const load: LayoutServerLoad = async ({ locals, params }) => {
	const [project, userProject] = await Promise.all([
		getProject(locals, params),
		getUserProject(locals, params),
	]);

	return {
		project: project!,
		userProject,
	};
};
