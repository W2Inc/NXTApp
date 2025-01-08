// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { sequence } from "@sveltejs/kit/hooks";
import { error, redirect, type Handle, type RequestEvent } from "@sveltejs/kit";
import { handle as authenticationHandle } from "./lib/auth";
import { dev } from "$app/environment";
import type { paths as BackendRoutes } from "$lib/api/types";
import type { paths as KeycloakRoutes } from "$lib/api/keycloak";
import createClient from "openapi-fetch";
import { KC_CLIENT_ID, KC_CLIENT_SECRET, KC_COOKIE_NAME, KC_ISSUER } from "$env/static/private";
import KeycloakClient from "$lib/keycloak";

// ============================================================================

// TODO: Ok idk why but I assume this is due to self signed certificates
// and I don't want to fuck around with this shit until the end.
process.env['NODE_TLS_REJECT_UNAUTHORIZED'] = String(!dev);

const keycloak = new KeycloakClient(KC_CLIENT_ID, KC_CLIENT_SECRET, KC_ISSUER);

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
};

// ============================================================================

const authorizationHandle: Handle = async ({ event, resolve }) => {
	const session = await event.locals.auth();
	if (session === undefined || session === null) {
		const url = `/${event.url.pathname.split("/")[1]}`;
		if (routes[url]) {
			redirect(301, "/");
		}
	}
	// TODO: Check for roles or anything to authorize the user to access stuff
	return resolve(event);
};

/** Create the universal api fetch function */
const apiHandle: Handle = async ({ event, resolve }) => {
	const session = await event.locals.auth();
	event.locals.api = createClient<BackendRoutes>({
		baseUrl: dev ? "https://localhost:3000" : "https://localhost:3000",
		mode: "cors",
		headers: {
			Authorization: `Bearer ${session?.access_token}`
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

// First handle authentication, then authorization
// Each function acts as a middleware, receiving the request handle
// And returning a handle which gets passed to the next function
export const handle: Handle = sequence(
	authenticationHandle,
	authorizationHandle,
	apiHandle,
);

// ============================================================================

// NOTE(W2): Attach authorization here as putting it in the client means
// it would be stuck on the first client request's cookie.
export async function handleFetch({ fetch, request, event }) {
	if (request.url.startsWith('https://localhost:3000/')) {
		const session = await event.locals.auth();
		request.headers.set('cookie', event.request.headers.get('cookie') ?? "");
		request.headers.set('authorization', `Bearer ${session?.access_token}`);
	}

	return fetch(request);
}
