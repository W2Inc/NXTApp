<script lang="ts">
	import type { HTMLTextareaAttributes } from "svelte/elements";

	export let value: string = "";
	export let vertical: boolean = false;
	export let horizontal: boolean = false;
	interface $$Props extends HTMLTextareaAttributes {
		value?: string;
		vertical?: boolean;
		horizontal?: boolean;
	}
</script>

<textarea class:horizontal class:vertical bind:value {...$$restProps} />

<style>
	.vertical {
		resize: vertical;
	}

	.horizontal {
		resize: horizontal;
	}

	textarea {
		display: block;
		border-radius: var(--br);
		border: 1px solid var(--border-color);
		background-color: var(--shade-01);
		color: var(--fg);
		padding: 8px;
		outline: none;
		box-shadow: var(--box-shadow);
		width: 100%;
		min-height: 128px;

		&:focus-visible {
			border-radius: var(--br);
			outline-offset: var(--outline-offset);
			outline: var(--outline);
		}
	}
</style>
