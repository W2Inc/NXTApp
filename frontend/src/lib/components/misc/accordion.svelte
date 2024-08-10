<script lang="ts">
	import { ChevronDown, ChevronRight, Icon } from "svelte-hero-icons";
	import { quintOut } from "svelte/easing";
	import type { HTMLAttributes, HTMLDetailsAttributes } from "svelte/elements";
	import { fly, slide } from "svelte/transition";

	export let open = false;
	interface $$Props extends HTMLDetailsAttributes {
		open?: boolean;
	}
</script>

<div>
	<details bind:open {...$$restProps}>
		<summary class="center">
			<span class:down={open}>
				<Icon src={ChevronRight} size="16px" />
			</span>
			{$$props.title}
		</summary>
		{#if open}
			<div
				transition:fly={{
					y: -10,
					opacity: 0.5,
				}}
			>
				<slot />
			</div>
		{/if}
	</details>
</div>

<style>
	details summary::-webkit-details-marker {
		display: none;
	}

	summary {
		cursor: pointer;
		user-select: none;

		& span {
			display: inline-flex;
			transform: rotate(0deg);
			transform-origin: center;
			transition: transform 0.1s ease-in-out;

			&.down {
				transform: rotate(90deg);
			}
		}
	}
</style>
