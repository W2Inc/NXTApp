<script lang="ts">
	import { LoaderCircle } from "lucide-svelte";

	const { data }: PageProps = $props();
	let search = $state("");
	let cursus = $state("");
	let promise = $state<Promise<unknown>>();

	// Audio state and Web Audio API setup
	import type { PageProps } from "./$types";
	import Menu from "./menu.svelte";
	import D3 from "./d3.svelte";
	import { generateSampleData } from "./d3utils.svelte";
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
		onCursusChange={(v) => {}}
		cursi={["42", "c-piscine"]}
	/>

	<!-- {#await Promise.resolve() then _} -->
		<D3 data={generateSampleData()} />
	<!-- {/await} -->
</div>

<style>
	.canvas {
		width: 100%;
		height: calc(100dvh);
		display: block;
	}
</style>
