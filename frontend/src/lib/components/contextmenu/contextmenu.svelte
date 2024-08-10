<script lang="ts">
	import { fade } from "svelte/transition";

	export let x = 0;
	export let y = 0;
	export let show = false;

</script>

{#if show}
	<menu style="display: {show ? "block" : "none"}; top: {y}px; left: {x}px;">
		<slot />
	</menu>
{/if}

<style>
	menu {
		position: absolute;
		z-index: 99999;
		background-color: var(--shade-01);
		box-shadow: var(--box-shadow);
		border-radius: var(--br);
		padding: 4px;
		list-style: none;
	}
</style>
