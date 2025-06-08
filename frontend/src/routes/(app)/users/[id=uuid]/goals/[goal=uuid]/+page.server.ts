import { decodeID } from '$lib/utils';
import { check } from '$lib/utils/check.svelte';
import { error } from '@sveltejs/kit';
import type { PageServerLoad, RouteParams } from './$types';
import { logger } from '$lib/logger';

async function fetchGoal(locals: App.Locals, goalId: GUID) {
	const { response, data } = await check(locals.api.GET("/goals", {
		params: {
			query: {
				"filter[id]": decodeID(goalId),
			}
		}
	}));

	return data?.at(0) ?? error(404);
}

async function fetchUserGoal(locals: App.Locals, goalId: GUID, userId: GUID) {
	const { response, data } = await locals.api.GET("/users/{id}/goals", {
		params: {
			path: { id: userId },
			query: {
				"filter[goal_id]": decodeID(goalId),
			}
		}
	});

	if (!response.ok && response.status !== 404)
		error(response.status, `Failed to fetch user goal: ${response.statusText}`);
	return data?.at(0) ?? null;
}

export const load: PageServerLoad = async ({ url, params, locals }) => {
	const goalId = decodeID(params.goal);
	logger.info(`Loading goal with ID: ${goalId} for user with ID: ${params.id}`, { url: url.pathname });
	const [goal, userGoal] = await Promise.all([
		fetchGoal(locals, goalId),
		fetchUserGoal(locals, goalId, decodeID(params.id))
	]);

	return {
		goal,
		userGoal,
	};
};
