// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { z } from "zod";
import { fail } from "@sveltejs/kit";
import type { PageServerLoad } from "./$types";
import { zod } from "sveltekit-superforms/adapters";
import { message, superValidate } from "sveltekit-superforms";
import { validate } from "$lib/utils/form.svelte";

// ============================================================================

const schema = z.object({
	id: z.string().uuid().readonly(),
	userName: z.string().readonly(),

	email: z.string().email(),

	displayName: z.string(),
	firstName: z.string(),
	lastName: z.string(),
	twitter: z.string().url(),
	linkedin: z.string().url(),
	github: z.string().url(),
	website: z.string().url(),
});

// ============================================================================

export const load: PageServerLoad = async ({ locals, request }) => {
	// TODO: Fill in the form data with already known user data...
	const session = await locals.auth();
	const form = await validate(request, schema);

	form.data = {
		id: session?.user?.id ?? "",
		userName: session?.user?.username ?? "",
		displayName: session?.user?.username ?? "",
		firstName: session?.user?.firstName ?? "",
		lastName: session?.user?.lastName ?? "",
		email: session?.user?.email ?? "",
		website: "",
		linkedin: "",
		twitter: "",
		github: "",
	};

	return { form };
};

export const actions = {
	default: async ({ request }) => {
		const form = await validate(request, schema);
		if (!form.valid) {
			return fail(400, { form });
		}

		await new Promise((resolve) => setTimeout(resolve, 2000));
		return {
			form,
		};
	},
};
