<script lang="ts">
	import GoalNode from "$lib/components/graph/goal-node.svelte";
	import GoalNode2 from "$lib/components/graph/goal-node.svelte";
	import { XGraphV1 } from "@w2inc/xgraph";
	import {
		SvelteFlow,
		Controls,
		Background,
		BackgroundVariant,
		Position,
		MiniMap,
		Panel,
		type ColorMode,
		type NodeTypes,
		type Node,
		type Edge,
	} from "@xyflow/svelte";
	import "@xyflow/svelte/dist/style.css";


	interface Props {
		data: ArrayBufferLike;
	}

	const { data }: Props = $props();

	const nodeTypes: NodeTypes = {
		goalNode: GoalNode2,
	};

	// Expanded tree with more nodes
	let rawNodes: XGraphV1.Node = {
		id: 0,
		parentId: 0,
		goals: [],
		children: [
			{
				// Should connect to the top from root
				id: 1,
				parentId: 0,
				goals: [],
				children: [
					{
						id: 4,
						parentId: 1,
						goals: [],
						children: [],
					},
					{
						id: 5,
						parentId: 1,
						goals: [],
						children: [],
					},
				],
			},
			{
				// Should connect to the right from root
				id: 2,
				parentId: 0,
				goals: [],
				children: [
					{
						id: 6,
						parentId: 2,
						goals: [],
						children: [
							{
								id: 9,
								parentId: 6,
								goals: [],
								children: [],
							},
						],
					},
					{
						id: 7,
						parentId: 2,
						goals: [],
						children: [],
					},
				],
			},
			{
				// Should connect to the bottom from root...
				id: 3,
				parentId: 0,
				goals: [],
				children: [
					{
						id: 8,
						parentId: 3,
						goals: [],
						children: [],
					},
				],
			},
		],
	};

	function convertTreeToGraph() {
		const reader = new XGraphV1.Reader(data as ArrayBuffer);
		reader.deserialize();

	}

	// Initialize the graph
	$effect(() => convertTreeToGraph());
	let nodes = $state.raw<Node[]>([]);
	let edges = $state.raw<Edge[]>([]);
	let colorMode: ColorMode = $state("system");
</script>

<SvelteFlow {nodeTypes} bind:nodes bind:edges {colorMode} colorModeSSR={"dark"} fitView>
	<Controls position="top-right" />
	<Background variant={BackgroundVariant.Dots} />
</SvelteFlow>
