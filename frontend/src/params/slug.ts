// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import type { ParamMatcher } from "@sveltejs/kit";

// ============================================================================

// Example guid: cursus-c or blabla or blabla23 or bla_bla
export const match: ParamMatcher = (param) => {
	return /^[a-z0-9_-]+$/i.test(param);
};
