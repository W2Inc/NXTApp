<script lang="ts">
	import { Icon, MagnifyingGlass } from "svelte-hero-icons";
	import type { HTMLBaseAttributes } from "svelte/elements";
	import Card from "../cards/card.svelte";
	import Loading from "../misc/loading.svelte";

	export let toggle = false;
	export let open = false;
	interface $$Props extends HTMLBaseAttributes {
		nomarker?: boolean;
		toggle?: boolean;
	}

	let body: HTMLDivElement;
	let dropdown: HTMLDetailsElement;

	function onLeave() {
		open = toggle ? open : false;
	}

	function onOpen() {
		open = toggle ? open : true;
	}

	function windowClick({ target }: MouseEvent) {
		const node = target as Node;
		if (open && target && !dropdown.contains(node)) {
			open = false;
		}
	}


</script>

<!-- We lose focus on clicking anywhere but this -->
<svelte:window on:click={windowClick} />

<details bind:open bind:this={dropdown} {...$$restProps}>
	<summary class="center">
		<Icon src={MagnifyingGlass} size="16px" />
		<div class="container">
			<input
				on:input={onOpen}
				title="Search the world..."
				name="search"
				autocomplete="off"
				placeholder="Search"
			/>
		</div>
	</summary>
	<div class="body" bind:this={body}>
		<Card>
			<Loading />
		</Card>
	</div>
</details>

<style>
	div.body {
		position: absolute;
		padding: 8px 4px;
		z-index: 999999;
	}

	summary {
		border-radius: var(--br);
		border: 1px solid var(--border-color);
		padding: 4px 12px;
		min-width: 12rem;
		width: 15rem;
		flex: 1 1 auto;
		box-shadow: var(--box-shadow);
		background: var(--shade-01);
		cursor: pointer;
		pointer-events: none;
		&::marker {
			content: "";
		}
	}

	input {
		flex: 1;
		border: none;
		outline: none;
		background-color: var(--shade-01);
		width: 100%;
		height: 20px;
		color: var(--foreground-color);
		pointer-events: all;
	}

	div.container {
		flex: 1;
		position: relative;
	}
</style>
