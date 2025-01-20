// ============================================================================
// W2Inc, Amsterdam 2023-2025, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { fail, superValidate } from "sveltekit-superforms";
import type { Actions, PageServerLoad } from "./$types";
import { zod } from "sveltekit-superforms/adapters";
import { z } from "zod";
import { error } from "@sveltejs/kit";
import { Formkit } from "$lib/utils/form.svelte";

// ============================================================================

const schema = z.object({
	name: z.string().nonempty(),
	description: z.string().min(4).max(128),
	markdown: z.string().min(128).max(2048),
	public: z.boolean(),
	enabled: z.boolean(),
	projects: z.array(z.custom<BackendTypes["ProjectDO"]>()),
});

// ============================================================================

export const load: PageServerLoad = async ({ locals, url }) => {
	const form = await superValidate(zod(schema));
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
			projects: projects.data,
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
		projects: [],
	};

	return { form };
};

// ============================================================================

export const actions: Actions = {
	update: async ({ request, locals }) => {
		const form2 = await Formkit.validate(request, schema);
		if (!form2.valid) {
			return Formkit.problem({
				status: 400,
				title: "Nope"
			}, form2);
		}

		const isBadValue = form2.errors.enabled; // String array of errors regarding this entry of the form.
		if (isBadValue) {
			return Formkit.problem({
				status: 400,
				title: "Something is wrong with this!",
				detail: isBadValue
			}, form2);
		}

		return Formkit.success("Project created!")

	},
	create: async ({ request, locals }) => {
		const form = await superValidate(request, zod(schema));
		if (!form.valid) {
			return fail(400, { form });
		}

		console.log(form.data.projects);

		const { response, data } = await locals.api.POST("/goals", {
			body: {
				name: form.data.name,
				description: form.data.description,
				markdown: form.data.markdown,
				public: form.data.public,
				enabled: form.data.enabled,
			},
		});

		if (!response.ok) {
			console.log(data);
			return fail(response.status, data);
		}

		return {
			form,
		};
	},
};
