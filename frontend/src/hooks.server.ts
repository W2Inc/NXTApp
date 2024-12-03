// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { sequence } from "@sveltejs/kit/hooks";
import { error, redirect, type Handle } from "@sveltejs/kit";
import { handle as authenticationHandle } from "./lib/auth";
import { dev } from "$app/environment";
import type { paths as BackendRoutes } from "$lib/api/types";
import type { paths as KeycloakRoutes } from "$lib/api/keycloak";
import createClient from "openapi-fetch";

// ============================================================================

const authorizationHandle: Handle = async ({ event, resolve }) => {
	// TODO: Check for roles or anything to authorize the user to access stuff
	return resolve(event);
};

/** Create the universal api fetch function */
const apiHandle: Handle = async ({ event, resolve }) => {
	event.locals.api ??= createClient<BackendRoutes>({
		baseUrl: dev ? "http://localhost:3000" : "http://localhost:3000",
		mode: "cors",
		fetch: event.fetch,
	});

	event.locals.keycloak ??= createClient<KeycloakRoutes>({
		baseUrl: dev ? "http://localhost:8089/auth" : "http://localhost:8089/auth",
		mode: "cors",
		fetch: event.fetch,
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
