// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { z } from "zod";
import type { Actions, PageServerLoad, RequestEvent } from "./$types";
import { problem, success, validate } from "$lib/utils/form.svelte";
import { error } from "@sveltejs/kit";
import { logger } from "$lib/logger";

// Form Schema
// ============================================================================

const schema = z.object({
	name: z
		.string()
		.max(64)
		.regex(
			/^[a-zA-Z0-9-_]+$/,
			"Only letters, numbers, hyphens and underscores are allowed",
		),
	markdown: z.string().min(128).max(2048),
	description: z.string().min(1).max(128),
	maxMembers: z.number().min(1).max(5),
	enabled: z.coerce.boolean(),
	public: z.coerce.boolean(),
	gitUrl: z.string().optional(),
	gitBranch: z.string().optional(),
	gitKind: z.enum(["Thirdparty" , "Managed" , "Github"]),
	image: z
		.union([
			z.string(),
			z
				.instanceof(File, { message: "Please upload a file." })
				.refine((file) => file.size < 100_000, "Max 100 kB upload size."),
		])
		.optional(),
});

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
		error(project.response.status, "Something went wrong fetching the project...");
	}

	return {
		project: project.data,
	};
}

// ============================================================================

export const load: PageServerLoad = async ({ request, locals, url }) => {
	const form = await validate(request, schema);
	if (url.searchParams.has("edit")) {
		const { error: err, data } = await locals.api.GET("/projects/{id}", {
			params: { path: { id: url.searchParams.get("edit")! } },
		});

		if (err || !data) {
			error(err?.status ?? 500, err?.title ?? "Something went wrong...");
		}

		form.data = {
			name: data.name,
			description: data.description,
			markdown: data.markdown,
			maxMembers: data.maxMembers,
			image: data.thumbnailUrl,
			public: data.public,
			enabled: data.enabled,
		};

		return { form, git: data.gitInfo, entity: data };
	}

	form.data = {
		name: "",
		description: "",
		markdown: "",
		maxMembers: 1,
		public: false,
		gitKind: "Thirdparty",
		enabled: false,
	};

	return { form };
};

// ============================================================================

export const actions: Actions = {
	create: async ({ url, locals, request }) => {
		const session = (await locals.session()) ?? error(401, "No session");
		const form = await validate(request, schema);

		logger.debug(`Submitted Form => ${url}`, form.data)

		if (!form.valid) {
			logger.debug("Invalid form", form.errors);
			return problem(400, "Unable to update project", { form });
		}

		let avatarUrl: string | undefined;

		return success("Created!", { form });
	},
	update: async ({ url, locals, request }) => {
		const session = (await locals.session()) ?? error(401, "No session");
		const form = await validate(request, schema);
		let avatarUrl: string | undefined;

		logger.debug(`Submitted Form => ${url}`, form.data)

		if (!form.valid) {
			logger.debug("Invalid form", form.errors);
			return problem(400, "Unable to update project", { form });
		}

		return success("Updated!", { form });
	},
};
