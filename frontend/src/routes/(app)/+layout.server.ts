import { encodeUUID64 } from "$lib/utils";
import { hasRole } from "$lib/utils/roles.svelte";
import type { LayoutServerLoad } from "./$types";

export const load: LayoutServerLoad = async ({ locals, fetch }) => {
	const session = await locals.session();

	return {
		session,
	};
};
