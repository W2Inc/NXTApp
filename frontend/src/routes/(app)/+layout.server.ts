import { encodeUUID64 } from "$lib/utils";
import type { LayoutServerLoad } from "./$types";

export const load: LayoutServerLoad = async ({ locals, fetch }) => {
	const session = await locals.session();
	return {
		session: session,
	};
};
