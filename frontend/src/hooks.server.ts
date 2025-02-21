// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { sequence } from "@sveltejs/kit/hooks";
import { error, redirect, type Handle, type RequestEvent, type ServerInit } from "@sveltejs/kit";
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
import type { Role } from "$lib/utils/roles.svelte";
import { initLogger, logger } from "$lib/logger";
// import config from "$lib/routes.json" with { type: "json" };

// ============================================================================

// TODO: Replace with arctic client...
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
};

export const init: ServerInit = async () => {
	initLogger();
	logger.info("Starting FE...");
}

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
	if (!requiredRoles.some(role => session.roles.includes(role))) {
		// Check if user has required roles
		error(403, "Insufficient permissions");
	}

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

const ratelimit: Handle = async ({ event, resolve }) => {
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
};

// First handle authentication, then authorization
// Each function acts as a middleware, receiving the request handle
// And returning a handle which gets passed to the next function
export const handle: Handle = sequence(
	ratelimit,
	authenticationHandle,
	authorizationHandle,
	apiHandle,
);

// ============================================================================

// NOTE(W2): Attach authorization here as putting it in the client means
// it would be stuck on the first client request's cookie.
export async function handleFetch({ fetch, request, event }) {
	if (request.url.startsWith("http://localhost:3001/")) {
		const session = await event.locals.session();
		const accessToken = event.cookies.get(`${KC_COOKIE_NAME}-a`);
		// request.headers.set("cookie", event.request.headers.get("cookie") ?? "");
		request.headers.set("authorization", `Bearer ${accessToken}`);
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
