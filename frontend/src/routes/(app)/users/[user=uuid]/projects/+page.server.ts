// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import api from "$lib/api/api";
import type { components } from "$lib/api/types";
import { error, type NumericRange } from "@sveltejs/kit";
import type { PageServerLoad } from "./$types";

// ============================================================================

export const load: PageServerLoad = async ({ url }) => {
	//const parentData = await parent();
	//const { response, data, error: err } = await api.GET("/users/{id}/projects", {
	//	params: { path: { id: parentData.user.id } },
	//	fetch,
	//});
	//if (!response.ok || err) {
	//	const status = response.status as NumericRange<400, 599>;
	//	error(status, err?.message ?? "Unknown error");
	//}

	//return {
	//	projects: data,
	//};

	const requestPage = Number(url.searchParams.get("page") ?? "0");
	const pages: components["schemas"]["Project"][][] = [
		[
			{
				created_at: "2023-03-03T00:00:00Z",
				description: "This is a project description",
				id: 1,
				learning_goal_ids: [1, 2, 3],
				maintainer_ids: ["1", "2", "3"],
				name: "Project 1",
				owner_id: "1",
				project_git_info_id: 1,
				slug: "project-1",
				subject_markdown: "This is a markdown",
				tag_ids: [1, 2, 3],
				updated_at: "2023-03-03T00:00:00Z",
				visibility: "public",
			},
			{
				created_at: "2023-03-03T00:00:00Z",
				description: "This is a project description",
				id: 1,
				learning_goal_ids: [1, 2, 3],
				maintainer_ids: ["1", "2", "3"],
				name: "Project 2",
				owner_id: "1",
				project_git_info_id: 1,
				slug: "project-2",
				subject_markdown: "This is a markdown",
				tag_ids: [1, 2, 3],
				updated_at: "2023-03-03T00:00:00Z",
				visibility: "public",
			},
		],
		[
			{
				created_at: "2023-03-03T00:00:00Z",
				description: "This is a project description",
				id: 2,
				learning_goal_ids: [1, 2, 3],
				maintainer_ids: ["1", "2", "3"],
				name: "Project 3",
				owner_id: "1",
				project_git_info_id: 1,
				slug: "project-3",
				subject_markdown: "This is a markdown",
				tag_ids: [1, 2, 3],
				updated_at: "2023-03-03T00:00:00Z",
				visibility: "public",
			},
			{
				created_at: "2023-03-03T00:00:00Z",
				description: "This is a project description",
				id: 2,
				learning_goal_ids: [1, 2, 3],
				maintainer_ids: ["1", "2", "3"],
				name: "Project 4",
				owner_id: "1",
				project_git_info_id: 1,
				slug: "project-4",
				subject_markdown: "This is a markdown",
				tag_ids: [1, 2, 3],
				updated_at: "2023-03-03T00:00:00Z",
				visibility: "public",
			},
		],
		[
			{
				created_at: "2023-03-03T00:00:00Z",
				description: "This is a project description",
				id: 3,
				learning_goal_ids: [1, 2, 3],
				maintainer_ids: ["1", "2", "3"],
				name: "Project 5",
				owner_id: "1",
				project_git_info_id: 1,
				slug: "project-5",
				subject_markdown: "This is a markdown",
				tag_ids: [1, 2, 3],
				updated_at: "2023-03-03T00:00:00Z",
				visibility: "public",
			},
		],
	];

	return {
		projects: pages[requestPage] ?? [],
	};
};
