import { check } from "$lib/utils/check.svelte";
import type { PageServerLoad } from "./$types";

export const load: PageServerLoad = async ({ locals, url }) => {
	const { data } = await check(locals.api.GET("/users/current/notifications", {
		params: { query: {
			Page: 0,
			Size: 10,
			"filter[read]": url.searchParams.get("read") ?? undefined
		}}
	}))

	return {
		notifications: data ?? []
	}

	// const data = new Promise<BackendNotification[]>((resolve) => {
	// 	const notifications: BackendNotification[] = Array.from({ length: 1000 }, (_, i) => ({
	// 		id: (i + 1).toString(),
	// 		type: i % 2 === 0 ? "invite" : "notification" as const,
	// 		title: `Notification ${i + 1}`,
	// 		createdAt: new Date(Date.now() + i * 24 * 60 * 60 * 1000),
	// 	}));
	// 	setTimeout(() => resolve(notifications), 4000);
	// });

	// const notifications: BackendNotification[] = Array.from({ length: 1000 }, (_, i) => ({
	// 	id: (i + 1).toString(),
	// 	type: Math.random() >= 0.5 ? "invite" : "notification" as const,
	// 	title: `Notification ${i + 1}`,
	// 	createdAt: new Date(Date.now() + i * 24 * 60 * 60 * 1000),
	// }));

	// return {
	// 	notifications
	// };
};
