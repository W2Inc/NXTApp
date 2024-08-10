// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { ToastForm } from "$lib/utils";
import { redirect } from "@sveltejs/kit";
import type { Actions, PageServerLoad } from "./$types";

// ============================================================================

export const load: PageServerLoad = async ({}) => {
	// 1 Check if session id is present
	// 2. Get that session from the api and check if the user is
	//    allowed to do it. If not, kick them out.
	// ...
};

// ============================================================================

export const actions: Actions = {
	finish: async ({}) => {
		// 1. Check if the user is allowed to finish the rubric
		// 2. If yes, then finish the rubric
		// 3. If no, then show an error message
		// ...
		redirect(301, "/"); // Redirect to that project page to show the result
		return ToastForm.success("Rubric finished successfully");
	}
};
