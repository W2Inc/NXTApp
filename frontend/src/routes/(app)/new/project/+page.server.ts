// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { z } from "zod";
import type { PageServerLoad, RequestEvent } from "./$types";
import { validate } from "$lib/utils/form.svelte";
import { error } from "@sveltejs/kit";

// Form Schema
// ============================================================================

const schema = z.object({
	name: z.string().max(128),
	markdown: z.string().max(2048),
	projectID: z.string().uuid().optional(),
	description: z.string().max(128),
	thumbnailUrl: z.string().url().optional(),
	maxMembers: z.number().min(1).max(5).default(1),
	enabled: z.boolean().default(true),
	public: z.boolean().default(false),
	rubricID: z.string().uuid().optional()
	// gitUrl: z.string().url().optional(),
	// gitBranch: z.string().url().optional(),
	// file: z
	// 	.instanceof(File, { message: "Please upload a file." })
	// 	.refine((f) => f.size < 100_000, "Max 100 kB upload size."),
});

// ============================================================================

async function fetchProject(locals: App.Locals, id: string) {
	const [ project, rubric ] = await Promise.all([
		locals.api.GET("/projects/{id}", {
			params: { path: { id } }
		}),
		locals.api.GET("/rubrics", {
			params: { query: { "filter[project_id]": id } }
		})
	]);

	if (project.error || !project.data || rubric.error) {
		error(project.response.status, "Something went wrong fetching the project...");
	}

	return {
		project: project.data,
	}
}

// ============================================================================

export const load: PageServerLoad = async ({ request, locals, url }) => {
	const form = await validate(request, schema);
	// if (url.searchParams.has("edit")) {
	// 	const [ project, rubric ] = await fetchProject(locals, url.searchParams.get("edit")!)
	// 	return { form };
	// }

	form.data = {
		name: "",
		description: "",
		markdown: "",
		maxMembers: 1,
		enabled: true,
		public: true,
	};

	return { form };
};
