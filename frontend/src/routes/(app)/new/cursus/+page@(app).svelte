<script lang="ts">
	import CardNotice from "$lib/components/cards/card-notice.svelte";
	import { writable } from "svelte/store";
	import {
		SvelteFlow,
		Controls,
		Background,
		BackgroundVariant,
		NodeToolbar,
		ControlButton,
	} from "@xyflow/svelte";
	import "@xyflow/svelte/dist/style.css";
	import {
		ArrowLeft,
		ArrowRight,
		Check,
		CheckCircle,
		Cog,
		DocumentArrowDown,
		DocumentArrowUp,
		Icon,
		PlusCircle,
	} from "svelte-hero-icons";
	import { onMount } from "svelte";
	import Loading from "$lib/components/misc/loading.svelte";
	import Contextmenu from "$lib/components/contextmenu/contextmenu.svelte";
	import MenuItem from "$lib/components/contextmenu/menu-item.svelte";
	import Button from "$lib/components/buttons/button.svelte";
	import Input from "$lib/components/input/input.svelte";
	import IconFloppy from "$lib/icons/icon-floppy.svelte";

	export let width = 200;
	export let height = 200;
	export let isLoaded = false;

	let contextX = 0;
	let contextY = 0;
	let contextShow = false;
	let resizeCanvas: VoidFunction;

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

	onMount(() => {
		let headerOffset = 0;
		const header = document.querySelector("header");
		if (header) headerOffset = header.getBoundingClientRect().height;
		resizeCanvas = () => {
			width = window.innerWidth;
			height = window.innerHeight - headerOffset;
		};

		resizeCanvas();
		isLoaded = true;
	});
</script>

<svelte:window
	on:click={(e) => {
		if (contextShow) {
			contextShow = false;
		}
	}}
	on:resize={() => resizeCanvas()}
/>

<Contextmenu bind:show={contextShow} bind:x={contextX} bind:y={contextY}>
	<MenuItem on:click={() => {}}>
		<Icon src={DocumentArrowUp} size="24px" />
		Copy
	</MenuItem>
	<MenuItem on:click={() => {}}>
		<Icon src={DocumentArrowDown} size="24px" />
		Cut
	</MenuItem>
	<MenuItem on:click={() => {}}>
		<Icon src={Cog} size="24px" />
		Paste
	</MenuItem>
</Contextmenu>

<aside>
	<div class="selection-search">
		<Input
			type="text"
			placeholder="Search"
			autocomplete="off"
			autocorrect="off"
			autocapitalize="off"
			spellcheck="false"
			list="search-goals-:graph:"
		/>
		<Button type="button">
			<IconFloppy />
		</Button>
	</div>
</aside>

{#if !isLoaded}
	<Loading />
{:else}
	<div
		role="none"
		style="width: {width}px; height: {height}px;"
		on:contextmenu|preventDefault={(e) => {
			contextShow = true;
			contextX = e.clientX;
			contextY = e.clientY;
		}}
	>
		<SvelteFlow
			{nodes}
			{edges}
			fitView
			onlyRenderVisibleElements={true}
			colorMode="system"
			on:nodecontextmenu={(e) => {}}
			on:nodedragstart={(e) => (contextShow = false)}
			on:nodeclick={(event) => console.log("on node click", event.detail.node)}
		>
			<Controls />
			<NodeToolbar />
			<Background variant={BackgroundVariant.Dots} />
			<!-- <MiniMap /> -->
		</SvelteFlow>
	</div>
{/if}

<style>
	aside {
		width: 100%;
		position: absolute;
		left: 0;
		padding: 8px;

		pointer-events: none;
		z-index: 1000;

		& div.selection-search {
			display: flex;
			height: min-content;
			gap: 8px;
			pointer-events: all;
		}
	}
</style>
