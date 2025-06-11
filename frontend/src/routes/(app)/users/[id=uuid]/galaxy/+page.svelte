<script lang="ts">
	import { T, Canvas } from "@threlte/core";
	import { OrbitControls } from "@threlte/extras";
	import * as THREE from "three";

	// Import Shadcn components
	// import * as Select from "$lib/components/ui/select";
	import * as Tabs from "$lib/components/ui/tabs";
	import { Input } from "$lib/components/ui/input";
	import { Search, Volume2, VolumeX, LoaderCircle } from "lucide-svelte";
	import { Button } from "$lib/components/ui/button";

	const { data }: PageProps = $props();
	let search = $state("");
	let cursus = $state("");
	let promise = $state<Promise<unknown>>();

	// Audio state and Web Audio API setup
	import { Tween } from "svelte/motion";
	import type { PageProps } from "./$types";
	import Menu from "./menu.svelte";
	import { invalidateAll } from "$app/navigation";
	import { setContext } from "svelte";
	import { transformXGraphV1Data, type ClientGraph } from "$lib/graph";
	import { read } from "$app/server";
	import D3 from "./d3.svelte";

	let ctx = $state<GalaxyCTX>({
		mode: "3d",
		audioPlaying: false,
	});

	setContext("audio", ctx);

	let graph = $state.raw<ClientGraph>();
</script>

<svelte:head>
	<title>Learning Goals Galaxy</title>
</svelte:head>

{#snippet loader(text: string)}
	<div class="grid h-dvh place-content-center">
		<div class="flex flex-col items-center gap-2">
			<LoaderCircle size="64" class="text-primary animate-spin" />
			<span class="font-bold">{text}</span>
		</div>
	</div>
{/snippet}

<div class="flex h-screen flex-col">
	<Menu
		bind:search
		bind:cursus
		onCursusChange={(v) => (promise = invalidateAll())}
		cursi={["42", "c-piscine"]}
	/>

	<D3 />
</div>

<style>
	.canvas {
		width: 100%;
		height: calc(100dvh);
		display: block;
	}
</style>
