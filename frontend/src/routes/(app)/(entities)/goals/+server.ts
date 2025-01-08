import { json } from "@sveltejs/kit";
import type { RequestHandler } from "./$types";

export const GET: RequestHandler = async ({ locals, url, fetch }) => {
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

	const { data } = await locals.api.GET("/goals", {
		params: { query }
	});

	return json(data);
};
