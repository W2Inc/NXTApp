// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import jwt from "jsonwebtoken";
import type { Session } from "./app";
import { redirect, type Handle, error } from "@sveltejs/kit";
import { JWT_SECRET } from "$env/static/private";
import { sequence } from "@sveltejs/kit/hooks";
import { apiURL } from "$lib/api/api";

// ============================================================================

// Empty roles means that any role can access the route
// but they still need to be authenticated
type Route = { [key: string]: RouteMeta };
type RouteMeta = { roles: string[] };
const protectedBaseRoutes: Route = {
	new: { roles: [] },
	settings: { roles: [] },
	// boarding: { roles: [] },
	admin: { roles: [] },
};

/**
 * Checks if the user is an admin.
 * @param session The session / user to check.
 * @returns True if the user is an admin, otherwise false.
 */
function isAdministrator(session: Session | null) {
	if (!session) return false;
	if (!session.user.data.role) return false;
	return session.user.data.role.toLowerCase() === "admin";
}

// ============================================================================

// Check the cookies for a session
const checkSession: Handle = async ({ event, resolve }) => {
	event.locals.session = null;
	const JWT = event.cookies.get("SESSION_COOKIE");
	if (!JWT) return await resolve(event);

	try {
		const verified = jwt.verify(JWT, JWT_SECRET);
		if (!verified) return await resolve(event);
		const session = jwt.decode(JWT, { json: true });
		if (!session) return await resolve(event);

		event.locals.session = session as Session;
		return await resolve(event);
	} catch (exception) {
		error(403, `${exception}`);
	}
};

// Check if the user is allowed to access the route
const checkRoute: Handle = async ({ event, resolve }) => {
	const byebye = async () => {
		const response = await resolve(event, {
			filterSerializedResponseHeaders(name) {
				return name === "content-length";
			},
		});

		//console.log("RESPONSE:", response.headers);
		//if (response.status === 401) {
		//	console.log("Redirecting to login page...");
		//	redirect(301, `/auth/login?redirect=${encodeURIComponent(event.url.pathname)}`);
		//}

		return response;
	};

	// Admins can access any route.
	//console.log("REQUEST:", event.request.headers);
	if (isAdministrator(event.locals.session)) return await byebye();

	const urlPath = event.url.pathname;
	const urlRootUrl = urlPath.split("/")[1];
	if (!urlRootUrl) return await byebye();

	const route = protectedBaseRoutes[urlRootUrl];
	if (!route) {
		return await byebye();
	} else if (route && !event.locals.session) {
		// http://localhost:5173/auth/login?redirect=/settings/profile
		redirect(307, `/auth/login?redirect=${encodeURIComponent(event.url.pathname)}`);
	}

	const { user } = event.locals.session!;
	if (!user.data.role) error(403, "Forbidden"); // No role, no access

	if (route.roles.length > 0 && !route.roles.includes(user.data.role)) {
		error(403, "Forbidden");
	}
	return await byebye();
};

// ============================================================================

export const handle = sequence(checkSession, checkRoute);

// ============================================================================

// Forwards the session cookies to the API
export async function handleFetch({ event, request, fetch }) {
	const reqCookies = request.headers.get("cookie");
	if (reqCookies && request.url.startsWith(apiURL)) {
		request.headers.set("cookie", reqCookies);
	}
	return fetch(request);
}
