import { redirect } from "@sveltejs/kit";
import type { PageServerLoad } from "./$types";

export const load: PageServerLoad = async ({ url }) => {
	const review = {
		id: "6757cc1b-15b3-4e5b-bff9-28a30c6d0977",
		created_at: "2023-03-03T00:00:00Z",
		updated_at: "2023-03-03T00:00:00Z",
		validates_goals: [1, 2, 3],
		rubric_id: 1,
		outcome: "This is the outcome",
		project_id: 1,
		project_slug: "project-example",
		users_project_id: "123e4567-e89b-12d3-a456-426614174000",
		reviewer: {
			id: "6757cc1b-15b3-4e5b-bff9-28a30c6d0977",
			avatar_url: "https://avatars.githubusercontent.com/u/4510610?v=4",
			display_name: "Wizard",
		},
		user: {
			id: "6757cc1b-15b3-4e5b-bff9-28a30c6d0977",
			avatar_url: "https://avatars.githubusercontent.com/u/4510610?v=4",
			display_name: "Nicolas",
		},
		type: "self",
		state: "active",
	};

	const comments = [
		{
			review_id: "123e4567-e89b-12d3-a456-426614174000",
			created_at: "2023-03-03T00:00:00Z",
			updated_at: "2023-03-03T00:00:00Z",
			comment: "This is a comment",
			user: {
				id: "6757cc1b-15b3-4e5b-bff9-28a30c6d0977",
				avatar_url: "https://avatars.githubusercontent.com/u/4510610?v=4",
				display_name: "Wizard"
			}
		},
		{
			review_id: "123e4567-e89b-12d3-a456-426614174000",
			created_at: "2023-03-03T00:00:00Z",
			updated_at: "2023-03-03T00:00:00Z",
			comment: "This is a comment",
			user: {
				id: "6757cc1b-15b3-4e5b-bff9-28a30c6d0977",
				avatar_url: "https://avatars.githubusercontent.com/u/4510610?v=4",
				display_name: "Wizard"
			}
		},
		{
			review_id: "123e4567-e89b-12d3-a456-426614174000",
			created_at: "2023-03-03T00:00:00Z",
			updated_at: "2023-03-03T00:00:00Z",
			comment: "This is a comment",
			user: {
				id: "6757cc1b-15b3-4e5b-bff9-28a30c6d0977",
				avatar_url: "https://avatars.githubusercontent.com/u/4510610?v=4",
				display_name: "Wizard"
			}
		},
		{
			review_id: "123e4567-e89b-12d3-a456-426614174000",
			created_at: "2023-03-03T00:00:00Z",
			updated_at: "2023-03-03T00:00:00Z",
			comment: "This is a comment",
			user: {
				id: "6757cc1b-15b3-4e5b-bff9-28a30c6d0977",
				avatar_url: "https://avatars.githubusercontent.com/u/4510610?v=4",
				display_name: "Wizard"
			}
		},
		{
			review_id: "123e4567-e89b-12d3-a456-426614174000",
			created_at: "2023-03-03T00:00:00Z",
			updated_at: "2023-03-03T00:00:00Z",
			comment: "This is a comment",
			user: {
				id: "6757cc1b-15b3-4e5b-bff9-28a30c6d0977",
				avatar_url: "https://avatars.githubusercontent.com/u/4510610?v=4",
				display_name: "Wizard"
			}
		},
	];

	return {
		review,
		comments
	};
};
