import { logger } from "$lib/logger";
import { ensure } from "$lib/utils";
import { error, redirect, type Cookies } from "@sveltejs/kit";
import type { FetchResponse } from "openapi-fetch";

export async function check<
	T extends Record<string | number, any>,
	Options extends Record<string, any> = any,
	Media extends `${string}/${string}` = `${string}/${string}`,
>(
	fetchPromise: Promise<FetchResponse<T, Options, Media>>,
	ignoreStatusCodes?: number[],
	cookies?: Cookies
): Promise<{
	data: NonNullable<FetchResponse<T, Options, Media>["data"]> | null;
	error: NonNullable<FetchResponse<T, Options, Media>["error"]> | null;
	response: Response;
}> {
	const { data, error: e, response } = await fetchPromise;
	if (e || (!response.ok && !ignoreStatusCodes?.includes(response.status))) {
		switch (response.status) {
			case 401:
				redirect(303, "/");
			default:
				error(response.status);
		}
	}

	return {
		data: data!,
		error: null,
		response: response,
	};
}
