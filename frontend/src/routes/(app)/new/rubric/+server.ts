import type { RequestHandler } from "./$types";

/**
 * On average this method results in around 20ms overhead which is fine!
 * Other methods such as invaldiating the page etc usually can add a whole 50-100ms!!
 */
export const GET: RequestHandler = async ({ locals, url }) => {
	const { data, response } = await locals.api.GET("/rubrics", {
		parseAs: "stream",
		params: {
			query: {
				"filter[name]": url.searchParams.get("name") ?? undefined,
			},
		},
	});

	// NOTE: https://github.com/sveltejs/kit/issues/12197
	response.headers.delete("content-encoding");
	return new Response(data, response)
};
