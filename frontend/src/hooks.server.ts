// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { sequence } from "@sveltejs/kit/hooks";
import { error, redirect, type Handle, type RequestEvent } from "@sveltejs/kit";
import { handle as authenticationHandle } from "./lib/oauth";
import { dev } from "$app/environment";
import type { paths as BackendRoutes } from "$lib/api/types";
import type { paths as KeycloakRoutes } from "$lib/api/keycloak";
import createClient from "openapi-fetch";
import {
	KC_CLIENT_ID,
	KC_CLIENT_SECRET,
	KC_COOKIE_NAME,
	KC_ISSUER,
} from "$env/static/private";
import KeycloakClient from "$lib/keycloak";
import { useRetryAfter } from "$lib/utils/limiter.svelte";

// ============================================================================


const keycloak = new KeycloakClient(KC_CLIENT_ID, KC_CLIENT_SECRET, KC_ISSUER);

/**
 * Global rate limiting, you can use this rate limiter across pages or use
 * a new rate limiter to determine rate limits per page if so desired.
 */
const limiter = useRetryAfter({
	IP: [10, "h"],
	IPUA: [60, "m"],
});

/**
 * Here you can configure the overal routes that need which permission in order
 * to be accessed.
 *
 * AT THIS TIME, any route can be visited by anyone. But later on the idea
 * is to configure as such that it is permission based.
 *
 * e.g: "/": [] or "/admin": [view:admin] or "/settings": [view:settings]
 */
const routes: Record<string, boolean> = {
	"/": false,
	"/signout": false,
	"/signin": false,
	"/settings": true,
	"/new": true
};

/** Default per page fetch size */
export const PER_PAGE = 10;

// ============================================================================

const authorizationHandle: Handle = async ({ event, resolve }) => {
	const session = await event.locals.session();
	// if (session === null) {
	// 	const url = `/${event.url.pathname.split("/")[1]}`;
	// 	if (routes[url]) {
	// 		redirect(301, "/");
	// 	}
	// }
	return resolve(event);
};

/** Create the universal api fetch function */
const apiHandle: Handle = async ({ event, resolve }) => {
	const session = await event.locals.session();
	event.locals.api = createClient<BackendRoutes>({
		baseUrl: dev ? "http://localhost:3001" : "https://localhost:3000",
		mode: "cors",
		headers: {
			Authorization: `Bearer ${session?.access_token}`,
		},
		fetch: event.fetch,
	});

	event.locals.keycloak = createClient<KeycloakRoutes>({
		baseUrl: dev ? "http://localhost:8089/auth" : "http://localhost:8089/auth",
		mode: "cors",
		fetch: event.fetch,
	});

	return resolve(event);
};

const otherHandle: Handle = async ({ event, resolve }) => {
	event.locals.limiter = limiter;
	event.setHeaders({
		"x-powered-by": `Bun ${Bun.version}`,
		"x-application": "APP_NAME",
	});

	const limit = await limiter.check(event);
	if (limit.isLimited) {
		error(429, `Too many requests, try again in ${limit.retryAfter} seconds.`);
	}

	return resolve(event);
}

// First handle authentication, then authorization
// Each function acts as a middleware, receiving the request handle
// And returning a handle which gets passed to the next function
export const handle: Handle = sequence(
	authenticationHandle,
	authorizationHandle,
	apiHandle,
	otherHandle
);

// ============================================================================

// NOTE(W2): Attach authorization here as putting it in the client means
// it would be stuck on the first client request's cookie.
export async function handleFetch({ fetch, request, event }) {
	if (request.url.startsWith("http://localhost:3001/")) {
		const session = await event.locals.session();
		// request.headers.set("cookie", event.request.headers.get("cookie") ?? "");
		request.headers.set("authorization", `Bearer ${session?.access_token}`);
	}

	if (request.url.startsWith("http://localhost:8089/")) {
		const token = await keycloak.getToken();
		request.headers.set("Authorization", `Bearer ${token}`);
		// console.log("KEYCLOAK CALL")
		// const session = await event.locals.auth();
		// request.headers.set('cookie', event.request.headers.get('cookie') ?? "");
		// request.headers.set('authorization', `Bearer ${session?.access_token}`);
	}

	return fetch(request);
}
