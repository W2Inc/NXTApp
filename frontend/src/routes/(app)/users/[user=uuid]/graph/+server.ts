// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import api from "$lib/api/api";
import MockCore from "$lib/graph/mock-core.json";
import MockDev from "$lib/graph/mock-dev.json";
import MockPisc from "$lib/graph/mock-piscine.json";
import { json, type RequestHandler } from "@sveltejs/kit";

const coregraph = {
		"seed": "uuid-v4-1337-420-69-666-4",
		"root": {
			"id": "0",
			"next": ["1", "2", "3", "4"],
			"prev": [],
			"type": "goal",
			"meta": {
				"name": "libasm",
				"state": "validated",
				"goal_id": "0",
				"final": false,
				"slug": "libasm",
				"description": "Look into how to create a lib with NASM"
			}
		},
		"nodes": [
			{
				"id": "1",
				"next": ["9", "10", "42"],
				"prev": ["0"],
				"type": "goal",
				"meta": {
					"name": "miniGM",
					"state": "in_progress",
					"goal_id": "1",
					"final": false,
					"slug": "minigm",
					"description": "Code your first small game"
				}
			},
			{
				"id": "2",
				"next": ["11", "12", "13"],
				"prev": ["0"],
				"type": "goal",
				"meta": {
					"name": "VMMaster",
					"state": "available",
					"goal_id": "2",
					"final": false,
					"slug": "vmmaster",
					"description": "Virtual Box is broken, rip."
				}
			},
			{
				"id": "3",
				"next": ["5", "6"],
				"prev": ["0"],
				"type": "goal",
				"meta": {
					"name": "Dr Docker",
					"state": "unavailable",
					"goal_id": "3",
					"final": false,
					"slug": "drdocker",
					"description": "Time to enter deployment hell."
				}
			},
			{
				"id": "4",
				"next": [],
				"prev": ["0"],
				"type": "goal",
				"meta": {
					"name": "Dr RTX",
					"state": "unavailable",
					"goal_id": "4",
					"final": false,
					"slug": "drrtx",
					"description": "Time to enter deployment hell."
				}
			},
			{
				"id": "42",
				"next": [],
				"prev": ["1"],
				"type": "goal",
				"meta": {
					"name": "miniGM2",
					"state": "suggested",
					"goal_id": "42",
					"final": false,
					"slug": "minigm2",
					"description": "Code your first small gam2e"
				}
			},
			{
				"id": "9",
				"next": [],
				"prev": ["1", "0"],
				"type": "goal",
				"meta": {
					"name": "LinuxKernel22",
					"state": "unavailable",
					"goal_id": "9",
					"final": false,
					"slug": "linuxkernel22",
					"description": "Make a kernel."
				}
			},
			{
				"id": "10",
				"next": [],
				"prev": ["1"],
				"type": "goal",
				"meta": {
					"name": "FS22",
					"state": "unavailable",
					"goal_id": "10",
					"final": false,
					"slug": "fs22",
					"description": "Make a file system."
				}
			},
			{
				"id": "11",
				"next": [],
				"prev": ["6"],
				"type": "goal",
				"meta": {
					"name": "libhaskell",
					"state": "unavailable",
					"goal_id": "11",
					"final": false,
					"slug": "libhaskell",
					"description": "Haskell so that you can be a hipster."
				}
			},
			{
				"id": "12",
				"next": [],
				"prev": ["1", "0"],
				"type": "action",
				"meta": {
					"count": 0,
					"type": "new:goal"
				}
			},
			{
				"id": "13",
				"next": [],
				"prev": ["1"],
				"type": "goal",
				"meta": {
					"name": "HolyC",
					"state": "suggested",
					"goal_id": "13",
					"final": false,
					"slug": "holyc",
					"description": "Terry Davis would be proud."
				}
			}
		]
	}

// ============================================================================

export const GET: RequestHandler = async ({ url, fetch, params }) => {
	const cursus = url.searchParams.get("cursus");
	switch (cursus) {
		case "Core":
			return json(coregraph);
		case "Piscine":
			return json(coregraph);
		case "Dev":
			return json(coregraph);
		default: {
			return new Response("Cursus not found", {
				status: 404,
			});
		}
	}
};
