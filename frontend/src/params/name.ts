// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import type { ParamMatcher } from "@sveltejs/kit";

// ============================================================================

// Shortcut search by name
export const match: ParamMatcher = (param) => {
	return param.startsWith("@");
};
