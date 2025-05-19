// ============================================================================
// W2Inc, Amsterdam 2023-2025, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { z } from "zod";
import type { Actions, PageServerLoad } from "./$types";
import { validate } from "$lib/utils/form.svelte";

// ============================================================================

const schema = z.object({
	name: z.string(),
	description: z.string(),
	markdown: z.string(),
	public: z.boolean(),
	enabled: z.boolean(),
	kind: z.enum(["Dynamic", "Fixed"])
});

export const load: PageServerLoad = async ({ locals, request }) => {
	const form = await validate(request, schema);

	form.data = {
		name: "",
		description: "",
		markdown: "",
		public: true,
		enabled: true,
		kind: "Fixed"
	};

	return { form };
};

// ============================================================================

export const actions: Actions = {
	default: ({ }) => {
		// const writer = new XGraphV1.Writer();
		// writer.serialize(node);
		// writer.toArrayBuffer()

		// const reader = new XGraphV1.Reader(writer.toArrayBuffer());
		// reader.deserialize();
	}
};
