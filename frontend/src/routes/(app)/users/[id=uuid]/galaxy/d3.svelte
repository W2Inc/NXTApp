<script lang="ts">
	import * as d3 from "d3";
	import Reference from "./reference.json" with { type: "json" };
	import { untrack } from "svelte";

	// Type definitions
	type Goal = {
		name: string;
		guid: string;
		state: number;
	};

	type TreeNode = {
		name: string;
		path: string;
		goals: Goal[];
		children: TreeNode[];
	};

	// State management using Svelte 5 runes
	let currentNode = $state(Reference);
	let nodeHistory = $state<TreeNode[]>([]);
	let selectedNode = $state<d3.HierarchyNode<TreeNode> | null>(null);
	let svg = $state<SVGSVGElement | null>(null);
	let width = $state(window.innerWidth);
	let height = $state(window.innerHeight);
	let simulation = $state<d3.Simulation<
		d3.HierarchyNode<TreeNode>,
		d3.SimulationLinkDatum<d3.HierarchyNode<TreeNode>>
	> | null>(null);
	let zoomLevel = $state(1);

	// Function to navigate to a node
	const navigateToNode = (node: TreeNode) => {
		nodeHistory.push(currentNode);
		currentNode = node;
	};

	// Function to navigate back
	const navigateBack = () => {
		if (nodeHistory.length > 0) {
			currentNode = nodeHistory.pop()!;
		}
	};

	// Keyboard event handler
	const handleKeydown = (event: KeyboardEvent) => {
		if (event.key === "Escape") {
			navigateBack();
		}
	};

	// Resize handler
	const handleResize = () => {
		width = window.innerWidth;
		height = window.innerHeight;
		renderGraph();
	};

	// Create draggable behavior for nodes
	const drag = (simulation: d3.Simulation<d3.HierarchyNode<TreeNode>, undefined>) => {
		function dragstarted(event: any, d: any) {
			if (!event.active) simulation.alphaTarget(0.3).restart();
			d.fx = d.x;
			d.fy = d.y;
		}

		function dragged(event: any, d: any) {
			d.fx = event.x;
			d.fy = event.y;
		}

		function dragended(event: any, d: any) {
			if (!event.active) simulation.alphaTarget(0);
			d.fx = null;
			d.fy = null;
		}

		return d3
			.drag<any, any, any>()
			.on("start", dragstarted)
			.on("drag", dragged)
			.on("end", dragended);
	};

	// Function for rendering the graph
	const renderGraph = () => {
		if (!svg) return;

		d3.select(svg).selectAll("*").remove();

		const svgContainer = d3
			.select(svg)
			.attr("width", width)
			.attr("height", height)
			.attr("viewBox", [0, 0, width, height])
			.attr("class", "bg-gray-50 dark:bg-gray-900");

		// Create zoom behavior
		const zoom = d3
			.zoom<SVGSVGElement, unknown>()
			.scaleExtent([0.1, 5])
			.on("zoom", (event) => {
				zoomLevel = event.transform.k;
				svgGroup.attr("transform", event.transform);
			});

		svgContainer.call(zoom);

		const svgGroup = svgContainer
			.append("g")
			.attr("transform", `translate(${width / 2}, ${height / 2})`);

		// Create hierarchy from current node
		const root = d3.hierarchy(currentNode);
		const links = root.links();
		const nodes = root.descendants();

		// Limit depth for visualization
		const visibleDepth = 3;
		const visibleNodes = nodes.filter((d) => d.depth <= visibleDepth);
		const visibleLinks = links.filter((d) => d.source.depth < visibleDepth);

		// Create simulation
		simulation = d3
			.forceSimulation<d3.HierarchyNode<TreeNode>>()
			.force(
				"link",
				d3
					.forceLink<
						d3.HierarchyNode<TreeNode>,
						d3.SimulationLinkDatum<d3.HierarchyNode<TreeNode>>
					>(visibleLinks)
					.id((d) => d.data.path)
					.distance(100)
					.strength(0.7),
			)
			.force("charge", d3.forceManyBody().strength(-500))
			.force("collide", d3.forceCollide().radius(60))
			.force("x", d3.forceX().strength(0.05))
			.force("y", d3.forceY().strength(0.05))
			.nodes(visibleNodes);

		// Create links
		const link = svgGroup
			.append("g")
			.attr("class", "links")
			.selectAll("line")
			.data(visibleLinks)
			.enter()
			.append("line")
			.attr("class", "link")
			.attr("stroke", "#64748b")
			.attr("stroke-opacity", 0.6)
			.attr("stroke-width", 2);

		// Create node groups
		const node = svgGroup
			.append("g")
			.attr("class", "nodes")
			.selectAll("g")
			.data(visibleNodes)
			.enter()
			.append("g")
			.attr("class", "node")
			.call(drag(simulation))
			.on("click", (event, d) => {
				event.stopPropagation();
				selectedNode = d;

				// If the node has children and is at max depth, navigate to it
				if (d.depth === visibleDepth && d.data.children.length > 0) {
					navigateToNode(d.data);
				}
			});

		// Create node circles
		node
			.append("circle")
			.attr("r", (d) => (d.depth === 0 ? 40 : 30)) // Root node is larger
			.attr("fill", (d) => {
				// Color based on node state
				if (d.data.goals.some((g) => g.state === 1)) {
					return "#10b981"; // Active - green
				} else if (d.depth === visibleDepth && d.data.children.length > 0) {
					return "#f59e0b"; // Route node - amber
				} else {
					return "#6b7280"; // Inactive - gray
				}
			})
			.attr("stroke", "#f8fafc")
			.attr("stroke-width", 2)
			.attr(
				"class",
				"transition-all duration-200 ease-in-out hover:stroke-blue-500 hover:stroke-[3px]",
			);

		// Add node text
		node
			.append("text")
			.attr("dy", (d) => (d.depth === 0 ? "0.35em" : "0.30em"))
			.attr("text-anchor", "middle")
			.attr("fill", "#f8fafc")
			.attr("font-size", (d) => (d.depth === 0 ? "12px" : "10px"))
			.attr("class", "select-none pointer-events-none")
			.text((d) =>
				d.data.name.length > 15 ? d.data.name.substring(0, 15) + "..." : d.data.name,
			)
			.clone(true)
			.lower()
			.attr("fill", "none")
			.attr("stroke", "#1f2937")
			.attr("stroke-width", 3);

		// Add indicators for nodes with children at max depth
		node
			.filter((d) => d.depth === visibleDepth && d.data.children.length > 0)
			.append("circle")
			.attr("r", 7)
			.attr("cx", 20)
			.attr("cy", -20)
			.attr("fill", "#3b82f6")
			.attr("class", "pulse-animation");

		// Simulation tick function
		simulation.on("tick", () => {
			link
				.attr("x1", (d) => (d.source as any).x)
				.attr("y1", (d) => (d.source as any).y)
				.attr("x2", (d) => (d.target as any).x)
				.attr("y2", (d) => (d.target as any).y);

			node.attr("transform", (d) => `translate(${d.x}, ${d.y})`);
		});

		// Stop simulation after settling
		simulation.alphaDecay(0.028);
	};

	// Effect to redraw when current node changes

	/*

		let currentNode = $state(Reference);
	let nodeHistory = $state<TreeNode[]>([]);
	let selectedNode = $state<d3.HierarchyNode<TreeNode> | null>(null);
	let svg = $state<SVGSVGElement | null>(null);
	let width = $state(window.innerWidth);
	let height = $state(window.innerHeight);
	let simulation = $state<d3.Simulation<
		d3.HierarchyNode<TreeNode>,
		d3.SimulationLinkDatum<d3.HierarchyNode<TreeNode>>
	> | null>(null);
	let zoomLevel = $state(1);
	*/

	$effect.root(() => {
		renderGraph();
	});

	// Setup event listeners when component mounts
	$effect.root(() => {
		window.addEventListener("resize", handleResize);
		window.addEventListener("keydown", handleKeydown);

		return () => {
			window.removeEventListener("resize", handleResize);
			window.removeEventListener("keydown", handleKeydown);
			if (simulation) simulation.stop();
		};
	});
</script>

<svelte:head>
	<title>Cursus Track Visualization</title>
</svelte:head>

<div
	class="relative h-[100dvh] w-full overflow-hidden bg-gray-50 text-gray-900 dark:bg-gray-900 dark:text-gray-100"
>
	<!-- Main visualization -->
	<svg bind:this={svg} class="h-full w-full"></svg>

	<!-- UI Controls -->
	<div class="absolute left-4 top-4 flex gap-2">
		<button
			class="rounded-md bg-blue-600 px-4 py-2 text-white shadow transition-colors hover:bg-blue-700"
			onclick={() => navigateBack()}
			disabled={nodeHistory.length === 0}
		>
			Back
		</button>
		<span class="rounded-md bg-gray-200 px-4 py-2 shadow dark:bg-gray-800">
			Zoom: {Math.round(zoomLevel * 100)}%
		</span>
		<span class="rounded-md bg-gray-200 px-4 py-2 shadow dark:bg-gray-800">
			Path: {currentNode.path}
		</span>
	</div>

	<!-- Node details panel -->
	{#if selectedNode}
		<div
			class="absolute bottom-4 right-4 w-80 rounded-lg border border-gray-200 bg-white p-4 shadow-lg dark:border-gray-700 dark:bg-gray-800"
		>
			<div class="mb-3 flex items-center justify-between">
				<h3 class="text-lg font-bold">{selectedNode.data.name}</h3>
				<button
					class="text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-200"
					onclick={() => (selectedNode = null)}
				>
					✕
				</button>
			</div>
			<div class="space-y-3">
				<p class="text-sm text-gray-600 dark:text-gray-300">
					Path: {selectedNode.data.path}
				</p>
				<div>
					<h4 class="mb-1 font-medium">Goals:</h4>
					<div class="space-y-2">
						{#each selectedNode.data.goals as goal}
							<div
								class="flex items-center gap-2 rounded bg-gray-50 p-2 dark:bg-gray-700"
							>
								<div
									class={`h-3 w-3 rounded-full ${goal.state === 1 ? "bg-green-500" : "bg-gray-400"}`}
								></div>
								<span class="text-sm">{goal.name}</span>
							</div>
						{/each}
					</div>
				</div>
				{#if selectedNode.depth === 3 && selectedNode.data.children.length > 0}
					<button
						class="w-full rounded-md bg-amber-500 px-3 py-2 text-white shadow transition-colors hover:bg-amber-600"
						onclick={() => navigateToNode(selectedNode!.data)}
					>
						Continue to {selectedNode.data.children.length} child nodes
					</button>
				{/if}
			</div>
		</div>
	{/if}

	<!-- Instructions -->
	<div
		class="absolute bottom-4 left-4 rounded-md bg-gray-800 bg-opacity-75 p-3 text-sm text-white"
	>
		<p>🖱️ Click node to view details</p>
		<p>🎯 Click amber nodes to navigate deeper</p>
		<p>⌨️ Press ESC to go back</p>
	</div>
</div>

<style>
	/* Pulse animation for route nodes */
	.pulse-animation {
		animation: pulse 1.5s infinite;
	}

	@keyframes pulse {
		0% {
			transform: scale(0.95);
			box-shadow: 0 0 0 0 rgba(59, 130, 246, 0.7);
		}

		70% {
			transform: scale(1);
			box-shadow: 0 0 0 8px rgba(59, 130, 246, 0);
		}

		100% {
			transform: scale(0.95);
			box-shadow: 0 0 0 0 rgba(59, 130, 246, 0);
		}
	}
</style>
