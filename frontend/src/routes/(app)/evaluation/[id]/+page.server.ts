import { read } from "$app/server";
import type { PageServerLoad } from "./$types";
import Markdown from "./demo.txt";
import { codeToHtml } from 'shiki'

export const load: PageServerLoad = async ({ }) => {
	const file = read(Markdown);
	const code = codeToHtml('const demo = "balls";', {
		lang: 'c',
		theme: 'vitesse-dark',
	});

	return {
		rubric: file.text(),
		code,
	}
};
