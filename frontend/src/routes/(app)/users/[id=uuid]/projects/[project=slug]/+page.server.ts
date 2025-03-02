// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { error, redirect } from "@sveltejs/kit";
import type { Actions, PageServerLoad, RouteParams } from "./$types";
import { decodeID } from "$lib/utils";
import { logger } from "$lib/logger";
import { z } from "zod";
import { success, validate } from "$lib/utils/form.svelte";

// ============================================================================

const schema = z.object({
	id: z.string().uuid(),
});

// ============================================================================

async function getUserProject(locals: App.Locals, params: RouteParams) {
	const { data } = await locals.api.GET("/users/{id}/projects", {
		params: {
			path: { id: decodeID(params.id) },
			query: { "filter[slug]": params.project },
		},
	});

	return data?.at(0);
}

async function getProject(locals: App.Locals, params: RouteParams) {
	const { data } = await locals.api.GET("/projects", {
		params: { query: { "filter[slug]": params.project } },
	});

	return data?.at(0);
}

async function getReviews(upId: string, locals: App.Locals, params: RouteParams) {
	const { data } = await locals.api.GET("/reviews", {
		params: { query: { "filter[user_project_id]": upId } },
	});

	return data ?? [];
}

async function subscribeToProject(projectId: string, locals: App.Locals, params: RouteParams) {
	const { data, error: err } = await locals.api.POST("/users/{id}/projects/{projectId}", {
		params: { path: { id: decodeID(params.id), projectId }, },
	});

	logger.error(err);
	return data!;
}

async function unSubscribeToProject(projectId: string, locals: App.Locals, params: RouteParams) {
	const { data, error: err } = await locals.api.DELETE("/users/{id}/projects/{projectId}", {
		params: { path: { id: decodeID(params.id), projectId }, },
	});

	logger.error(err);
	return data!;
}

// ============================================================================

export const load: PageServerLoad = async ({ locals, params, request }) => {
	const form = await validate(request, schema);
	const [project, userProject] = await Promise.all([
		getProject(locals, params),
		getUserProject(locals, params),
	]);

	if (!project) {
		error(404);
	}

	form.data.id = project.id;

	// Project is still being developed
	if (userProject && userProject.state !== "Active") {
		return {
			userProject: userProject,
			project: project,
			reviews: getReviews(userProject.id, locals, params),
			form
		};
	}

	return {
		userProject: userProject,
		project: project,
		reviews: [],
		form
	};
};

// ============================================================================

export const actions: Actions = {
	subscribe: async ({ locals, request, url, params }) => {
		const form = await validate(request, schema);
		logger.debug(`Form data on ${url}`, form.data);
		await subscribeToProject(form.data.id, locals, params);
		return success("Subscribed", { form });

	},
	unsubscribe: async ({ locals, request, url, params }) => {
		const form = await validate(request, schema);
		logger.debug(`Form data on ${url}`, form.data);
		await unSubscribeToProject(form.data.id, locals, params);
		return success("Unsubscribed", { form });
	},
};
