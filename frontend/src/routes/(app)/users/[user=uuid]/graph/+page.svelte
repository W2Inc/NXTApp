<script lang="ts">
	import { page } from "$app/stores";
	import Input from "$lib/components/input/input.svelte";
	import Select from "$lib/components/input/select.svelte";
	import type { ApiGraph, ClientGraph, Goal, Node } from "$lib/graph/builder";
	import transformAPIData from "$lib/graph/builder";
	import { useStorage } from "$lib/stores/storage";
	import { AudioListener, Environment, Gizmo, OrbitControls, useProgress } from "@threlte/extras";
	import { onDestroy, onMount } from "svelte";
	import { tweened } from "svelte/motion";
	import { Matrix4, type PerspectiveCamera } from "three";
	import type { PageServerData } from "./$types";
	import Button from "$lib/components/buttons/button.svelte";
	import { selectedGoal } from "$lib/stores/node-store";
	import Link from "$lib/components/links/link.svelte";
	import { Canvas, T } from "@threlte/core";
	import Graph from "$lib/graph/graph.svelte";
	import Galaxy from "$lib/galaxy/galaxy.svelte";
	import Bloom from "$lib/galaxy/effects/bloom.svelte";
	import { ArrowsPointingOut, Icon } from "svelte-hero-icons";
	import { cdnURL } from "$lib/api/api";

	// Params

	export let width = 0;
	export let height = 0;
	export let data: PageServerData;

	// Variables

	let body: HTMLBodyElement | null;
	let orbit: OrbitControls;
	let camera: PerspectiveCamera;
	let resizeCanvas: VoidFunction;
	const { progress } = useProgress();
	const graphData = useStorage<ClientGraph>("graphData", {
		seed: "undefined",
		nodes: [],
		links: [],
	});

	// const tweenedProgress = tweened($progress, {
	// 	duration: $graphData.links.length > 0 ? 0 : 400,
	// });

	// HACK: TS Can't infer the type for some dumb reason
	let goals: Node<Goal>[];
	$: goals = $graphData.nodes.filter((node) => node.type === "goal") as Node<Goal>[];

	// Functions

	onMount(() => {
		let headerOffset = 0;
		const header = document.querySelector("header");
		if (header) headerOffset = header.clientHeight;
		resizeCanvas = () => {
			width = window.innerWidth;
			height = window.innerHeight - headerOffset + 1;
		}

		body = document.querySelector("body");
		if (body) body.style.overflow = "hidden";

		resizeCanvas();

		const urlCursus = $page.url.searchParams.get("cursus");
		if (urlCursus) setCursus(urlCursus);
	});

	onDestroy(() => {
		if (body) body.style.overflow = "auto";
	});

	/** Reset the "camera" to the default position */
	function resetCamera() {
		camera.applyMatrix4(new Matrix4());
		camera.matrixWorldNeedsUpdate = true;
	}

	/** Set the cursus to display */
	async function setCursus(cursus: string) {
		const response = await fetch(`./graph?cursus=${cursus}`);
		const data = (await response.json()) as ApiGraph;
		graphData.set(transformAPIData(data));
		selectedGoal.set(null);

		// Update the URL to add the cursus in the query
		const url = new URL($page.url.toString());
		url.searchParams.set("cursus", cursus);
		history.replaceState(null, "", url.toString());
	}

	/** Search for a goal by name */
	function searchForGoal(e: Event) {
		if (e instanceof KeyboardEvent && e.key !== "Enter") return;

		const input = e.target as HTMLInputElement;
		const value = input.value;
		if (value === "") return;

		const goal = $graphData.nodes.find((node) => {
			if (node.type === "goal") return node.meta.name === value;
			return false;
		});

		if (goal) selectedGoal.set(goal as Node<Goal>);
	}
</script>

<svelte:window on:resize={() => resizeCanvas()} />

<aside>
	<div class="selection-search">
		<Select name="cursus" on:selection={(e) => setCursus(e.detail.option)}>
			{#each data.cursi as cursus, i}
				<option selected={i === 0} value={cursus}>{cursus}</option>
			{/each}
		</Select>
		<Input
			type="text"
			placeholder="Search"
			autocomplete="off"
			autocorrect="off"
			autocapitalize="off"
			spellcheck="false"
			list="search-goals-:graph:"
			on:keydown={searchForGoal}
			on:input={searchForGoal}
		/>
		<Button type="button" on:click={resetCamera}>
			<Icon src={ArrowsPointingOut} size="20px" />
		</Button>

		<!-- <Button type="button" on:click={() => ambience.toggle()}>
			<Icon src={isAudioPlaying ? SpeakerWave : SpeakerXMark} size="20" />
		</Button> -->
	</div>

	<!-- TODO: Figure out what to display -->
	{#if $selectedGoal}
		{@const friendlyState = $selectedGoal.meta.state.replaceAll("_", " ")}
		{@const href = `/${$page.params.user}/projects/${$selectedGoal.meta.slug}`}

		<div id="graph" class="description">
			<div class="header" data-project-state={$selectedGoal.meta.state}>
				<Link {href}>{$selectedGoal.meta.name}</Link>
				<strong>{friendlyState}</strong>
			</div>
			<hr />
			<p>{$selectedGoal.meta.state}</p>
		</div>
	{/if}
	<datalist id="search-goals-:graph:">
		{#each goals as goal}
			<option value={goal.meta.name}>{goal.meta.name}</option>
		{/each}
	</datalist>
</aside>

<!--<div class="credits">
		Music made with ❤️ by <Link href="https://freekb.es/monospace/">Monospace</Link>
	</div>-->

<div style="position: relative; overflow: hidden;">
	<Canvas size={{ width, height }}>
		<T.PerspectiveCamera
			makeDefault
			bind:ref={camera}
			position={[0, 750, -1000]}
			on:create={({ ref }) => ref.lookAt(0, 0, 0)}
		>
			<OrbitControls
				autoRotate
				enableDamping
				zoom={1.0}
				maxDistance={1000}
				autoRotateSpeed={Math.PI / 128}
				bind:ref={orbit}
			/>
			<AudioListener />
		</T.PerspectiveCamera>

		<Graph graphData={$graphData} />
		<Galaxy />
		<Bloom />
		<Environment files={`${cdnURL}/envmap/hdr.png`} isBackground={true} />
		<!-- <Ambience bind:this={ambience} bind:isPlaying={isAudioPlaying} /> -->

		<T.AxesHelper args={[5]} renderOrder={1} />
		{#if width > 768}
			<Gizmo paddingX={20} paddingY={20} size={96} />
		{/if}
	</Canvas>
</div>

<style>
	/* div.credits {
		position: absolute;
		bottom: 0;
		left: 0;
		padding: 8px;
		font-size: 0.75rem;
		line-height: 1rem;
		color: hsla(0, 0%, 65%, 1);

		@media screen and (max-width: 700px) {
			display: none;
		}
	} */

	/* div.wrapper {
		position: absolute;
		width: 100%;
		height: 100%;
		top: 0;
		left: 0;
		display: flex;
		flex-direction: column;
		gap: 0.25rem;
		align-items: center;
		justify-content: center;
		color: var(--primary);
	} */

	aside {
		--spacing: 10px;

		width: 100%;
		position: absolute;
		left: 0;
		padding: 8px;

		display: grid;
		gap: var(--spacing);
		grid-template-columns: auto auto;

		justify-content: space-between;
		border-bottom-right-radius: var(--br);
		pointer-events: none;

		z-index: 1000;

		@media (max-width: 700px) {
			grid-template-columns: auto;
			grid-template-rows: auto auto;
		}

		& div.selection-search {
			display: flex;
			height: min-content;
			gap: var(--spacing);
			pointer-events: all;
		}
	}

	#graph.description {
		backdrop-filter: blur(5px);

		width: 100%;
		max-width: 300px;
		min-width: 200px;
		min-height: 100px;

		color: white;
		border: solid 1px var(--border-color);

		background-color: hsla(0, 0%, 9%, 0.5);
		backdrop-filter: blur(4px);
		border-radius: var(--br);
		padding: 20px;

		pointer-events: all;

		& p {
			font-size: 0.875rem;
			line-height: 1.25rem;
			color: hsla(0, 0%, 65%, 1);
		}
	}

	#graph .header {
		display: flex;
		color: white;
		gap: var(--spacing);
		--color: var(--primary);
		justify-content: space-between;

		&[data-project-state="available"] {
			color: black;
			--color: rgb(223, 223, 223);
		}

		&[data-project-state="validated"] {
			--color: rgb(11, 150, 57);
		}

		&[data-project-state="in_progress"] {
			--color: rgb(185, 111, 14);
		}

		&[data-project-state="unavailable"] {
			--color: hsla(0, 0%, 25%, 1);
		}

		& strong {
			font-size: 0.8em;
			border-radius: 4px;
			padding-inline: 4px;
			text-transform: capitalize;
			background-color: var(--color);
		}
	}

	/* .loading {
		font-size: 0.875rem;
		line-height: 1.25rem;
	}

	.bar-wrapper {
		width: 33.333333%;
		height: 10px;
		border: 1px solid var(--primary);
		position: relative;
	}

	.bar {
		height: 100%;
		background-color: var(--primary);
	} */
</style>
