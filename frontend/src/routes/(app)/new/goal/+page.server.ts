// ============================================================================
// W2Inc, Amsterdam 2023-2025, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { fail, superValidate } from "sveltekit-superforms";
import type { Actions, PageServerLoad } from "./$types";
import { zod } from "sveltekit-superforms/adapters";
import { z } from "zod";
import { error } from "@sveltejs/kit";

// ============================================================================

const schema = z.object({
	name: z.string().nonempty(),
	description: z.string().min(4).max(128),
	markdown: z.string().min(128).max(2048),
	public: z.boolean(),
	enabled: z.boolean(),
	projects: z.string().array().max(5),
});

export const load: PageServerLoad = async ({ locals, url }) => {
	const form = await superValidate(zod(schema));
	if (url.searchParams.has("edit")) {
		const { data, response } = await locals.api.GET("/goals/{id}", {
			params: {
				path: {
					id: url.searchParams.get("edit") ?? "unknown"
				}
			},
		});

		if (!data)
			error(response.status);

		form.data = {
			name: data.name,
			description: data.description,
			markdown: data.markdown,
			public: true,
			enabled: true,
			projects: [],
		};

		return { form };
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
	default: async ({ request, locals }) => {
		const form = await superValidate(request, zod(schema));
		if (!form.valid) {
			return fail(400, { form });
		}

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
