<script lang="ts">
	import type { HTMLBaseAttributes } from "svelte/elements";

	export let radius: string | number = 20;
	interface $$Props extends HTMLBaseAttributes {
		radius?: string | number;
	}
</script>

<div {...$$restProps}>
	<svg viewBox="0 0 50 50">
		<circle class="path" cx="25" cy="25" r={radius} fill="none" stroke-width="5"
		></circle>
	</svg>
</div>

<style>
	div {
    display: block;
    z-index: 9999;
		animation: rotate 2s linear infinite;
		width: 42px;
		height: 42px;

		& .path {
			stroke: var(--primary);
			stroke-linecap: round;
			animation: dash 1.5s ease-in-out infinite;
		}
	}

	@keyframes rotate {
		100% {
			transform: rotate(360deg);
		}
	}

	@keyframes dash {
		0% {
			stroke-dasharray: 1, 150;
			stroke-dashoffset: 0;
		}
		50% {
			stroke-dasharray: 90, 150;
			stroke-dashoffset: -35;
		}
		100% {
			stroke-dasharray: 90, 150;
			stroke-dashoffset: -124;
		}
	}
</style>
