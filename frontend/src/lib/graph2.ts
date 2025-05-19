// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import type { Vector3 } from "three";

// ============================================================================

export type Options = {
	/** Add leaflet nodes to the end of nodes to allow the graph to be extended */
	addLeaflets?: boolean;
	/** Ignore Z axis, used for the 2D Graph view */
	flat?: boolean;
};

// ============================================================================

export type State =
	| "validated"
	| "available"
	| "in_progress"
	| "unavailable"
	| "suggested";
export type ActionTypes = "new:goal";

type BaseNode = {
	id: GUID;
	next: GUID[];
	prev: GUID[];
};

export type Goal = {
	name: string;
	state: State;
	goal_id: GUID;
	slug: string;
	final: boolean;
	description: string;
};

export type Action = {
	type: ActionTypes;
};

export type Types = Goal | Action;

type XNode<T = Types> = BaseNode & {
	type: T extends Goal ? "goal" : "action";
	meta: T;
};

export type ApiGraph = {
	seed: string;
	root: XNode;
	nodes: XNode[];
};

export type Node<T = Types> = XNode<T> & {
	position: Vector3;
	depth: number;
};

/** Edge between 2 nodes */
export type Edge = {
	state: State;
	source: Node;
	target: Node;
};

/** Structure to render in either 3D or 2D. */
export type RenderGraph = {
	seed: string;
	nodes: Node[];
	edges: Edge[];
};

// ============================================================================

export function buildGraph(raw: Uint8Array, options: Options = {}) {

	if (node === null) {

	}
}
