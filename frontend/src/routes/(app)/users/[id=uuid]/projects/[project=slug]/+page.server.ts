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
import { check } from "$lib/utils/check.svelte";
import { problem, type FormState, type PageFormBundle } from "$lib/utils/api.svelte";

// ============================================================================

// async function getReviews(upId: string, locals: App.Locals, params: RouteParams) {
// 	const { data } = await locals.api.GET("/reviews", {
// 		params: { query: { "filter[user_project_id]": upId } },
// 	});

// 	return data ?? [];
// }

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

// export type FormBundle = PageFormBundle<
// 	BackendTypes["ProjectPostRequestDTO"],
// 	BackendTypes["ProjectPatchRequestDto"],
// 	{
// 		thumbnailUrl?: string | File;
// 	}
// >;

export type FormBundle = PageFormBundle<{}, {}, { id: GUID }>;

// ============================================================================
async function getProjectMarkdown(id: GUID, locals: App.Locals) {
	const { data } = await check(
		locals.api.GET("/projects/{id}/markdown/{file}", {
			parseAs: "text",
			params: { path: { id, file: "README.md" } },
		}),
	);

	return data;
}

// ============================================================================

export const load: PageServerLoad = async ({ locals, params, request, parent }) => {
	const { project } = await parent();
	return {
		markdown: await getProjectMarkdown(project.id, locals),
		form: {
			data: {
				id: project.id,
			},
			errors: {},
			isLoading: false,
		} as FormState<FormBundle>,
	};
};

// ============================================================================

export const actions: Actions = {
	subscribe: async ({ locals, request, url, params }) => {
		const form = await request.formData();
		logger.debug(`Form data on ${url}`, form.entries());

		const id = form.get("id")?.toString();
		if (!id)
			return problem({ status: 400, title: "Missing project ID." })

		await subscribeToProject(id, locals, params);
		return success("Subscribed", { form });
	},
	unsubscribe: async ({ locals, request, url, params }) => {
		const form = await request.formData();
		logger.debug(`Form data on ${url}`, form.entries());

		const id = form.get("id")?.toString();
		if (!id)
			return problem({ status: 400, title: "Missing project ID." })

		await unSubscribeToProject(id, locals, params);
		return success("Subscribed", { form });
	},
};
