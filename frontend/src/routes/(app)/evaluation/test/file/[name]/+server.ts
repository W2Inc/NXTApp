import type { RequestHandler } from "@sveltejs/kit";

const files = [
	{
		name: "README.md",
		path: "README.md",
		sha: "e9168dbb7daed641f96a493ae7aba8d431b148e2",
		last_commit_sha: "8403dfea36ca3bb4ed6bd3cf6027e2862f895ca6",
		type: "file",
		size: 13,
		encoding: null,
		content: null,
		target: null,
		url: "https://git.hamette.net/api/v1/repos/W2Wizard/demo/contents/README.md?ref=main",
		html_url: "https://git.hamette.net/W2Wizard/demo/src/branch/main/README.md",
		git_url:
			"https://git.hamette.net/api/v1/repos/W2Wizard/demo/git/blobs/e9168dbb7daed641f96a493ae7aba8d431b148e2",
		download_url: "https://git.hamette.net/W2Wizard/demo/raw/branch/main/README.md",
		submodule_git_url: null,
		_links: {
			self: "https://git.hamette.net/api/v1/repos/W2Wizard/demo/contents/README.md?ref=main",
			git: "https://git.hamette.net/api/v1/repos/W2Wizard/demo/git/blobs/e9168dbb7daed641f96a493ae7aba8d431b148e2",
			html: "https://git.hamette.net/W2Wizard/demo/src/branch/main/README.md",
		},
	},
];

export const GET: RequestHandler = async ({ params }) => {
	const file = files.find(({ name }) => name === params.name);
	if (file) return new Response(file.html_url);
	return new Response(null, {
		status: 404,
	});
};
