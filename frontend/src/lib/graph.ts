// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// ============================================================================

import * as THREE from "three";
import AleaPRNG from "./utils";

// Types
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

type ApiNode<T = Types> = BaseNode & {
	type: T extends Goal ? "goal" : "action";
	meta: T;
};

export type ApiGraph = {
	seed: string;
	root: ApiNode;
	nodes: ApiNode[];
};

export type Node<T = Types> = ApiNode<T> & {
	position: THREE.Vector3;
	depth: number;
};

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

// XGraphV1 Adapter Types
// ============================================================================

export type XGraphOptions = {
	addEndActionNodes?: boolean;
	flatten?: boolean;
};

// Helper Functions
// ============================================================================

function createNode(
	apiNode: ApiNode,
	position = new THREE.Vector3(0, 0, 0),
	depth = 0,
): Node {
	return { ...apiNode, position, depth };
}

function createCircleVertices(count: number, radius: number): THREE.Vector3[] {
	const angleStep = (Math.PI * 2) / count;
	return Array.from({ length: count }, (_, i) => {
		const angle = i * angleStep;
		return new THREE.Vector3(radius * Math.cos(angle), 0, radius * Math.sin(angle));
	});
}

/** Arranges first-level nodes in a circle around the root node */
function arrangeCoreNodes(rand: AleaPRNG, data: ApiGraph, graph: ClientGraph): Node[] {
	// Create root node
	const root = createNode(data.root);
	graph.nodes.push(root);

	if (root.type === "action") return [];
	const coreApiNodes = data.nodes.filter(
		(node) =>
			(node.type === "goal" && node.prev.includes(data.root.id)) ||
			(node.type === "action" && root.next.includes(node.id)),
	);

	if (coreApiNodes.length === 0) return [];
	const positions = createCircleVertices(coreApiNodes.length, 10);
	const coreNodes = coreApiNodes.map((apiNode, i) => {
		const position = positions[i].clone().setY(rand.range(-5, 5));
		const node = createNode(apiNode, position, 1);
		graph.links.push({
			state: root.type === "goal" ? root.meta.state : "available",
			source: root,
			target: node,
		});

		return node;
	});

	graph.nodes.push(...coreNodes);
	return coreNodes;
}

/**
 * Finds a suitable position for a new node based on its parent
 */
function calculateNodePosition(
	rand: AleaPRNG,
	parentPosition: THREE.Vector3,
	existingNodes: Node[],
): THREE.Vector3 {
	const Y_AXIS = new THREE.Vector3(0, 1, 0);
	const MAX_ATTEMPTS = 100;
	const MIN_DISTANCE = 5;

	let attempts = 0;
	let position = new THREE.Vector3();
	while (attempts < MAX_ATTEMPTS) {
		const height = rand.range(-5, 5);
		const distance = rand.range(10, 20);
		const angle = rand.range(-180, 180);

		position = parentPosition
			.clone()
			.add(
				new THREE.Vector3(distance, 0, 0)
					.applyAxisAngle(Y_AXIS, THREE.MathUtils.degToRad(angle))
					.setY(height),
			);

		// Check if position is far enough from all other nodes
		if (
			!existingNodes.some((node) => node.position.distanceTo(position) < MIN_DISTANCE)
		) {
			break;
		}

		attempts++;
	}

	return position;
}

/**
 * Flattens a 3D graph to 2D by projecting onto the XZ plane
 */
function flattenGraph(graph: ClientGraph): ClientGraph {
	const flattenedGraph: ClientGraph = {
		seed: graph.seed,
		nodes: graph.nodes.map((node) => ({
			...node,
			position: new THREE.Vector3(node.position.x, 0, node.position.z),
		})),
		links: [],
	};

	// Recreate links with the flattened nodes
	flattenedGraph.links = graph.links.map((link) => {
		const sourceNode = flattenedGraph.nodes.find((n) => n.id === link.source.id);
		const targetNode = flattenedGraph.nodes.find((n) => n.id === link.target.id);

		if (!sourceNode || !targetNode) {
			throw new Error(
				`Could not find nodes for link: ${link.source.id} -> ${link.target.id}`,
			);
		}

		return {
			state: link.state,
			source: sourceNode,
			target: targetNode,
		};
	});

	return flattenedGraph;
}

/**
 * Check if a node is an end node (has no next nodes)
 */
function isEndNode(node: Node, graph: ClientGraph): boolean {
	return (
		node.next.length === 0 ||
		!node.next.some((nextId) => graph.nodes.some((n) => n.id === nextId))
	);
}

/**
 * Create action nodes at the end of paths if needed
 */
function addActionNodesToEnds(graph: ClientGraph, rand: AleaPRNG): void {
	const endNodes = graph.nodes.filter((node) => isEndNode(node, graph));

	endNodes.forEach((endNode) => {
		// Create a new action node
		const actionMeta: Action = { type: "new:goal" };
		const actionNode: ApiNode<Action> = {
			id: `action-${endNode.id}-${Date.now()}` as GUID,
			next: [],
			prev: [endNode.id],
			type: "action",
			meta: actionMeta,
		};

		// Position the action node
		const position = calculateNodePosition(rand, endNode.position, graph.nodes);
		const actionClientNode = createNode(actionNode, position, endNode.depth + 1);

		// Update the end node's next array
		endNode.next.push(actionClientNode.id);

		// Add the new node and link to the graph
		graph.nodes.push(actionClientNode);
		graph.links.push({
			state: "available",
			source: endNode,
			target: actionClientNode,
		});
	});
}

// XGraphV1 Conversion Functions
// ============================================================================

/**
 * Convert XGraphV1.Goal to ApiNode Goal
 */
function convertXGraphGoalToApiGoal(xgraphGoal: XGraphV1.Goal): Goal {
	const stateMap: Record<XGraphV1.GoalState, State> = {
		[XGraphV1.GoalState.NotStarted]: "available",
		[XGraphV1.GoalState.InProgress]: "in_progress",
		[XGraphV1.GoalState.Completed]: "validated",
		[XGraphV1.GoalState.Failed]: "unavailable",
	};

	return {
		name: xgraphGoal.name,
		state: stateMap[xgraphGoal.state] || "available",
		goal_id: xgraphGoal.goalGUID as GUID,
		slug: xgraphGoal.name.toLowerCase().replace(/\s+/g, "-"),
		final: false, // Default value - no equivalent in XGraphV1
		description: xgraphGoal.description,
	};
}

/**
 * Convert XGraphV1.Node to ApiGraph structure
 */
function convertXGraphNodeToApiGraph(
	xgraphNode: XGraphV1.Node,
	seed: string = "default",
): ApiGraph {
	const nodeMap = new Map<number, ApiNode>();
	const allNodes: ApiNode[] = [];

	// Process node and its children recursively
	function processNode(node: XGraphV1.Node, parentId: GUID | null = null): ApiNode {
		// If we've already processed this node, return the cached result
		if (nodeMap.has(node.id)) {
			return nodeMap.get(node.id)!;
		}

		// Create an API node for this XGraph node
		let apiNode: ApiNode;

		if (node.goals.length > 0) {
			// Use the first goal as the main goal for this node
			const mainGoal = convertXGraphGoalToApiGoal(node.goals[0]);
			apiNode = {
				id: `goal-${node.id}` as GUID,
				next: [],
				prev: parentId ? [parentId] : [],
				type: "goal",
				meta: mainGoal,
			};
		} else {
			// If no goals, create an action node
			apiNode = {
				id: `action-${node.id}` as GUID,
				next: [],
				prev: parentId ? [parentId] : [],
				type: "action",
				meta: { type: "new:goal" },
			};
		}

		// Cache and collect the node
		nodeMap.set(node.id, apiNode);
		allNodes.push(apiNode);

		// Process children
		for (const child of node.children) {
			const childApiNode = processNode(child, apiNode.id);
			apiNode.next.push(childApiNode.id);
		}

		return apiNode;
	}

	// Start processing from the root
	const rootApiNode = processNode(xgraphNode);

	return {
		seed,
		root: rootApiNode,
		nodes: allNodes,
	};
}

// Main Functions
// ============================================================================

/**
 * Transforms API graph data into a renderable client graph structure
 */
export function transformAPIData(
	data: ApiGraph,
	options: XGraphOptions = {},
): ClientGraph {
	if (data.nodes.length === 0 || data.root.type === "action") {
		return {
			seed: data.seed,
			nodes: [createNode(data.root)],
			links: [],
		};
	}

	const graph: ClientGraph = {
		seed: data.seed,
		nodes: [],
		links: [],
	};

	const rand = new AleaPRNG(data.seed);
	const coreNodes = arrangeCoreNodes(rand, data, graph);
	const processChildren = (parent: Node, depth: number) => {
		if (parent.type === "action") return;

		for (const childId of parent.next) {
			const childApiNode = data.nodes.find((node) => node.id === childId);
			if (!childApiNode) continue;

			// Calculate position for child node
			const position = calculateNodePosition(rand, parent.position, graph.nodes);

			// Create the node and add to graph
			const childNode = createNode(childApiNode, position, depth);
			graph.nodes.push(childNode);

			// Create link from parent to child
			graph.links.push({
				state: childNode.type === "goal" ? childNode.meta.state : "available",
				source: parent,
				target: childNode,
			});

			// Process this node's children
			processChildren(childNode, depth + 1);
		}
	};

	// Process children for each core node
	coreNodes.forEach((node) => {
		if (node.type !== "goal") return;
		processChildren(node, 2);
	});

	// Add action nodes to end nodes if requested
	if (options.addEndActionNodes) {
		addActionNodesToEnds(graph, rand);
	}

	// Flatten the graph if requested
	if (options.flatten) {
		return flattenGraph(graph);
	}

	return graph;
}

/**
 * Transforms XGraphV1 data into a renderable client graph structure
 */
export function transformXGraphV1Data(
	xgraphNode: XGraphV1.Node,
	options: XGraphOptions = {},
	seed: string = Date.now().toString(),
): ClientGraph {
	// Convert XGraphV1 to ApiGraph format
	const apiGraph = convertXGraphNodeToApiGraph(xgraphNode, seed);

	// Use the existing transform function
	return transformAPIData(apiGraph, options);
}

export default transformAPIData;
