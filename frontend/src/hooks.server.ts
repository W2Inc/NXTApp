// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { sequence } from "@sveltejs/kit/hooks";
import { redirect, type Handle } from "@sveltejs/kit";
import { handle as authenticationHandle } from "./lib/auth";
import { dev } from "$app/environment";
import { KC_COOKIE_NAME } from "$env/static/private";
import type { paths } from "$lib/api/types";
import createClient from "openapi-fetch";

// ============================================================================

const authorizationHandle: Handle = async ({ event, resolve }) => {
	// TODO: Check for roles or anything to authorize the user to access stuff
	return resolve(event);
};

/** Create the universal api fetch function */
const apiHandle: Handle = async ({ event, resolve }) => {
	event.locals.api ??= createClient<paths>({
		baseUrl: dev ? "http://localhost:3000" : "https://api.oolp.dev",
		mode: "cors",
		fetch: event.fetch,
		signal: AbortSignal.timeout(5000),
	});

	return resolve(event);
};

// First handle authentication, then authorization
// Each function acts as a middleware, receiving the request handle
// And returning a handle which gets passed to the next function
export const handle: Handle = sequence(
	authenticationHandle,
	apiHandle,
	authorizationHandle,
);

// ============================================================================

// NOTE(W2): Attach authorization here as putting it in the client means
// it would be stuck on the first client request's cookie.
export async function handleFetch({ fetch, request, event }) {
	const url = new URL(request.url);
	if (url.hostname.startsWith("api") || (dev && url.port === "3000")) {
		const session = await event.locals.auth();
		request.headers.set("Authorization", `Bearer ${session?.access_token}`);
	}
	return fetch(request);
}
