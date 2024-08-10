// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { error, type Action, type Actions } from "@sveltejs/kit";
import type { PageServerLoad } from "./$types";
import api, { verify } from "$lib/api/api";
import transformAPIData, { type ApiGraph } from "$lib/graph/builder";

// ============================================================================

const generatedGraph: ApiGraph = {
	"seed": "99",
	"root": {
			"id": "0",
			"next": ["1", "2", "3", "4"],
			"prev": [],
			"type": "goal",
			"meta": {
					"name": "Graph Theory Basics",
					"state": "validated",
					"goal_id": "0",
					"final": false,
					"slug": "graph-theory-basics",
					"description": "Learn the fundamentals of graph theory"
			}
	},
	"nodes": [
			{
					"id": "1",
					"next": ["9", "10", "42"],
					"prev": ["0"],
					"type": "goal",
					"meta": {
							"name": "Algorithm Design",
							"state": "validated",
							"goal_id": "1",
							"final": false,
							"slug": "algorithm-design",
							"description": "Master algorithm design principles"
					}
			},
			{
					"id": "2",
					"next": ["11", "12", "13"],
					"prev": ["0"],
					"type": "goal",
					"meta": {
							"name": "Data Structures Mastery",
							"state": "validated",
							"goal_id": "2",
							"final": false,
							"slug": "data-structures-mastery",
							"description": "Become proficient in data structures"
					}
			},
			{
					"id": "3",
					"next": ["5", "6"],
					"prev": ["0"],
					"type": "goal",
					"meta": {
							"name": "Dynamic Programming",
							"state": "available",
							"goal_id": "3",
							"final": false,
							"slug": "dynamic-programming",
							"description": "Explore dynamic programming techniques"
					}
			},
			{
					"id": "4",
					"next": [],
					"prev": ["0"],
					"type": "action",
					"meta": {
							"type": "new:goal"
					}
			},
			{
					"id": "42",
					"next": [],
					"prev": ["1"],
					"type": "action",
					"meta": {
							"type": "new:goal"
					}
			},
			{
					"id": "9",
					"next": ["14"],
					"prev": ["1", "0"],
					"type": "goal",
					"meta": {
							"name": "Randomized Algorithms",
							"state": "suggested",
							"goal_id": "9",
							"final": false,
							"slug": "randomized-algorithms",
							"description": "Learn about randomized algorithm techniques"
					}
			},
			{
					"id": "10",
					"next": [],
					"prev": ["1"],
					"type": "goal",
					"meta": {
							"name": "Computational Geometry",
							"state": "available",
							"goal_id": "10",
							"final": true,
							"slug": "computational-geometry",
							"description": "Explore computational geometry concepts"
					}
			},
			{
					"id": "11",
					"next": ["15"],
					"prev": ["2"],
					"type": "goal",
					"meta": {
							"name": "String Algorithms",
							"state": "unavailable",
							"goal_id": "11",
							"final": false,
							"slug": "string-algorithms",
							"description": "Master algorithms for string manipulation"
					}
			},
			{
					"id": "12",
					"next": [],
					"prev": ["2"],
					"type": "action",
					"meta": {
							"type": "new:goal"
					}
			},
			{
					"id": "13",
					"next": [],
					"prev": ["2"],
					"type": "action",
					"meta": {
							"type": "new:goal"
					}
			},
	]
};



export const load: PageServerLoad = async ({ fetch }) => {
	return { users: [], graph: generatedGraph };
};

// Shared App routes
// These routes are shared between the frontend and the backend.
// ============================================================================

const noNotification: Action = async ({ request, fetch }) => {
	const data = await request.formData();
	const id = data.get("id");

	if (!id) throw error(400, "No ID provided.");

	// await fetch(`api://users/${id}/notifications`, {
	// 	method: "DELETE",
	// });
};

// const toggleFeature: Action = async ({ request, fetch }) => {
// 	const data = await request.formData();
// 	const id = data.get("id");
// 	const enabled = data.get("enabled");

// 	console.log(id, enabled);

// 	// if (!id) throw error(400, "No ID provided.");
// 	// await fetch(`api://users/${id}/notifications`, {
// 	// 	method: "DELETE",
// 	// });
// };

// ============================================================================

export const actions: Actions = {
	noNotification,
};
