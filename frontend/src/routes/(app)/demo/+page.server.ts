import { z } from "zod";
import type { Actions, PageServerLoad } from "./$types";
import { validate, success, problem } from "$lib/utils/form.svelte";

const schema = z.object({
	demo: z.string().startsWith("a"),
	projects: z.array(z.string()).min(1).max(5), // Array with min 1 and max 5 items
});

export const load: PageServerLoad = async ({ request }) => {
	const form = await validate(request, schema);

	form.data = {
		demo: "Hello!",
		projects: [
			"123e4567-e89b-12d3-a456-426614174000",
			"123e4567-e89b-12d3-a456-426614174000",
			"123e4567-e89b-12d3-a456-426614174000",
			"123e4567-e89b-12d3-a456-426614174000",
		],
	};

	return { form };
};

// Form endpoints
export const actions: Actions = {
	default: async ({ request }) => {
		const form = await validate(request, schema);

		if (form.valid) {
			return success("Well done!", { form });
		}
		return problem(400, "Form is invalid. Please check & resubmit!", { form });
	},
};
