// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import api from "$lib/api/api";
import { ToastForm, doubleCheck } from "$lib/utils";
import type { Actions, PageServerLoad } from "./$types";
import { error, type NumericRange } from "@sveltejs/kit";

// ============================================================================

export const load: PageServerLoad = async ({ fetch, params}) => {
	const id = Number(params.id);
	if (isNaN(id)) error(404, "Not found");

	const { response, data, error: err }  = await api.GET("/projects/{id}", {
		fetch, params: { path: { id } }
	});
	if (!response.ok || err) {
		const status = response.status as NumericRange<400, 599>;
		error(status, err?.message ?? response.statusText);
	}

	//const [tags, maintainers] = await Promise.all([
	//	api.GET("/tags/", {
	//		fetch,
	//		params: {
	//			query: {
	//				"filter[id]": project.tag_ids?.join(" ") ?? "",
	//			}
	//		}
	//	}),
	//	api.GET("/users/", {
	//		fetch,
	//	}),
	//]);

	return {
		project: data
	}
};

// ============================================================================

export const actions: Actions = {
	default: async ({ fetch, request, params }) => {
		const form = await request.formData();
		console.log(params);

		return ToastForm.success("Project updated", {
			success: true,
		});
	}
};
