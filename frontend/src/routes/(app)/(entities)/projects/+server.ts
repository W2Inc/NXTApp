import { json, redirect } from "@sveltejs/kit";
import type { RequestHandler } from "./$types";

export const GET: RequestHandler = async ({ locals, url, fetch, request }) => {
	// request = new Request(
	// 	request.url.replace('https://api.yourapp.com/', url.),
	// 	request
	// );

	const query: Record<string, string> = {};
	if (url.searchParams.get("id")) {
		query["filter[id]"] = url.searchParams.get("id")!;
	}
	if (url.searchParams.get("slug")) {
		query["filter[slug]"] = url.searchParams.get("slug")!;
	}
	if (url.searchParams.get("name")) {
		query["filter[name]"] = url.searchParams.get("name")!;
	}

	const { data } = await locals.api.GET("/projects", {
		params: { query }
	});

	return json(data);
};
