// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { useStorage } from "./storage";
import type { ThemeStore } from "$lib/types";

// ============================================================================

export const theme = useStorage<ThemeStore>("theme", {
	scheme: "light",
	primary: "#ff4400",
});
