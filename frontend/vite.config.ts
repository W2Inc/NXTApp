// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { defineConfig, type Plugin } from "vite";
import { sveltekit } from "@sveltejs/kit/vite";
import { enhancedImages } from "@sveltejs/enhanced-img";

// https://svelte.dev/docs/typescript#enhancing-built-in-dom-types
export default defineConfig({
	plugins: [enhancedImages(), sveltekit()],
	ssr: {
		noExternal: ["three"],
	},
});
