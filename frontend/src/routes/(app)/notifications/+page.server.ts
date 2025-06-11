// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import {check} from "$lib/utils/check.svelte";
import z from "zod/v4";
import type {Actions, PageServerLoad} from "./$types";
import {useQuery} from "$lib/utils/url.svelte";
import {logger} from "$lib/logger";

// ============================================================================

const schema = z.object({
	page: z.coerce.number().optional(),
	size: z.coerce.number().optional(),
	mask: z.coerce.number().optional(),
	search: z.string().optional(),
	read: z.enum(["read", "unread"]).optional(),
	type: z.enum(["exclude", "include"]).optional(),
});

export type QueryKeys = keyof z.infer<typeof schema>;

// ============================================================================

export const load: PageServerLoad = async ({locals, url, depends}) => {
	depends("app:notifications");

	const query = useQuery(url, schema);
	const mask = query.read("mask");
	const exclude = query.read("type") === "exclude";
	logger.debug("Loading notifications", {
		page: query.read("page"),
		size: query.read("size"),
		mask,
		exclude,
		search: query.read("search"),
		read: query.read("read"),
	});

	const {data, response} = await check(locals.api.GET("/users/current/notifications", {
		params: { query: {
				"page[index]": query.read("page"),
				"page[size]": query.read("size"),
				[exclude ? "filter[not[kind]]" : "filter[kind]"]: mask,
				"filter[read]": query.read("read") === "read" ? true : false,
		}}
	}));

	return {
		notifications: data ?? []
	}
};

// ============================================================================

export const actions: Actions = {
	read: async ({locals, request}) => {
		logger.debug("Marking notifications as unread");

		const formData = await request.formData();
		await check(locals.api.POST("/users/current/notifications/read", {
			body: formData.getAll("id") as string[]
		}));


		return {};
	},
	unread: async ({locals, request}) => {
		logger.debug("Marking notifications as unread");
		const formData = await request.formData();
		await check(locals.api.POST("/users/current/notifications/unread", {
			body: formData.getAll("id") as string[]
		}));

		return {};
	}
};
