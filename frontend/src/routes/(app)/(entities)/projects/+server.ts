import { json } from "@sveltejs/kit";
import type { RequestHandler } from "./$types";

export const GET: RequestHandler = async ({ locals, fetch }) => {
	await new Promise((resolve) => setTimeout(resolve, 250));
	return json([
		{
			id: "48a27d33-3571-4d0c-ad09-668b921abee6",
			created_at: "2024-11-19T22:35:38.865+00:00",
			updated_at: "2024-11-19T22:35:40.293+00:00",
			name: "Demo",
			markdown: null,
			slug: "demo",
			thumbnail_url: null,
			public: true,
			enabled: true,
			max_members: 1,
			git_info: {
				id: "402c6744-1c60-46d1-a524-fd2a82abb112",
				created_at: "2024-11-19T22:34:56.976+00:00",
				updated_at: "2024-11-19T22:35:00.619+00:00",
				git_url: "http://example.com",
				git_branch: "master",
				git_commit: "1",
			},
			creator: {
				id: "2e6c1f6b-51c4-42de-a385-4d016f35b9cd",
				created_at: "2024-11-19T20:19:08.332456+00:00",
				updated_at: "2024-11-19T20:19:08.332456+00:00",
				login: "w2wizard",
				display_name: "w2wizard",
				avatar_url: null,
				details_id: null,
			},
			tags: [],
		},
	]);
};
