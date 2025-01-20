// ============================================================================
// W2Inc, Amsterdam 2023-2025, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import type { Actions, PageServerLoad } from "./$types";
import { zod } from "sveltekit-superforms/adapters";
import { z } from "zod";
import { error } from "@sveltejs/kit";
import { problem, success, validate } from "$lib/utils/form.svelte";

// ============================================================================

const schema = z.object({
	name: z.string().nonempty(),
	description: z.string().min(4).max(128),
	markdown: z.string().min(128).max(2048),
	public: z.boolean().optional().default(false),
	enabled: z.boolean().optional().default(false),
	projects: z.string().array().default([]),
});

// ============================================================================

export const load: PageServerLoad = async ({ locals, url, request }) => {
	const form = await validate(request, schema);
	const edit = url.searchParams.has("edit");
	if (edit) {
		const id = url.searchParams.get("edit")!;
		const [goal, projects] = await Promise.all([
			locals.api.GET("/goals/{id}", {
				params: { path: { id } },
			}),
			locals.api.GET("/goals/{id}/projects", {
				params: { path: { id } },
			}),
		]);

		if (!goal.data || !projects.data) {
			error(404, "Goal or projects not found");
		}

		form.data = {
			...goal.data,
			projects: projects.data.map((p) => p.id),
			public: true,
			enabled: true,
		};

		return { form, edit };
	}

	form.data = {
		name: "",
		description: "",
		markdown: "",
		public: true,
		enabled: true,
		projects: ["1"],
	};

	return { form };
};

// ============================================================================

export const actions: Actions = {
	update: async ({ request, locals }) => {
		const form = await validate(request, schema);
		if (!form.valid) {
			return problem(400, "Nope", { form });
		}

		return success("Project created!", { form });
	},
	create: async ({ request, locals }) => {
		const form = await validate(request, schema);
		if (!form.valid) {
			return problem(400, "Invalid form, check errors", { form });
		}

		const { response, data, error } = await locals.api.POST("/goals", {
			body: {
				name: form.data.name,
				description: form.data.description,
				markdown: form.data.markdown,
				public: form.data.public,
				enabled: form.data.enabled,
			},
		});

		if (error) {
			return problem(400, error.title ?? "Something went wrong!", { form });
		}

		return success("Project created!", { form });
	},
};
