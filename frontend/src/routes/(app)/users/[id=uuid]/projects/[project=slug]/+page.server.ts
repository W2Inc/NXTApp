// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { error } from "@sveltejs/kit";
import type { PageServerLoad, RouteParams } from "./$types";
import { decodeID } from "$lib/utils";

// ============================================================================

export async function getProject(locals: App.Locals, slug: string) {
	const getProjectResponse = await locals.api.GET("/projects", {
		params: { query: { "filter[slug]": slug } }
	});

	if (err || !data) {
		error()
	}
}

// ============================================================================

export const load: PageServerLoad = async ({ locals, params }) => {

};
