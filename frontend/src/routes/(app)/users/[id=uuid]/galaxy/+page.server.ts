// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

// import { XGraphV1, } from "@w2inc/xgraph";
import type { PageServerLoad } from "./$types";

// ============================================================================

// const node: XGraphV1.Node = {
// 	id: 0,
// 	parentId: -1,
// 	goals: [],
// 	children: [
// 		{
// 			id: 1,
// 			parentId: 0,
// 			goals: [],
// 			children: [],
// 		},
// 		{
// 			id: 2,
// 			parentId: 0,
// 			goals: [
// 				{
// 					goalGUID: "d0fd9301-e04c-e67d-865e-a075db322e13",
// 					name: "1",
// 					state: 0,
// 					description: "woa"
// 				},
// 				{
// 					goalGUID: "d0fd9301-e04c-e67d-865e-a075db322e13",
// 					name: "1",
// 					state: 0,
// 					description: "woa"
// 				},
// 				{
// 					goalGUID: "d0fd9301-e04c-e67d-865e-a075db322e13",
// 					name: "1",
// 					state: 0,
// 					description: "woa"
// 				},
// 			],
// 			children: [],
// 		},
// 	],
// };

// ============================================================================

export const load: PageServerLoad = async ({ locals }) => {
	// const wr = new XGraphV1.Writer();
	// wr.serialize(node);

	// return {
	// 	buff: Buffer.from(wr.toArrayBuffer()).toString("base64url"),
	// 	cursus: new Promise<Array<string>>((resolve) =>
	// 		setTimeout(() => resolve([]), Math.random() * 2000),
	// 	),
	// };
};
