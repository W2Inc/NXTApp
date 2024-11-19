// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import type { components, paths } from "./types";
import { dev } from "$app/environment";
import createClient, { type FetchResponse  } from "openapi-fetch";
import { error, type NumericRange } from "@sveltejs/kit";

// ============================================================================

const socketFetch: typeof fetch = async (input, init) => {
	return await fetch(input, {
		// @ts-ignore // NOTE(W2): Bun supports this.
		unix: "/var/run/docker.sock",
		...init,
	});
};

export const apiURL = dev ? "http://localhost:3000" : "https://api.oolp.dev";
export default createClient<paths>({
	baseUrl: apiURL,
	mode: "cors",
	fetch: socketFetch,
	headers: {
		"Host": "localhost",
		"User-Agent": Bun.env.SERVER ?? "robopeer",
	},
});

export interface APIError {
	type: string;
	title: string;
	status: number;
	detail: string;
	instance: string;
}

// ============================================================================
