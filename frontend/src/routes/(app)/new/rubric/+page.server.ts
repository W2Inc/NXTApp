import api from "$lib/api/api";
import { ToastForm } from "$lib/utils";
import type { Actions, PageServerLoad } from "./$types";

// ============================================================================

export const load: PageServerLoad = async ({ fetch }) => {
	const projects = await api.GET("/projects/", { fetch });

	if (!projects.response.ok) {
		const status = projects.response.status;
		throw new Error(`Failed to load projects: ${status}`);
	}

	return {
		projects: projects.data!,
	};
}

export const actions: Actions = {
	default: async ({ fetch, request}) => {
		const form = await request.formData();
		const name = form.get("name");
		const rubricType = form.get("type");
		const rubricMarkdown = form.get("description");
		const projectId = form.get("projectID");

		if (!name || !rubricType || !rubricMarkdown || !projectId) {
			return ToastForm.fail(400, "Please fill in all required fields.");
		};

		const { response, data, error: err } = await api.POST("/rubric/", {
			body: {
				name: name.toString(),
				project_id: Number(projectId),
				rubric_git_info_id: 0,
				rubric_markdown: rubricMarkdown.toString(),
				rubric_type: rubricType.toString(),
				status: "n/a"
			},
			fetch
		});

		if (!response.ok || err)
			return ToastForm.fail(response.status, err?.message || "Failed to create rubric.");
		return ToastForm.success("Rubric created successfully.", data);
	},
};
