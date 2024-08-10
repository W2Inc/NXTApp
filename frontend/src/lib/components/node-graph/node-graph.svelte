<script lang="ts">
	import { writable } from "svelte/store";
	import {
		SvelteFlow,
		Controls,
		Background,
		BackgroundVariant,
		NodeToolbar,
	} from "@xyflow/svelte";
	import "@xyflow/svelte/dist/style.css";
	import Card from "../cards/card.svelte";
	import { Cog, Icon } from "svelte-hero-icons";

	let showGraphContext = false;
	let showNodeContext = false;
	let clickPos: number[] = [0, 0];

	// We are using writables for the nodes and edges to sync them easily. When a user drags a node for example, Svelte Flow updates its position.
	const nodes = writable([
		{
			id: "1",
			type: "input",
			data: { label: "Input Node" },
			position: { x: 0, y: 0 },
		},
		{
			id: "2",
			type: "default",
			data: { label: "Node" },
			position: { x: 0, y: 150 },
		},
	]);

	// same for edges
	const edges = writable([
		// {
		// 	id: "1-2",
		// 	type: "default",
		// 	source: "1",
		// 	target: "2",
		// 	label: "Edge Text",
		// },
	]);

	const snapGrid = [25, 25];
</script>

<!--
👇 By default, the Svelte Flow container has a height of 100%.
This means that the parent container needs a height to render the flow.
-->

{#if showGraphContext}
	<div class="context-menu" style="top:{clickPos[1]}px; left:{clickPos[0]}px;">
		<menu>
			<li>
				<Icon src={Cog} size="24px" />
				<button on:click={() => {}}>Copy</button>
			</li>
			<li>
				<Icon src={Cog} size="24px" />
				<button on:click={() => {}}>Cut</button>
			</li>
			<li>
				<Icon src={Cog} size="24px" />
				<button on:click={() => {}}>Paste</button>
			</li>
		</menu>
	</div>
{/if}

{#if showNodeContext}
	<div class="context-menu" style="top:{clickPos[1]}px; left:{clickPos[0]}px;">
		<menu>
			<li>
				<Icon src={Cog} size="24px" />
				<button on:click={() => {}}>Copy</button>
			</li>
			<li>
				<Icon src={Cog} size="24px" />
				<button on:click={() => {}}>Cut</button>
			</li>
			<li>
				<Icon src={Cog} size="24px" />
				<button on:click={() => {}}>Paste</button>
			</li>
		</menu>
	</div>
{/if}

<svelte:window on:click|stopPropagation={() => { showGraphContext = false; showNodeContext = false; }} />

<div
	role="none"
	style:height="500px"
	on:contextmenu|preventDefault={(e) => {
		console.log("Graph Context", e);
		clickPos = [e.clientX, e.clientY];
		showGraphContext = true;
		showNodeContext = false;
		e.preventDefault();
	}}
>
	<SvelteFlow
		{nodes}
		{edges}
		fitView
		colorMode="system"
		on:nodecontextmenu={(e) => {
			console.log("Node Context", e.detail.node);

			e.detail.event.stopPropagation();
			e.detail.event.preventDefault();
			showGraphContext = false;
			showNodeContext = true;
		}}
		on:nodeclick={(event) => console.log("on node click", event.detail.node)}
	>
		<Controls />
		<NodeToolbar />
		<Background variant={BackgroundVariant.Dots} />
		<!-- <MiniMap /> -->
	</SvelteFlow>
</div>

<style>
	.context-menu {
		top: 0;
		left: 0;
		position: absolute;
		width: 192px;
		background: var(--shade-01);
		border: 1px solid var(--border-color);
		border-radius: var(--br);
		font-size: 0.9rem;
		z-index: 9999;
		padding: 8px;
		border-radius: 8px;

		& menu {
			& button {
				appearance: none;
				border: none;
				/* background: none; */
				width: 100%;
			}

			& li {
				display: flex;
				align-items: center;
				gap: 4px;
				width: 100%;
				padding: 4px;
				border-radius: var(--br);
				color: var(--text-01);
				margin-bottom: 4px;
				text-decoration: none;
				cursor: pointer;

				&:hover {
					color: var(--text-01);
					background: var(--shade-03);
				}
			}
		}
	}
</style>
