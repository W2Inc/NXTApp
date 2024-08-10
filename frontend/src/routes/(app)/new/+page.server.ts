// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import api from "$lib/api/api";
import type { components } from "$lib/api/types";
import { ToastForm } from "$lib/utils";
import type { PageServerLoad } from "./$types";
import { error as err, type Actions, fail, type NumericRange } from "@sveltejs/kit";

// ============================================================================

export const load: PageServerLoad = async ({ fetch }) => {
	const { response, data, error } = await api.GET("/tags/", { fetch });
	if (!response.ok) {
		const status = response.status as NumericRange<400, 599>;
		err(status, error?.message || "Failed to fetch tags");
	}

	return {
		tags: data,
	};
};

export const actions: Actions = {
	project: async ({ request, fetch }) => {
		const form = await request.formData();
		const name = form.get("name")?.toString();
		const thumbnail = form.get("thumbnail") as File | null;
		const description = form.get("description")?.toString();
		const tagsData = form.get("tags")?.toString().split(",");

		if (!name || !description || !thumbnail) {
			return ToastForm.fail(400, "Name and description is required!");
		} else if (thumbnail.size > 1024 * 1024 * 5) {
			return ToastForm.fail(400, "Thumbnail is too large, max 5MB!");
		} else if (!thumbnail.type.startsWith("image/")) {
			return ToastForm.fail(400, "Thumbnail must be an image!");
		} else if (thumbnail.type !== "image/png" && thumbnail.type !== "image/jpeg") {
			return ToastForm.fail(400, "Thumbnail must be a png or jpeg!");
		}

		try {
			const { response, error } = await api.POST("/projects/", {
				fetch,
				body: {
					name,
					tags: tagsData,
					subject_markdown: description,
				},
			});
			if (!response || error) {
				return ToastForm.fail(
					response.status,
					error?.message ?? "Failed to create project"
				);
			}
		} catch (error) {
			throw err(500, "Failed to create project");
		}

		return ToastForm.success("Project has been created!");
	},

	// Create a new learning goal
	goal: async ({ request, fetch, cookies }) => {
		const form = await request.formData();
		const name = form.get("name")?.toString();
		const description = form.get("description")?.toString();

		if (!name || !description) {
			return ToastForm.fail(400, "Name and description is required!");
		}

		const { response, error } = await api.POST("/learning_goals/", {
			fetch,
			body: {
				name,
				description,
			},
		});
		if (!response || error) {
			return ToastForm.fail(
				response.status,
				error?.message ?? "Failed to create learning goal",
				{
					test: "test",
				}
			);
		}
		return ToastForm.success("Learning goal has been created!");
	},

	// Subscribe to something
	subscribe: async ({ request, fetch }) => {
		const formData = await request.formData();
		const name = formData.get("name")?.toString();
		const type = formData.get("type")?.toString();

		if (!name || !type) {
			return ToastForm.fail(400, "Name and type is required!");
		}

		// type can either be "goal" or "project"
		if (type !== "goal" && type !== "project") {
			return ToastForm.fail(400, `Invalid type: ${type}`);
		}

		return ToastForm.success("[WIP] Subscribed!");
	},
};
