import { logger } from "$lib/logger";
import { encodeID } from "$lib/utils";
import { hasRole } from "$lib/utils/roles.svelte";
import { error } from "@sveltejs/kit";
import type { LayoutServerLoad } from "./$types";

export const load: LayoutServerLoad = async ({ locals, cookies }) => {
	const session = await locals.session();
	logger.debug("Loading root layout...");
	return {
		session,
	};
};
