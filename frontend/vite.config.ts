import { sveltekit } from "@sveltejs/kit/vite";
import { enhancedImages } from "@sveltejs/enhanced-img";
import mkcert from 'vite-plugin-mkcert';
import { defineConfig } from "vite";

export default defineConfig({
	plugins: [sveltekit(), enhancedImages(), mkcert() ],
	ssr: {
		noExternal: ["three"],
	},
	server: {
    proxy: {
      '/api': {
        target: 'http://localhost:3000/',
        changeOrigin: false,
        rewrite: (path) => path.replace(/^\/api/, '')
      }
    }
  }
});
