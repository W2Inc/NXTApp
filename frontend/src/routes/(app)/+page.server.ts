import type { Actions } from "./$types";

export const actions: Actions = {
	search: async ({ request }) => {
		console.log("qwdqwd")
		const form = await request.formData();
		const query = form.get("query")?.toString()
		if (!query) return { };
		console.log("SEARCH", query)
		//todo: searhc api
		return {}
	}
};
