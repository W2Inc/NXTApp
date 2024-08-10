import { redirect } from "@sveltejs/kit";
import type { PageServerLoad } from "./$types";

export const load: PageServerLoad = async ({ url }) => {
	let page = 0;
	if (url.searchParams.has("page")) {
		page = Number(url.searchParams.get("page") ?? 0);
	}

	const reviews = [
		[
			{
				id: "123e4567-e89b-12d3-a456-426614174000",
				created_at: "2023-03-03T00:00:00Z",
				updated_at: "2023-03-03T00:00:00Z",
				validates_goals: [1, 2, 3],
				rubric_id: 1,
				outcome: "This is the outcome",
				users_project_id: "123e4567-e89b-12d3-a456-426614174000",
				project_id: 1,
				reviewer: {
					id: "123e4567-e89b-12d3-a456-426614174000",
					avatar_url: "https://avatars.githubusercontent.com/u/4510610?v=4",
					display_name: "Wizard",
				},
				user: {
					id: "123e4567-e89b-12d3-a456-426614174000",
					avatar_url: "https://avatars.githubusercontent.com/u/4510610?v=4",
					display_name: "Nicolas",
				},
				type: "self",
				state: "active",
			},
		],
		[
			{
				id: "123e4567-e89b-12d3-a456-426614174000",
				created_at: "2023-03-03T00:00:00Z",
				updated_at: "2023-03-03T00:00:00Z",
				validates_goals: [1, 2, 3],
				rubric_id: 1,
				outcome: "This is the outcome",
				users_project_id: "123e4567-e89b-12d3-a456-426614174000",
				project_id: 1,
				project_slug: "project-example",
				reviewer: {
					id: "123e4567-e89b-12d3-a456-426614174000",
					avatar_url: "https://avatars.githubusercontent.com/u/4510610?v=4",
					display_name: "Wizard",
				},
				user: {
					id: "123e4567-e89b-12d3-a456-426614174000",
					avatar_url: "https://avatars.githubusercontent.com/u/4510610?v=4",
					display_name: "Nicolas",
				},
				type: "self",
				state: "active",
			},
		],
	];

	return {
		reviews: reviews[page] ?? [],
	};
};
