import { fail } from "@sveltejs/kit";
import type { Actions, PageServerLoad } from "./$types";

export const load: PageServerLoad = async ({ locals }) => {
	const session = await locals.auth();
	const { data, response, error } = await locals.keycloak.GET("/admin/realms/{realm}/users/{user-id}/sessions", {
		params: {
			path: {
				"user-id": session!.user!.id!,
				realm: "student"
			}
		}
	});

	console.log(data?.map((s) => s.id));

	return {
		sessions: data ?? []
	}
};

export const actions: Actions = {
	delete: async ({ locals, request}) => {
		const session = await locals.auth();
		const form = await request.formData();
		const id = form.get("id")?.toString();
		if (!id)
			return fail(400);

		const { data, response, error } = await locals.keycloak.DELETE("/admin/realms/{realm}/sessions/{session}", {
			params: {
				path: {
					session: id,
					realm: "student"
				}
			}
		});

		if (!response.ok)
			return fail(response.status);
		return {};
	},
	deleteAll: async({ locals, request}) => {
		const session = await locals.auth();
		const { data, response, error } = await locals.keycloak.POST("/admin/realms/{realm}/users/{user-id}/logout", {
			params: {
				path: {
					"user-id": session?.user?.id!,
					realm: "student"
				}
			}
		});

		console.log(response.statusText)

		return {

		}
	}
};
