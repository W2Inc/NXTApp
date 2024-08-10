// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import type { ParamMatcher } from "@sveltejs/kit";

// ============================================================================

export const match: ParamMatcher = (param) => {
	return /^[a-f0-9]{8}-([a-f0-9]{4}-){3}[a-f0-9]{12}$/i.test(param);
};
