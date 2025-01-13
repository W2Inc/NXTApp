import { sveltekit } from "@sveltejs/kit/vite";
import { enhancedImages } from "@sveltejs/enhanced-img";
import mkcert from "vite-plugin-mkcert";
import Sonda from "sonda/sveltekit";
import { defineConfig } from "vite";

export default defineConfig({
	plugins: [sveltekit(), enhancedImages() /*mkcert()*/, , Sonda({ server: true })],
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
