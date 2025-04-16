// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { z } from "zod";
import type { Actions, PageServerLoad, RequestEvent } from "./$types";
import { PUBLIC_S3_BUCKET } from "$env/static/public";
import {
	formValueToS3,
	problem,
	success,
	type FormState,
	type PageFormBundle,
} from "$lib/utils/api.svelte";
import { error as kitError, redirect } from "@sveltejs/kit";
import { logger } from "$lib/logger";
import { ensure } from "$lib/utils";

export type FormBundle = PageFormBundle<
	BackendTypes["ProjectPostRequestDto"],
	BackendTypes["ProjectPatchRequestDto"],
	{
		thumbnailUrl?: string | File;
	}
>;

const C_DEFAULT_BUNDLE: FormBundle = {
	name: "",
	description: "",
	markdown: "",
	maxMembers: 1,
	public: false,
	enabled: false,
	thumbnailUrl: undefined,
};

// Form Schema
// ============================================================================

// ============================================================================

async function fetchProject(locals: App.Locals, id: string) {
	const [project, rubric] = await Promise.all([
		locals.api.GET("/projects/{id}", {
			params: { path: { id } },
		}),
		locals.api.GET("/rubrics", {
			params: { query: { "filter[project_id]": id } },
		}),
	]);

	if (project.error || !project.data || rubric.error) {
		kitError(project.response.status, "Something went wrong fetching the project...");
	}

	return {
		project: project.data,
	};
}

// ============================================================================

// ============================================================================

export const load: PageServerLoad = async ({ request, locals, url }) => {
	if (url.searchParams.has("edit")) {
		const { project } = await fetchProject(locals, url.searchParams.get("edit")!)
		return {
			entity: project,
			form: {
				data: {
					name: project.name,
					description: project.description,
					markdown: project.markdown,
					maxMembers: project.maxMembers,
					public: project.public,
					enabled: project.enabled,
					thumbnailUrl: project.thumbnailUrl,
				},
				errors: {},
				isLoading: false,
			} as FormState<FormBundle>
		}
	}

	return {
		entity: null,
		form: {
			data: C_DEFAULT_BUNDLE,
			errors: {},
			isLoading: false,
		} as FormState<FormBundle>
	}
};

// ============================================================================

export const actions: Actions = {
	create: async ({ locals, request }) => {
		const form = await request.formData();
		let [image, issue] = await ensure(formValueToS3<FormBundle>(form, "thumbnailUrl"));
		let errors: Record<string, string[]>;
		if (issue) {
			return problem({
				status: 422,
				title: "Please upload a thumbnail first",
				errors: {
					ThumbnailUrl: [issue.message],
				},
			});
		}

		const { client, url, file } = image!; // TODO: Typescript is a bit dumb here;
		const { data, error } = await locals.api.POST("/projects", {
			body: {
				name: form.get("name")?.toString() ?? "",
				description: form.get("description")?.toString() ?? "",
				markdown: form.get("markdown")?.toString() ?? "",
				maxMembers: Number(form.get("maxMembers")),
				public: form.get("public")?.toString() === "true",
				enabled: form.get("enabled")?.toString() === "true",
				tags: form.getAll("tags").flatMap((v) => v.toString()),
				thumbnailUrl: url
			},
		});

		if (error || !data) {
			logger.error(error);
			return problem({
				status: error?.status ?? 422,
				title: error?.title ?? "Something went wrong...",
				// @ts-ignore
				errors: error.errors,
			});
		}

		await client.write(file, { acl: "public-read", bucket: PUBLIC_S3_BUCKET });
		return success("Project created!", `/new/project?edit=${data.id}`);
	},
	update: async ({ locals, request }) => {
		console.log(request);
		const form = await request.formData();
		const id = form.get("id")?.toString();
		console.log(id);

	}
};
