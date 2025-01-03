import type { RequestHandler } from "./$types";

export const GET: RequestHandler = async ({}) => {

	await new Promise(resolve => setTimeout(resolve, 5000));
	return new Response();
};
