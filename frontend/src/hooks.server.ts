// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { sequence } from "@sveltejs/kit/hooks";
import {
	error,
	type Handle,
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
	BE_HOST
} from "$env/static/private";
import KeycloakClient from "$lib/keycloak";
import { useRetryAfter } from "$lib/utils/limiter.svelte";
import type { Role } from "$lib/utils/roles.svelte";
import { initLogger, logger } from "$lib/logger";
// import config from "$lib/routes.json" with { type: "json" };

// ============================================================================

const DebugLogMD: Middleware = {
  // async onRequest({ request, options }) {
	// 	logger.debug(`API: ${request.method.toUpperCase()} => ${request.url}`);
  //   return request;
  // },
	async onResponse({ request, response, options }) {
		logger.debug(`API: ${request.method.toUpperCase()} ${request.url} => [${response.status}:${response.statusText}]`);
		// if (!response.ok)
		// 	throw error(response.status, response.statusText);
    return response;
  },
};

// ============================================================================

// TODO: Replace with arctic client...
const keycloak = new KeycloakClient(KC_CLIENT_ID, KC_CLIENT_SECRET, KC_ISSUER);

/**
 * Global rate limiting, you can use this rate limiter across pages or use
 * a new rate limiter to determine rate limits per page if so desired.
 *
 * @deprecated Use Traeffik.
 */
const limiter = useRetryAfter({
	IP: [10, "h"],
	IPUA: [60, "m"],
});

/**
 * Here you can configure the overal routes that need which role in order
 * to be accessed.
 */
// const routes: Record<string, Role[]> = config;
// or
const routes: Record<string, Role[]> = {
	"/": [],
	"/settings": [],
	"/auth": [],
	"/users": [],
	"/new": [],
	"/notifications": [],
	"/changelog": [],
	"/search": [],
	"/projects": [],
};

export const init: ServerInit = async () => {
	initLogger();
	logger.info("Starting FE...");
};

// ============================================================================

const authorizationHandle: Handle = async ({ event, resolve }) => {
	const session = await event.locals.session();
	const url = `/${event.url.pathname.split("/")[1]}`;
	const requiredRoles = routes[url];

	if (!requiredRoles) {
		// If route is not defined in routes, default to restricted access
		error(403, "Access denied: Route not configured");
	}
	if (requiredRoles.length === 0) {
		// If route requires no roles, allow access
		return resolve(event);
	}
	if (!session) {
		// Check if user is authenticated when roles are required
		error(401, "Authentication required");
	}
	if (!requiredRoles.some((role) => session.roles.includes(role))) {
		// Check if user has required roles
		error(403, "Insufficient permissions");
	}

	return resolve(event);
};

/** Create the universal api fetch function */
const apiHandle: Handle = async ({ event, resolve }) => {
	event.locals.api = createClient<BackendRoutes>({
		baseUrl: dev ? "http://localhost:3001" : "https://localhost:3000",
		mode: "cors",
		fetch: event.fetch,
	});

	event.locals.api.use(DebugLogMD);

	event.locals.keycloak = createClient<KeycloakRoutes>({
		baseUrl: KC_HOST,
		mode: "cors",
		fetch: event.fetch,
	});

	return resolve(event);
};

const initial: Handle = async ({ event, resolve }) => {
	// logger.debug("Current cookies", { cookies: event.cookies.getAll().map(cookie => cookie.name) })
	event.setHeaders({
		"x-powered-by": `Bun ${Bun.version}`,
		"x-application": "APP_NAME",
	});

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
	if (request.url.startsWith(BE_HOST)) {
		const accessToken = event.cookies.get(`${KC_COOKIE_NAME}-a`);
		request.headers.set("authorization", `Bearer ${accessToken}`);
	}

	if (request.url.startsWith(KC_HOST)) {
		const token = await keycloak.getToken();
		request.headers.set("Authorization", `Bearer ${token}`);
	}

	return fetch(request);
}
