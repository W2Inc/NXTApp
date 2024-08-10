import type { components } from "$lib/api/types";
import type { LayoutServerLoad } from "./$types";

export const load: LayoutServerLoad = async ({ parent, locals, params }) => {
	const data = await parent();
	const project: components["schemas"]["Project"] = {
		created_at: "2023-03-03T00:00:00Z",
		description: "This is the description",
		id: 1,
		learning_goal_ids: [1, 2, 3],
		maintainer_ids: ["123e4567-e89b-12d3-a456-426614174000"],
		name: "Rust Webserver",
		owner_id: "123e4567-e89b-12d3-a456-426614174000",
		project_git_info_id: 1,
		slug: "project-example",
		subject_markdown: "Project markdown content goes here.",
		tag_ids: [1, 2, 3],
		updated_at: "2023-03-03T00:00:00Z",
		visibility: "public",
	};

	const usersProject = {
		id: "123e4567-e89b-12d3-a456-426614174000",
		created_at: "2023-03-03T00:00:00Z",
		updated_at: "2023-03-03T00:00:00Z",
		project_id: 1,
		status: "active", // active, in-review, completed
		commit: "a3fj1dsf3",
		branch: "main",
		// To whom this instance belongs
		user_id: "123e4567-e89b-12d3-a456-426614174000",
		repo_url: "https://github.com/codam-coding-college/MLX42",
		users: [ // Ref to all users participating in this instance (including owner!)
			{
				//TODO: Should we make this a simple thing in the model?
				// For instance we could use this for learning goals where i need the ID and the name and optionally a thumbnail
				id: "6757cc1b-15b3-4e5b-bff9-28a30c6d0977",
				avatar_url: "https://avatars.githubusercontent.com/u/4510610?v=4",
				display_name: "Wizard",
			},
			{
				id: "6757cc1b-15b3-4e5b-bff9-28a30c6d0977",
				avatar_url: "https://avatars.githubusercontent.com/u/4510610?v=4",
				display_name: "Nicolas",
			},
		],
	};

	const reviews = [
		{
			id: "123e4567-e89b-12d3-a456-426614174000",
			created_at: "2023-03-03T00:00:00Z",
			updated_at: "2023-03-03T00:00:00Z",
			validates_goals: [1, 2, 3],
			rubric_id: 1,
			outcome: "This is the outcome",
			project_id: 1,
			project_slug: "project-example",
			users_project_id: "123e4567-e89b-12d3-a456-426614174000",
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
		{
			id: "123e4567-e89b-12d3-a456-426614174000",
			created_at: "2023-03-03T00:00:00Z",
			updated_at: "2023-03-03T00:00:00Z",
			validates_goals: [1, 2, 3],
			rubric_id: 1,
			outcome: "This is the outcome",
			project_id: 1,
			project_slug: "project-example",
			users_project_id: "123e4567-e89b-12d3-a456-426614174000",
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
			type: "peer",
			state: "active",
		},
	];

	// TODO: This is a temporary solution to get the user id from the URL
	// We should instead check if the user is in the usersProject.users array
	const isMyProject = data.me?.data.id === params.user;

	return {
		isMyProject,
		project,
		usersProject,
		reviews,
	};
};
