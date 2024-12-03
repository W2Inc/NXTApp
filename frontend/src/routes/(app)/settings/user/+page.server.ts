// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { z } from "zod";
import { fail } from "@sveltejs/kit";
import type { PageServerLoad } from "./$types";
import { zod } from "sveltekit-superforms/adapters";
import { message, superValidate } from "sveltekit-superforms";

// ============================================================================

const schema = z.object({
	name: z.string().default("Hello world!"),
	email: z.string().email(),
});

// ============================================================================

export const load: PageServerLoad = async ({ locals }) => {
	const form = await superValidate(zod(schema));
	return { form };
};

export const actions = {
  default: async ({ request }) => {
    const form = await superValidate(request, zod(schema));
    if (!form.valid) {
      return fail(400, { form });
    }

		await new Promise(resolve => setTimeout(resolve, 2000));
    return message(form, 'Form posted successfully!');
  }
};
