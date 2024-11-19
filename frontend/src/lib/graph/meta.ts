// ============================================================================
// W2Inc, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import type { GUID } from "../../types";

export interface GoalEntry {
	name: string;
	goalId: GUID;
}

export class GraphNode {
	static readonly C_MAX_NODES: number = 4;
	static readonly C_MAX_GOALS: number = 3;
	static readonly C_MAX_DEPTH: number = 255; // byte.MaxValue in C#

	id: number;
	parentId: number;
	isRoot: boolean;
	goalCount: number;
	childrenCount: number;
	goals: GoalEntry[] = [];
	children: GraphNode[] = [];

	constructor(
		id: number,
		parentId: number,
		isRoot: boolean,
		goalCount: number,
		childrenCount: number
	) {
		this.id = id;
		this.parentId = parentId;
		this.isRoot = isRoot;
		this.goalCount = goalCount;
		this.childrenCount = childrenCount;
	}
}
