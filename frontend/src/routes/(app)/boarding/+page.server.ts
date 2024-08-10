import { redirect } from "@sveltejs/kit";
import type { Actions } from "./$types";

export const actions: Actions = {
	default: async () => {
		redirect(301, "/settings/profile");
	},
};