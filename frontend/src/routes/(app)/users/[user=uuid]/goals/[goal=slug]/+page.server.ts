// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { error } from "@sveltejs/kit";
import type { PageServerLoad } from "./$types";

// ============================================================================

export const load: PageServerLoad = async ({ parent, params }) => {
	const parentData = await parent();
	const goal = parentData.goals.find((goal) => goal.id.toString() === params.goal);
	if (!goal) error(404, "Goal not found");
	// TODO: If not in the layout data, fetch the goal from the API directly

	return {
		goal,
	};
};
