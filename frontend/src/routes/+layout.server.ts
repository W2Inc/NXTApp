// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import type { LayoutServerLoad } from "./$types";

// ============================================================================

// Server-side load function to load in the user data into page data.
export const load: LayoutServerLoad = async ({ locals }) => {
	return {
		me: locals.session ? locals.session.user : null,
	};
};
