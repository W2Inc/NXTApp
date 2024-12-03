import type { LayoutServerLoad } from "./$types";

export const load: LayoutServerLoad = async ({ locals, fetch }) => {
	const session = await locals.auth();
	// const { data } = await locals.api.GET("/users/current", {
	// });

	const kc = await locals.keycloak.GET("/admin/realms");

	console.log(kc.response.status, kc.data);
	return {
		session,
		data: null,
	};
};