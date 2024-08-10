// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

// import adapter from "svelte-adapter-bun";
import preprocess from "svelte-preprocess";
import adapter from "@sveltejs/adapter-node";

// If your environment is not supported or you settled on a specific environment, switch out the adapter.
// See https://kit.svelte.dev/docs/adapters for more information about adapters.

// Consult https://kit.svelte.dev/docs/integrations#preprocessors
// for more information about preprocessors

// ============================================================================

/** @type {import('@sveltejs/kit').Config} */
const config = {
	preprocess: [
		preprocess({
			postcss: {
				configFilePath: "./postcss.config.js",
			},
		}),
	],
	kit: {
		adapter: adapter({
			precompress: true,
		}),
		alias: {
			$static: "./static",
			$lib: "./src/lib",
			$components: "./src/lib/components",
			$stores: "./src/lib/stores",
			$utils: "./src/utils",
			$assets: "./src/assets",
		},
	},
};

export default config;
