// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import jwt from "jsonwebtoken";
import { dev } from "$app/environment";
import api, { apiURL } from "$lib/api/api";
import type { PageServerLoad } from "./$types";
import { JWT_SECRET } from "$env/static/private";
import cookieParser from "set-cookie-parser";
import { redirect, error as err, type Actions, type NumericRange } from "@sveltejs/kit";

// ============================================================================

export const load: PageServerLoad = async ({ locals, url, cookies, fetch }) => {
	if (!url.searchParams.has("token")) return;

	const token = url.searchParams.get("token");
	if (!token) err(400, "No token in the URL was provided");
	if (locals.session) redirect(303, "/");

	let decoded: string | jwt.JwtPayload;
	try {
		decoded = jwt.verify(token, JWT_SECRET);
	} catch (exception) {
		err(403, "Failed to decode token: Malformed token?");
	}

	// Open a session with the backend
	const sessionAPI = await api.POST("/open_session", {
		body: { otp: decoded.toString() },
		fetch,
	});
	if (!sessionAPI.response.ok || sessionAPI.error) {
		const status = sessionAPI.response.status as NumericRange<400, 599>;
		err(status, sessionAPI.error?.message || "Failed to open session");
	}

	const rawSetCookie = sessionAPI.response.headers.getSetCookie()[0]!;
	const cookie = cookieParser.parseString(rawSetCookie);
	cookies.set("BACKEND_COOKIE", cookie.value, {
		path: cookie.path ?? "/",
		httpOnly: cookie.httpOnly ?? true,
		sameSite: dev ? "lax" : "strict",
		secure: cookie.secure ?? false,
		maxAge: cookie.maxAge ?? 86400, // 1 day
	});

	// Set BFF session
	const userAPI = await api.GET("/me", { fetch });
	if (!userAPI.response.ok || userAPI.error) {
		const status = userAPI.response.status as NumericRange<400, 599>;
		err(status, userAPI.error?.message || "Failed to get user");
	}

	locals.session = {
		sessionID: cookie.value,
		session: userAPI.data.user_session,
		user: {
			data: userAPI.data.user,
			details: userAPI.data.user_details,
		},
	};
	cookies.set("SESSION_COOKIE", jwt.sign(locals.session, JWT_SECRET), {
		path: cookie.path ?? "/",
		httpOnly: cookie.httpOnly ?? true,
		sameSite: dev ? "lax" : "strict",
		secure: cookie.secure ?? false,
		maxAge: cookie.maxAge ?? 86400,
	});

	const redirectURL = url.searchParams.get("redirect") ?? "/";
	redirect(303, decodeURIComponent(redirectURL));
};

// ============================================================================

export const actions: Actions = {
	default: () => redirect(301, `${apiURL}/login?redirect=`),
};
