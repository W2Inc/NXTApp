import { error } from "@sveltejs/kit";
import type { PageServerLoad } from "./$types";

export const load: PageServerLoad = async ({ params }) => {
	const file = `/static/changelog/v${params.version}.md`;
	const changelogs = import.meta.glob("/static/changelog/*.md", {
		query: "?raw",
		import: "default",
	});
	const changelog = changelogs[file];
	if (!changelog) error(404);

	return {
		md: changelog(),
	};
};
