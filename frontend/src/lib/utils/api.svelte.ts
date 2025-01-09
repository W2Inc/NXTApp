import { page } from "$app/state";
import type { paths } from "$lib/api/types";
import createClient from "openapi-fetch";

export default function useAPI() {
	const client = createClient<paths>({
		baseUrl: "/api",
		mode: "cors",
		headers: {
			Authorization: `Bearer ${page.data.session?.access_token}`,
		},
	});

	return {
		client
	}
}
