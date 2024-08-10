<script lang="ts">
	import { createEventDispatcher } from "svelte";
	import { Icon, EyeSlash, Eye } from "svelte-hero-icons";
	import { scale } from "svelte/transition";

	const dispatch = createEventDispatcher();

	let toggle = true;
	function togglePassword() {
		toggle = !toggle;
		dispatch("toggle", {
			hidden: toggle,
		});
	}
</script>

<button type="button" on:click={togglePassword}>
	{#if toggle}
		<div transition:scale>
			<Icon src={Eye} size="16px" />
		</div>
	{:else}
		<div transition:scale>
			<Icon src={EyeSlash} size="16px" />
		</div>
	{/if}
</button>

<style>
	button {
		display: flex;

		border: none;
		border-radius: var(--br);
		border-top-left-radius: 0 !important;
		border-bottom-left-radius: 0 !important;
		border-left: 1px solid var(--shade-03);

		width: 34px;
		padding: 8px;
		outline: none;
		cursor: pointer;
		background-color: var(--shade-01);

		& > div {
			position: absolute;
			display: flex;
		}

		&:focus-visible {
			outline: var(--outline);
		}

		&:hover {
			background-color: var(--primary);
		}
	}
</style>
