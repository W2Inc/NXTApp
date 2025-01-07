// ============================================================================
// W2Inc, Amsterdam 2023-2025, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { superValidate } from "sveltekit-superforms";
import type { Actions, PageServerLoad } from "./$types";
import { zod } from "sveltekit-superforms/adapters";
import { z } from "zod";

// ============================================================================

const schema = z.object({
	name: z.string(),
	description: z.string(),
	markdown: z.string(),
	projects: z.string().array().max(5),
});

export const load: PageServerLoad = async ({ locals }) => {
	const form = await superValidate(zod(schema));

	form.data = {
		name: "",
		description: "",
		markdown: "",
		projects: ["1", "2"]
	};

	return { form };
};

// ============================================================================

export const actions: Actions = {
	default: async ({ request }) => {
		const form = await superValidate(request, zod(schema));
		console.log(form.data);
		return {
			form
		}
	}
};
