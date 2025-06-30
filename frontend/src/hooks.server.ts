// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { sequence } from "@sveltejs/kit/hooks";
import {
	error,
	redirect,
	type Handle,
	type HandleServerError,
	type RequestEvent,
	type ServerInit,
} from "@sveltejs/kit";
import { handle as authenticationHandle } from "./lib/oauth";
import { dev } from "$app/environment";
import type { paths as BackendRoutes } from "$lib/api/types";
import type { paths as KeycloakRoutes } from "$lib/api/keycloak";
import createClient, { type Middleware } from "openapi-fetch";
import {
	KC_CLIENT_ID,
	KC_CLIENT_SECRET,
	KC_COOKIE_NAME,
	KC_ISSUER,
	KC_HOST,
	S3_ACCESS_KEY_ID,
	S3_SECRET_ACCESS_KEY,
	API_URL
} from "$env/static/private";

import KeycloakClient from "$lib/keycloak";
import type { Role } from "$lib/utils/roles.svelte";
import { initLogger, logger } from "$lib/logger";
import { PUBLIC_S3_BUCKET, PUBLIC_S3_ENDPOINT } from "$env/static/public";
import RouteConfig from "./config.json" with { type: "json" };
import type { Problem } from "$lib/utils/api.svelte";

// ============================================================================

// TODO: Replace with arctic client...
// const keycloak = new KeycloakClient(KC_CLIENT_ID, KC_CLIENT_SECRET, KC_ISSUER);

/**
 * Here you can configure the overal routes that need which role in order
 * to be accessed.
 */
const routes = RouteConfig as Record<string, string[] | never[]>;

export const init: ServerInit = async () => {
	initLogger();
	logger.info("Starting FE...");
};

// ============================================================================

const authorizationHandle: Handle = async ({ event, resolve }) => {
	const session = await event.locals.session();
	const url = `/${event.url.pathname.split("/")[1]}`;
	const requiredScopes = routes[url];

	// Redirect to home if no route config or unauthorized
	if (!requiredScopes) return redirect(303, "/");

	// Allow if no roles required
	if (requiredScopes.length === 0) return resolve(event);

	// Require authentication for protected routes
	if (
		!session ||
		(requiredScopes.length > 0 &&
			!requiredScopes.some((s) => session.scopes.includes(s)))
	) {
		return new Response(
			"Unauthorized",
			{
				status: 401,
				statusText: "Unauthorized",
				headers: {
					"WWW-Authenticate": `Bearer realm="${KC_ISSUER}"`,
				},
			},
		);
	}

	return resolve(event);
};

/** Create the universal api fetch function */
const apiHandle: Handle = async ({ event, resolve }) => {
	event.locals.api ??= createClient<BackendRoutes>({
		baseUrl: API_URL,
		mode: "cors",
		fetch: event.fetch,
	});

	return resolve(event);
};

const initial: Handle = async ({ event, resolve }) => {
	event.setHeaders({
		"x-powered-by": `Bun ${Bun.version}`,
		"x-application": "APP_NAME",
	});

	// Configure various S3 buckets
	event.locals.buckets ??= {
		thumbnail: new Bun.S3Client({
			endpoint: PUBLIC_S3_ENDPOINT,
			bucket: PUBLIC_S3_BUCKET,
			accessKeyId: S3_ACCESS_KEY_ID,
			secretAccessKey: S3_SECRET_ACCESS_KEY,
		}),
	};

	// Easter egg
	if (event.url.pathname.startsWith("/powerwolf"))
		redirect(308, "https://youtu.be/u0yZOriAUlA?t=15");
	return resolve(event);
};

// First handle authentication, then authorization
// Each function acts as a middleware, receiving the request handle
// And returning a handle which gets passed to the next function
export const handle: Handle = sequence(
	initial,
	authenticationHandle,
	authorizationHandle,
	apiHandle,
);

// ============================================================================

// NOTE(W2): Attach authorization here as putting it in the client means
// it would be stuck on the first client request's cookie.
export async function handleFetch({ fetch, request, event }) {
	if (request.url.startsWith(API_URL)) {
		const accessToken = event.cookies.get(`${KC_COOKIE_NAME}-a`);
		request.headers.set("authorization", `Bearer ${accessToken}`);
	}

	// if (request.url.startsWith(KC_HOST)) {
	// 	const token = await keycloak.getToken();
	// 	request.headers.set("Authorization", `Bearer ${token}`);
	// }

	return fetch(request);;
}

// ============================================================================
