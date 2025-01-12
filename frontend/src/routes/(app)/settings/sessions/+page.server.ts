import { fail, redirect } from "@sveltejs/kit";
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

	return {
		sessions: data ?? []
	}
};

export const actions: Actions = {
	delete: async ({ locals, request, url}) => {
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
		return redirect(301, url);
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

		return redirect(301, url);
	}
};
