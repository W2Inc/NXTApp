import { decodeID } from "$lib/utils";
import type { LayoutServerLoad } from "./$types";

export const load: LayoutServerLoad = async ({ params, locals }) => {
	const session = await locals.session();
	return {
		/** Useful for if something should be visible to only the current user */
		isCurrentUser: decodeID(params.id) === session?.user_id
	};
};
