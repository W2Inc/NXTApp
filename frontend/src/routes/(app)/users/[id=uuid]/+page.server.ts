// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { decodeID } from "$lib/utils";
import { error } from "@sveltejs/kit";
import type { PageServerLoad } from "./$types";

// ============================================================================

export const load: PageServerLoad = async ({ locals, params }) => {
	const userId = decodeID(params.id);
	const [userResponse] = await Promise.all([
		locals.api.GET("/users/{id}", {
			params: { path: { id: userId }}
		}),
	]);

	if (!userResponse.data || userResponse.error) {
		error(userResponse.response.status, userResponse.error?.title ?? "Something went wrong...")
	}
	// if (bioResponse.error) {
	// 	error(bioResponse.response.status, bioResponse.error?.title ?? "Something went wrong...")
	// }

	return {
		user: userResponse.data,
		bio: userResponse.data.details?.markdown
	}
};
