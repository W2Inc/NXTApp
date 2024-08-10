import api, { verify } from "$lib/api/api";
import { error as err, type NumericRange } from "@sveltejs/kit";
import type { LayoutServerLoad } from "./$types";

export const load: LayoutServerLoad = async ({ fetch, params, locals }) => {
	// TODO: Avoid fetches if searched user is ourselves? (locals.user.id === params.user)


	console.log("layout.server.ts");
	const [dataReq, detailsReq] = await Promise.all([
		api.GET("/users/{id}", {
			params: { path: { id: params.user } },
			fetch,
		}),
		// api.GET("/users/{id}/details", {
		// 	params: { path: { id: params.user } },
		// 	fetch,
		// }),
	]);

	const data = verify(dataReq.response, dataReq.data, dataReq.error);
	// const details = verify(detailsReq.response, detailsReq.data, detailsReq.error);
	return {
		user: {
			/** Shortcut to data.id */
			id: data.id,
			data,
			details: {

			}
		}
	};
};
