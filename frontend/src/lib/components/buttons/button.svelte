<script lang="ts">
	import type { HTMLButtonAttributes } from "svelte/elements";

	export let highlight: boolean = false;
	export let href: string | undefined = undefined;
	interface $$Props extends HTMLButtonAttributes {
		highlight?: boolean;
		href?: string;
	}
</script>

{#if href}
	<a class:highlight role="button" class="button" {href} {...$$restProps}>
		<slot />
	</a>
{:else}
	<button class:highlight class="button" type="button" on:click {...$$restProps}>
		<slot />
	</button>
{/if}

<style>
	.button {
		padding: 8px;
		background-color: var(--shade-01);
		cursor: pointer;
		border-radius: var(--br);
		transition: background-color var(--transition);
		box-shadow: var(--box-shadow);
		border: 1px solid var(--border-color);
		display: inline-flex;
		gap: 6px;
		justify-content: center;
		align-items: center;
		color: var(--foreground-color);

		/* TODO: Jesus christ... */
		&:is(a) {
			font-size: calc(var(--font-size) - 3px);
			text-decoration: none;
			line-height: 1.25;
			color: var(--fg);
		}

		&:disabled {
			opacity: 0.5;
			cursor: not-allowed;
		}

		&:not(:disabled)[type="submit"] {
			border-color: transparent;
			background-color: var(--primary);
			color: white;

			&:hover {
				background-color: var(--primary-dark);
			}
		}

		&:focus-visible {
			outline-offset: var(--outline-offset);
			outline: var(--outline);
		}

		&:hover:not(:disabled) {
			background-color: var(--shade-02);
		}

		&:active:not(:disabled) {
			background-color: var(--shade-03);
		}
	}

	.highlight {
		--rotation: -45deg;
		--color-a: #b63c8e;
		--color-b: var(--primary);
		--border-width: 1px;
		--bg-color: var(--shade-01);

		border: var(--border-width) solid transparent !important;
		background-clip: content-box, border-box;
		background-origin: border-box;
		background-image: linear-gradient(var(--bg-color), var(--bg-color)),
			linear-gradient(var(--rotation), var(--color-a), var(--color-b));
		box-shadow:
			var(--box-shadow),
			2px 512px var(--border-width) var(--bg-color) inset !important;
		transition:
			background-color var(--transition),
			box-shadow var(--transition);

		&:hover {
			--bg-color: var(--shade-02);
			box-shadow:
				var(--box-shadow),
				2px 512px var(--border-width) var(--bg-color) inset !important;
		}

		&:active {
			--bg-color: var(--shade-03);
			box-shadow:
				var(--box-shadow),
				2px 512px var(--border-width) var(--bg-color) inset !important;
		}
	}
</style>
