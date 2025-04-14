// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { z } from "zod";
import type { Actions, PageServerLoad, RequestEvent } from "./$types";
import { S3_BUCKET, S3_ENDPOINT } from "$env/static/private";
import { createDTOSchema, preprocessForm, problem, success, type PageFormBundle } from "$lib/utils/api.svelte";
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

/*
					const MAX_FILE_SIZE = 5 * 1024 * 1024; // 5MB
					if (value.size > MAX_FILE_SIZE) {
						throw new Error("File size too large. Maximum size is 5MB.");
					}

					// Get file extension
					const fileNameParts = value.name.split('.');
					const ext = fileNameParts.length > 1 ? fileNameParts.pop() : null;

					if (!ext) {
						throw new Error("File must have an extension.");
					}

					// Generate unique filename
					const fileName = `project_thumbnail_${Date.now()}.${ext}`;

					// Upload file to S3
					const file = Bun.s3.file(fileName);
					await file.write(value, { acl: "public-read", bucket: S3_BUCKET });

					// Generate the full URL
					const thumbnailUrl = `${S3_ENDPOINT}/${S3_BUCKET}/${fileName}`;

					return thumbnailUrl;
					*/

const MAX_FILE_SIZE = 5 * 1024 * 1024; // 5MB

export const actions: Actions = {
	create: async ({ url, locals, request }) => {

		// Pre-process the form data and validate it
		type POST = BackendTypes["ProjectPostRequestDto"];
		const [body, fail] = await ensure(preprocessForm<POST>(request.formData(), async (key, value) => {
			if (key === "thumbnailUrl" && value instanceof File && value.size > 0) {
				console.log("File", value, value.size, value.name);
				if (value.size > MAX_FILE_SIZE) {
					throw new Error("File size too large. Maximum size is 5MB.");
				}

				// Check that it has an extension
				const parts = value.name.split('.');
				const ext = parts.length > 1 ? parts.pop() : null;
				if (!ext) {
					throw new Error("File must have an extension.");
				}

				// Upload file to S3
				const fileName = `${Bun.MD5.hash(value.name).toString()}.${ext}`;
				const file = Bun.s3.file(fileName);
				await file.write(value, { acl: "public-read", bucket: S3_BUCKET });

				value = `${S3_ENDPOINT}/${S3_BUCKET}/${fileName}`;
				console.log("File uploaded", value);
			}

			return value;
		}));

		if (fail) {
			logger.debug("Invalid form", fail);
			return problem(400, "Unable to create project", { error: fail });
		}

		logger.debug(`Submitted Form => ${url}`, body);
		const { response, data, error: err } = await locals.api.POST("/projects", { body });
		if (err || !data || !response.ok) {
			logger.error(err);
			//@ts-ignore
			return problem(response.status, err?.title ?? "S", err.errors);
		}

		// return success("Created!", {
		// 	form: data,
		// } `/new/project?edit=${data.id}`);



		// For debugging purposes
		// console.log('Form data:', Object.fromEntries(formData.entries()), response, err);
		// Create form object from form data using FormBundle keys
		// const form: BackendTypes["ProjectPostRequestDto"] = {
		// 	name: formData.get('name')!.toString(),
		// 	description: formData.get('description')!.toString(),
		// 	markdown: formData.get('markdown')?.toString() || "",
		// 	maxMembers: parseInt(formData.get('maxMembers')?.toString() || "1", 10),
		// 	public: formData.get('public') === 'true',
		// 	enabled: formData.get('enabled') === 'true',
		// 	thumbnailUrl: formData.get('thumbnailUrl') || null,
		// };

		// const thumbnail = form.get('thumbnailUrl');
		// if (thumbnail instanceof File) {
		// 	// TODO: Upload thumbnail to S3 or any other storage service
		// }

		// // Send data to API (exclude thumbnailUrl as it's not part of ProjectPostRequestDto)


	},
	// update: async ({ url, locals, request }) => {
	// 	const session = (await locals.session()) ?? error(401, "No session");
	// 	const form = await validate(request, schema);
	// 	let avatarUrl: string | undefined;

	// 	logger.debug(`Submitted Form => ${url}`, form.data);

	// 	if (!form.valid) {
	// 		logger.debug("Invalid form", form.errors);
	// 		return problem(400, "Unable to update project", { form });
	// 	}

	// 	logger.info("Updating:", url.searchParams.get("edit"), url)
	// 	const { response, data, error: err } = await locals.api.PATCH("/projects/{id}", {
	// 		params: { path: { id: "019504a3-a0e9-70fe-b392-e4ef023addf5" }},
	// 		body: {
	// 			name: form.data.name,
	// 			description: form.data.description,
	// 			markdown: form.data.markdown,
	// 			maxMembers: form.data.maxMembers,
	// 			public: form.data.public,
	// 			enabled: form.data.enabled,
	// 			tags: []
	// 		},
	// 	});

	// 	if (err || !data || !response.ok) {
	// 		logger.error(err);
	// 		return problem(response.status, err?.title ?? "S", { form });
	// 	}

	// 	return success("Updated!", { form });
	// },
	// delete: async ({ url, locals, request }) => {
	// 	const session = (await locals.session()) ?? error(401, "No session");
	// 	const form = await validate(request, schema);
	// 	return success("Project was deleted.", { form }, "/");
	// }
};
