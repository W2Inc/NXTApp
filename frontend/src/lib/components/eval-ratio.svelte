<script lang="ts">
	import type { HTMLAttributes } from "svelte/elements";
	import Tippy from "./tippy.svelte";

	interface Props extends HTMLAttributes<HTMLDivElement> {
		self?: number;
		auto?: number;
		peer?: number;
		async?: number;
	}

	const { self = 25, auto = 25, peer = 10, async = 0, ...rest }: Props = $props();

	let total = $derived(self + auto + async + peer);
	let selfPercentage = $derived(((self / total) * 100).toFixed(1));
	let autoPercentage = $derived(((auto / total) * 100).toFixed(1));
	let peerPercentage = $derived(((peer / total) * 100).toFixed(1));
	let asyncPercentage = $derived(((async / total) * 100).toFixed(1));
</script>

{#snippet value(name: string, percentage: string)}
	<li class="cursor-pointer text-center text-xs" style="width: {percentage}%;">
		<Tippy text={name}>
			<strong>{percentage}%</strong>
		</Tippy>
	</li>
{/snippet}

<div class="mb-2">
	<!-- Tooltips -->
	<ul class="m-0 flex list-none p-0">
		{#if self > 0}
			{@render value("Self Evaluations", selfPercentage)}
		{/if}
		{#if auto > 0}
			{@render value("Automatic Evaluations", autoPercentage)}
		{/if}
		{#if async > 0}
			{@render value("Remote Evaluations", asyncPercentage)}
		{/if}
		{#if peer > 0}
			{@render value("Peer Evaluations", peerPercentage)}
		{/if}
	</ul>
	<!-- Bar -->
	<div
		role="status"
		class="swatch relative h-2 rounded shadow hover:cursor-pointer"
		style=" --self: {selfPercentage}%; --tester: {autoPercentage}%; --remote: {peerPercentage}%; --onsite: {asyncPercentage}%;"
		{...rest}
	></div>
</div>

<style>
	div.swatch {
		--self-color: #006266;
		--tester-color: #ffc312;
		--remote-color: #ee5a24;
		--onsite-color: #b53471;
		--background-color: #f0f0f0;

		--start: calc(var(--self) + var(--tester));
		--end: calc(var(--start) + var(--remote));
		background-image: linear-gradient(
			90deg,
			var(--self-color) 0%,
			var(--self-color) var(--self),
			var(--tester-color) var(--self),
			var(--tester-color) var(--start),
			var(--remote-color) var(--start),
			var(--remote-color) var(--end),
			var(--onsite-color) var(--end),
			var(--onsite-color) calc(var(--end) + var(--onsite)),
			var(--background-color) calc(var(--end) + var(--onsite)),
			var(--background-color) 100%
		);
	}
</style>
