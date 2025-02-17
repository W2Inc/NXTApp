// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { decodeID } from "$lib/utils";
import type { ParamMatcher } from "@sveltejs/kit";

// ============================================================================

export const match: ParamMatcher = (param) => {
	const uuid = decodeID(param);
	return /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i.test(uuid);
};
