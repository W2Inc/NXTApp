<script lang="ts">
	import { createEventDispatcher } from "svelte";
	import type { HTMLButtonAttributes } from "svelte/elements";

	const dispatch = createEventDispatcher<{
		"click": { pressed: boolean };
	}>();

	// Props
	export let toggled: boolean = false;
	interface $$Props extends HTMLButtonAttributes {
		toggled?: boolean;
	}

	// Functions
	function onClick() {
		toggled = !toggled;
		dispatch("click", { pressed: toggled });
	}
</script>

<button
	on:click={onClick}
	aria-pressed={toggled ? "true" : "false"}
	{...$$restProps}
/>

<style>
	button {
		--br: 10px;
		--size: 1.25em;
		--bg: var(--shade-05);
		--fg: rgba(255, 255, 255, 0.906);
		--bg-active: var(--bg);
		--fg-active: var(--fg);

		position: relative;
		height: var(--size);
		width: 34px;
		/* TODO: Fix this later */
		/* min-width: calc(100% - 0.6em);
		max-width: calc(2 * var(--size)); */
		border-radius: var(--br);
		appearance: none;
		outline-offset: 2px;
		border: transparent;
		cursor: pointer;
	}

	button:focus-visible {
		outline: var(--outline);
	}

	button::before {
		content: "";
		position: absolute;
		display: block;
		height: 100%;
		width: 100%;
		padding: 2px;
		border-radius: var(--size);
		top: 0;
		left: 0;
		border-radius: var(--br);
		background: var(--bg);
		box-sizing: border-box;
		transition: background 0.2s ease-out;
		box-shadow: var(--box-shadow);
	}

	button:disabled {
		cursor: not-allowed;
		background: var(--shade-02);
	}

	button[aria-pressed="true"]::before {
		background: var(--primary);
	}

	button::after {
		content: "";
		position: absolute;
		display: block;
		width: calc(var(--size) - 4px);
		height: calc(var(--size) - 4px);
		aspect-ratio: 1;
		top: 1.5px;
		left: 2px;
		border-radius: 50%;
		background: var(--fg);
		transition: background 0.2s ease-out, left 0.2s ease-out;
	}

	button[aria-pressed="true"]::after {
		background: var(--fg-active);
		left: calc(100% - var(--size) + 2px);
	}
</style>
