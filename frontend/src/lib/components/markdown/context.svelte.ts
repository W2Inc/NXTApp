import { Carta } from "carta-md";
import { emoji } from "@cartamd/plugin-emoji";
import { code } from "@cartamd/plugin-code";
import DOMPurify from "isomorphic-dompurify";

/** Singleton carta context for markdown readers/writers. */
export const cartaContext = $state(
	new Carta({
		sanitizer: DOMPurify.sanitize,
		extensions: [
			// attachment({
			// 	async upload() {
			// 		return 'some-url-from-server.xyz';
			// 	}
			// }),
			emoji(),
			// slash(),
			code(),
		],
	}),
);
