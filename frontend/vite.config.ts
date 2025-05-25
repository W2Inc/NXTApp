import { sveltekit } from "@sveltejs/kit/vite";
import { enhancedImages } from "@sveltejs/enhanced-img";
import mkcert from "vite-plugin-mkcert";
import Sonda from "sonda/sveltekit";
import tailwindcss from '@tailwindcss/vite';
import { defineConfig } from "vite";

export default defineConfig({
	plugins: [
		tailwindcss(),
		enhancedImages(),
		sveltekit(),
		/*mkcert(),*/
		Sonda({ server: true, open: false }),
	],
	ssr: {
		noExternal: ["three"],
	},
	build: {
		sourcemap: true,
	},
	server: {
		proxy: {
			"/api": {
				target: "http://localhost:3001/",
				changeOrigin: false,
				rewrite: (path) => path.replace(/^\/api/, ""),
			},
		},
	},
});
