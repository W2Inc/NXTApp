// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import api from "$lib/api/api";
import { redirect } from "@sveltejs/kit";
import type { PageServerLoad } from "./$types";

// ============================================================================

export const load: PageServerLoad = async ({ fetch }) => {
	redirect(301, "/");
	//const [projects, goals] = await Promise.all([
	//	api.GET("/projects/", { fetch }),
	//	api.GET("/learning_goals/", { fetch }),
	//]);

	//if (!projects.response.ok || !goals.response.ok) {
	//	const status = projects.response.status;
	//	throw new Error(`Failed to load projects and goals: ${status}`);
	//}

	//return {
	//	projects: projects.data!,
	//	goals: goals.data!,
	//};
};
