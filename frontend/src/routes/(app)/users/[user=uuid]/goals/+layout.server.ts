// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import api from "$lib/api/api";
import type { components } from "$lib/api/types";
import type { LayoutServerLoad } from "./$types";
import { error, type NumericRange } from "@sveltejs/kit";

// ============================================================================

export const load: LayoutServerLoad = async ({ fetch, parent, url}) => {
	//const { response, data, error } = await api.GET("/users/{id}/learning_goals", {
	//	fetch,
	//	params: { path: { id: params.user } },
	//});
	//if (!response.ok || error) {
	//	const status = response.status as NumericRange<400, 599>;
	//	err(status, error?.message ?? "Failed to fetch learning goals");
	//}

	const requestPage = Number(url.searchParams.get("page") ?? "0");
	const data: components["schemas"]["LearningGoals"][][] = [
		[
			{
				id: 123,
				name: "Learn to code",
				description: "I want to learn to code",
				created_at: "2023-03-03T00:00:00Z",
				updated_at: "2023-03-03T00:00:00Z",
				tag_ids: [1, 2, 3],
				validation_count: 0,
				project_ids: [1, 2, 3],
			},
			{
				id: 123,
				name: "ASSEMBLY",
				description: "I want to learn to ASSEMBLY",
				created_at: "2023-03-03T00:00:00Z",
				updated_at: "2023-03-03T00:00:00Z",
				tag_ids: [1, 2, 3],
				validation_count: 0,
				project_ids: [],
			}
		],
		[
			{
				id: 123,
				name: "Learn to code 2",
				description: "I want to learn to code",
				created_at: "2023-03-03T00:00:00Z",
				updated_at: "2023-03-03T00:00:00Z",
				tag_ids: [1, 2, 3],
				validation_count: 0,
				project_ids: [1, 2, 3],
			}
		],
		[
			{
				id: 123,
				name: "Learn to code 3",
				description: "I want to learn to code",
				created_at: "2023-03-03T00:00:00Z",
				updated_at: "2023-03-03T00:00:00Z",
				tag_ids: [1, 2, 3],
				validation_count: 0,
				project_ids: [1, 2, 3],
			}
		]
	]

	return {
		goals: data[requestPage] ?? [],
	};
};
