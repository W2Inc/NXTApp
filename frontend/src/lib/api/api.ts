// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import type { components, paths } from "./types";
import { dev } from "$app/environment";
import createClient, { type FetchResponse  } from "openapi-fetch";
import { PUBLIC_CDN_URL } from "$env/static/public";
import { error, type NumericRange } from "@sveltejs/kit";
import { ToastForm } from "$lib/utils";

// ============================================================================

export type AppError = components["schemas"]["AppError"];
export const cdnURL = PUBLIC_CDN_URL;
export const apiURL = dev ? "http://localhost:3000" : "https://api.oolp.dev";
export default createClient<paths>({ baseUrl: apiURL, mode: "cors" });

// ============================================================================

export function verify<T>(response: Response, data?: T, err?: AppError): T {
	if (!response.ok && response.status >= 500) {
		console.log("[API FAIL]", response, data, err);
		const status = response.status as NumericRange<400, 599>;
		error(status, err?.message ?? response.statusText);
	}
	return data!;
}
