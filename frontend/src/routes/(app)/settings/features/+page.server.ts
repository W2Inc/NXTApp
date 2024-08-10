// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { fail, type Action, type Actions } from "@sveltejs/kit";
import type { PageServerLoad } from "./$types";
import api from "$lib/api/api";
import { ToastForm } from "$lib/utils";
import type { components } from "$lib/api/types";

// ============================================================================

export const load: PageServerLoad = async ({ fetch }) => {
	const features: components["schemas"]["Features"][] = [
		{
			created_at: "2021-09-28T14:00:00.000Z",
			description: "This is a demo feature.",
			id: 1,
			name: "Demo feature",
			public: true,
			updated_at: "2021-09-28T14:00:00.000Z",
		},
	];

	return {
		features,
	};
};

const toggleFeature: Action = async ({ request, fetch }) => {
	const data = await request.formData();
	const enabled = data.get("enabled")?.toString();
	const id = data.get("id")?.toString();

	if (!id || !enabled) {
		return ToastForm.fail(400, "No ID or enabled provided.");
	}
	// const response = await api.POST("/users_features/", {
	// 	body: {
	// 		// feature_id: id.,
	// 		// active: enabled,
	// 	}
	// });

	console.log("FEATURE", id, enabled);

	// if (!id) throw error(400, "No ID provided.");
	// await fetch(`api://users/${id}/notifications`, {
	// 	method: "DELETE",
	// });

	return ToastForm.success(`Feature ${enabled == "true" ? "enabled" : "disabled"}.`);
};

export const actions: Actions = {
	toggleFeature,
};
