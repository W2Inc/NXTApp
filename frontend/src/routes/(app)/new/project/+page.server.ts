// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { z } from "zod";
import type { Actions, PageServerLoad, RequestEvent } from "./$types";
import { S3_BUCKET, S3_ENDPOINT } from "$env/static/private";
import { createDTOSchema, extractForm, problem, success, type PageFormBundle } from "$lib/utils/api.svelte";
import { error, redirect } from "@sveltejs/kit";
import { logger } from "$lib/logger";
import { $ } from "bun";
import { ensure } from "$lib/utils";


export type FormBundle = PageFormBundle<
	BackendTypes["ProjectPostRequestDto"],
	BackendTypes["ProjectPatchRequestDto"],
	{
		thumbnailUrl: string | File | null;
	}
>;

const C_DEFAULT_BUNDLE: FormBundle = {
	name: "",
	description: "",
	markdown: "",
	maxMembers: 1,
	public: false,
	enabled: false,
	thumbnailUrl: null,
};

// Form Schema
// ============================================================================



// ============================================================================

// async function fetchProject(locals: App.Locals, id: string) {
// 	const [project, rubric] = await Promise.all([
// 		locals.api.GET("/projects/{id}", {
// 			params: { path: { id } },
// 		}),
// 		locals.api.GET("/rubrics", {
// 			params: { query: { "filter[project_id]": id } },
// 		}),
// 	]);

// 	if (project.error || !project.data || rubric.error) {
// 		error(project.response.status, "Something went wrong fetching the project...");
// 	}

// 	return {
// 		project: project.data,
// 	};
// }

// ============================================================================


// ============================================================================

export const load: PageServerLoad = async ({ request, locals, url }) => {
	if (url.searchParams.has("edit")) {



		return;
	}

	return {
		form: {
			data: {
				image: "",
				name: C_DEFAULT_BUNDLE.name,
				description: C_DEFAULT_BUNDLE.description,
				markdown: C_DEFAULT_BUNDLE.markdown,
				maxMembers: C_DEFAULT_BUNDLE.maxMembers,
				public: C_DEFAULT_BUNDLE.public,
				enabled: C_DEFAULT_BUNDLE.enabled,
			},
			errors: {},
			constraints: {}
		},
	}
};

// ============================================================================


const MAX_FILE_SIZE = 5 * 1024 * 1024; // 5MB
export const actions: Actions = {
	create: async ({ url, locals, request }) => {
		const form = await request.formData();

		//@ts-ignore
		const formData: FormBundle = {
			name: form.get("name")?.toString() ?? "",
			description: form.get("description")?.toString() ?? "",
			markdown: form.get("markdown")?.toString() ?? "",
			maxMembers: Number(form.get("maxMembers")),
			public: form.get("public") === "true",
			enabled: form.get("enabled") === "true",
			thumbnailUrl: null,
		};

		const thumbnail = form.get("thumbnailUrl") as File | null;
		if (thumbnail) {
			const MAX_FILE_SIZE = 5 * 1024 * 1024; // 5MB
			if (thumbnail.size > MAX_FILE_SIZE) {
				throw new Error("File size too large. Maximum size is 5MB.");
			}

			// Get file extension
			const fileNameParts = thumbnail.name.split('.');
			const ext = fileNameParts.length > 1 ? fileNameParts.pop() : null;

			if (!ext) {
				throw new Error("File must have an extension.");
			}

			// Generate unique filename
			const fileName = `thumbnail_${Bun.randomUUIDv7()}.${ext}`;

			const file = Bun.s3.file(fileName);
			await file.write(thumbnail, { acl: "public-read", bucket: S3_BUCKET });
			formData.thumbnailUrl = `${S3_ENDPOINT}/${S3_BUCKET}/${fileName}`;
		}


		console.log("FormData", formData);

		type POST = BackendTypes["ProjectPostRequestDto"];
		const { response, data, error: err } = await locals.api.POST("/projects", {
			body: formData as POST,
		});

		if (err || !data || !response.ok) {
			logger.error(err);
			return problem(response.status, err?.title ?? "S");
		}
	},
};
