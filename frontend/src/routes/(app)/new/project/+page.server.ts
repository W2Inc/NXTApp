// ============================================================================
// W2Inc, Amsterdam 2023-2025, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { z } from "zod";
import { message, superValidate } from "sveltekit-superforms";
import { zod } from "sveltekit-superforms/adapters";
import type { Actions, PageServerLoad } from "./$types";
import { fail } from "@sveltejs/kit";
import createClient from "openapi-fetch";

// ============================================================================

const schema = z.object({
	name: z.string(),
	description: z.string(),
	markdown: z.string(),
	thumbnailUrl: z.string().url().nullable(),
	maxMembers: z.number().min(1).max(5).default(1),
	enabled: z.boolean().default(true),
	public: z.boolean().default(false),
	gitUrl: z.string().refine((v) => {
		if (!v.endsWith(".git")) return "Must end with '.git'";
	}),
	gitBranch: z.string().default("master"),
});

export const load: PageServerLoad = async ({ locals }) => {
	const form = await superValidate(zod(schema));

	form.data = {
		name: "",
		thumbnailUrl: null,
		description: "",
		markdown: "",
		maxMembers: 1,
		enabled: true,
		public: true,
		gitUrl: "",
		gitBranch: "master",
	};

	return { form };
};

// ============================================================================

export const actions: Actions = {
	default: async ({ request, locals, fetch }) => {
		const session = await locals.auth();
		const form = await superValidate(request, zod(schema));
		if (!form.valid) {
			return fail(400, { form });
		}

		const { data, error, response } = await locals.api.POST("/projects", {
			fetch: fetch,
			// headers: {
			// 	"Authorization": `Bearer ${session?.access_token}`
			// },
			body: {
				name: form.data.name,
				description: form.data.description,
				markdown: form.data.markdown,
				maxMembers: form.data.maxMembers,
				thumbnailUrl: form.data.thumbnailUrl ?? "",
				public: form.data.public,
				enabled: form.data.enabled,
				gitInfo: {
					gitUrl: form.data.gitUrl,
					gitBranch: form.data.gitBranch,
				},
			},
		});

		if (!response.ok) {
			return message(form, response.statusText, {
				status: response.status,
			});
			// return fail(response.status, { form });
		}

		return {
			form,
		};
	},
};
