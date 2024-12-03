import type { PageServerLoad } from "./$types";

export const load: PageServerLoad = async ({ locals }) => {
	const session = await locals.auth();
	const { data, response, error } = await locals.keycloak.GET("/admin/realms/{realm}/users/{user-id}/sessions", {
		params: {
			path: {
				realm: "student",
				"user-id": session?.user?.id
			}
		}
	});

	console.log("123", error)
	return {
		sessions: data
	}
};
