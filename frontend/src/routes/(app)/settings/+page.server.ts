// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import cookieParser from "set-cookie-parser";
import { redirect, type Actions, error as err } from "@sveltejs/kit";
import type { PageServerLoad } from "./$types";
import api, { verify } from "$lib/api/api";
import jwt from "jsonwebtoken";
import { JWT_SECRET } from "$env/static/private";
import { dev } from "$app/environment";
import { ToastForm } from "$lib/utils";
import type { components } from "$lib/api/types";
import type { Session } from "../../../app";

// ============================================================================

export const load: PageServerLoad = async () => redirect(301, "/settings/profile");

// ============================================================================

export const actions: Actions = {
	nosessions: async ({ request, fetch, locals, cookies }) => {
		return {}
	},
	getdata: async ({ request, fetch, locals, cookies }) => {
		return {}
	},
	deleteaccount: async ({ request, fetch, locals, cookies }) => {
		return {}
	},

	profile: async ({ request, fetch, locals, cookies }) => {
		const form = await request.formData();
		const cookie = cookies.get("SESSION_COOKIE");

		if (!locals.session || !cookie)
			throw err(401, "Unauthorized");

		/** Update user details */
		const { user } = locals.session;
		const updateDetails = async () => {
			const { response, error, data } = await api.PATCH("/user_details/{id}", {
				params: { path: { id: user.details.id } },
				fetch,
				body: {
					first_name: form.get("firstName")?.toString(),
					last_name: form.get("lastName")?.toString(),
					facebook_url: form.get("facebookURL")?.toString(),
					twitter_url: form.get("twitterURL")?.toString(),
					linkedin_url: form.get("linkedinURL")?.toString(),
					github_url: form.get("githubURL")?.toString(),
					avatar_url: user.details.avatar_url,
				}
			});
			return verify(response, data, error);
		};

		/** Update user (display name mostly) */
		const updateUser = async () => {
			const { response, error, data } = await api.PATCH("/users/{id}", {
				params: { path: { id: user.data.id } },
				fetch,
				body: {
					display_name: form.get("Displayname")?.toString(),
				}
			});
			return verify(response, data, error);
		};

		/** Upload the avatar to the S3 Storage */
		const updateProfile = () => {
			const avatar = form.get("avatar") as File | null;
			if (!avatar) return;
			// TODO: Talk with @Oscar how to organize blob storage for this
		};

		/** Override the old cookie with new user data (avoids fetching again) */
		try {
			const session = jwt.decode(cookie, { json: true }) as Session | null;
			if (!session) throw new Error("Unable to decode cookie");

			session.user.details = await updateDetails();
			session.user.data = await updateUser();
			const oldCookie = cookieParser.parseString(cookie);
			cookies.set("SESSION_COOKIE", jwt.sign(session, JWT_SECRET), {
				path: oldCookie.path ?? "/",
				httpOnly: oldCookie.httpOnly ?? true,
				sameSite: (oldCookie.sameSite as "strict" | "lax" | "none") ?? "strict",
				secure: dev ? false : oldCookie.secure ?? true,
				maxAge: oldCookie.maxAge ?? 86400, // 1 day
			});

		} catch (error) {
			return ToastForm.fail(400, `Failed to update: ${error}`);
		}
		return ToastForm.success("Profile updated.");
	}
};
