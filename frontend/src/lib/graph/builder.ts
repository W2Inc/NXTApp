// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import * as THREE from "three";
import AleaPRNG from "./aleaPRNG";
import type { GUID } from "$lib/types";

// Shared Types
// ============================================================================

export type State =
	| "validated"
	| "available"
	| "in_progress"
	| "unavailable"
	| "suggested";

// The different existing node types.
export type Types = Goal | Action;
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
	final: boolean; // Is this the final goal?
	description: string;
};

export type Action = {
	type: ActionTypes;
};

// API Types
// ============================================================================

type ApiNode<T = Types> = T extends Goal
	? BaseNode & { type: "goal"; meta: Goal }
	: T extends Action
		? BaseNode & { type: "action"; meta: Action }
		: never;

export type ApiGraph = {
	seed: string;
	root: ApiNode;
	nodes: ApiNode[];
};

// Render Types
// ============================================================================

export type Node<T = Types> = {
	position: THREE.Vector3;
	depth: number;
} & ApiNode<T>;

export type Link = {
	state: State;
	source: Node;
	target: Node;
};

export type ClientGraph = {
	seed: string;
	nodes: Node[];
	links: Link[];
};

// Helper Functions
// ============================================================================

function newNode(node: ApiNode, pos = new THREE.Vector3(0, 0, 0), depth = 0) {
	return { ...node, depth, position: pos } as Node;
}

/** Create a rank of points around a circle. */
function createRank(points: number, radius: number): THREE.Vector3[] {
	const step = 360.0 / points;
	const master: THREE.Vector3[] = [];
	const origin = new THREE.Vector3(0, 0, 0);

	let theta = 0;
	for (let i = 0; i < points; i++) {
		const position = new THREE.Vector3(
			origin.x + radius * Math.cos(THREE.MathUtils.degToRad(theta)),
			0,
			origin.z + radius * Math.sin(THREE.MathUtils.degToRad(theta))
		);

		master.push(position);
		theta += step;
	}

	return master;
}

/**
 * Core nodes refer to the nodes that are not the root node but that are
 * the children of the root node, these nodes are arranged in a circle around
 * the root node. They're evenly spaced out to ensure that the graph is
 * not biased towards a certain direction later on when nodes are placed
 * randomly by the PRNG.
 */
function arrangeCore(rand: AleaPRNG, data: ApiGraph, graph: ClientGraph) {
	let root = newNode(data.root);
	graph.nodes.push(root);
	if (root.type === "action") {
		return [];
	}

	const apiCoreNodes = data.nodes.filter((node) => {
		switch (node.type) {
			case "goal":
				return node.prev.includes(data.root.id);
			case "action":
				return root.next.includes(node.id);
			default:
				throw new Error("Unknown registered node type");
		}
	});

	if (apiCoreNodes.length === 0) {
		return [];
	}

	let coreNodes: Node[] = [];
	const points = createRank(apiCoreNodes.length, 10);
	for (const point of points) {
		const coreNode = apiCoreNodes.shift();
		if (!coreNode) break;

		point.setY(rand.range(-5, 5));
		const node = newNode(coreNode, point, 1);
		coreNodes.push(node);

		graph.links.push({
			state: root.meta.state,
			source: root,
			target: node,
		});
	}

	graph.nodes.push(...coreNodes);
	return coreNodes;
}

// Default function to create a graph from the API data.
// ============================================================================

/**
 * Transform the API data into a format that can be used by the graph.
 * @param data - The data returned from the API.
 * @returns The data that can be used by the graph for rendering.
 */
export default function transformAPIData(data: ApiGraph): ClientGraph {
	if (data.nodes.length == 0 || data.root.type === "action") {
		return {
			seed: data.seed,
			nodes: [newNode(data.root)],
			links: [],
		};
	}

	let maxDepth = 0;
	let graph: ClientGraph = {
		seed: data.seed,
		nodes: [],
		links: [],
	};

	/**
	 * Given the parent position, we want to create a new position that is
	 * a random distance away from the parent position, but in the same
	 * direction. However, we also want to make sure that the new position
	 * is between -N and N degrees rotated from the parent position.
	 *
	 * Ensuring that the new position is:
	 * - Not too close to the parent position or other nodes in the same depth
	 * - That no 0 degree angles are created but rather a range of angles
	 *   so far this seems to be working fine, but I need to test it more
	 *   with real data.
	 */

	const rand = new AleaPRNG(data.seed);
	const axisYDirection = new THREE.Vector3(0, 1, 0);
	const createNodes = (apiNode: ApiNode, parent: Node, depth: number) => {
		let i = 0;
		const max = 100;
		const pos = parent.position.clone();

		// Figure out the position of the new node making sure that it's not
		// too close to the parent node or any other nodes in the same depth.
		while (i < max && graph.nodes.some((node) => node.position.distanceTo(pos) < 5)) {
			const height = rand.range(-5, 5);
			const distance = rand.range(5, 10);
			const angle = rand.range(-45, 45);

			// TODO: Clamp the final height between a Upper and Lower bound.
			pos.applyAxisAngle(axisYDirection, THREE.MathUtils.degToRad(angle));
			pos.setLength(distance);
			pos.setY(height);
			pos.add(parent.position);

			i++;
		}

		const node = newNode(apiNode, pos, depth);
		maxDepth = Math.max(maxDepth, depth);
		graph.nodes.push(node);
		graph.links.push({
			state: node.type === "goal" ? node.meta.state : "available",
			source: parent,
			target: node,
		});

		if (node.type === "action") return;

		// Follow down the children of the node.
		for (const id of node.next) {
			const next = data.nodes.find((node) => node.id === id);
			if (next) createNodes(next, node, depth + 1);
		}
	};

	// Here we actually start to create the graph.
	const coreNodes = arrangeCore(rand, data, graph);
	for (const coreNode of coreNodes) {
		if (coreNode.type === "action") continue;

		for (const id of coreNode.next) {
			const next = data.nodes.find((node) => node.id === id);
			if (next) createNodes(next, coreNode, 2);
		}
	}

	return graph;
}
