// ============================================================================
// W2Inc, Amsterdam 2023-2025, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { z } from "zod";
import { zod } from "sveltekit-superforms/adapters";
import { XGraphV1 } from "@w2inc/xgraph";
import type { Actions, PageServerLoad } from "./$types";
import { validate } from "$lib/utils/form.svelte";

// ============================================================================

const node: XGraphV1.Node = {
	id: 0,
	parentId: -1,
	goals: [],
	children: [
		{
			id: 1,
			parentId: 0,
			goals: [],
			children: [],
		},
		{
			id: 2,
			parentId: 0,
			goals: [
				{
					goalGUID: "d0fd9301-e04c-e67d-865e-a075db322e13",
				},
				{
					goalGUID: "d0fd9301-e04c-e67d-865e-a075db322e13",
				},
				{
					goalGUID: "d0fd9301-e04c-e67d-865e-a075db322e13",
				}
			],
			children: [],
		},
	],
};

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
		const writer = new XGraphV1.Writer();
		writer.serialize(node);
		writer.toArrayBuffer()

		const reader = new XGraphV1.Reader(writer.toArrayBuffer());
		reader.deserialize();
	}
};
