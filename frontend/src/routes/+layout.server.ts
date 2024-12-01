import type { LayoutServerLoad } from "./$types";

export const load: LayoutServerLoad = async ({ locals, fetch }) => {
	const session = await locals.auth();
	const { data } = await locals.api.GET("/users/current");

	return {
		session,
		data,
	};
};
