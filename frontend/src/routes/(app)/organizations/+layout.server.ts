// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { redirect } from "@sveltejs/kit";
import type { LayoutServerLoad } from "./$types";

// ============================================================================

export const load: LayoutServerLoad = async () => redirect(301, "/");
