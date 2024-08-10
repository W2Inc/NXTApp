// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { useStorage } from "./storage";
import type { RecentStore } from "$lib/types";

// ============================================================================

export const recentNavs = useStorage<RecentStore>("recent", {
	links: [],
});
