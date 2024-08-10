<script lang="ts">
	import type { HTMLBaseAttributes } from "svelte/elements";

	export let hightlight = false;
	interface $$Props extends HTMLBaseAttributes {
		hightlight?: boolean;
	}
</script>

<div class:highlight={hightlight} {...$$restProps}>
	<slot />
</div>

<style>
	div {
		background-repeat: no-repeat;
		background-size: cover;
		--glow-gradient: radial-gradient(
				ellipse at 40% 0%,
				#bf398910 0,
				transparent 75%
			),
			radial-gradient(ellipse at 60% 0%, #096bde10 0, transparent 75%);

		border-radius: var(--br);
		box-shadow: var(--box-shadow);
		border: 1px solid var(--border-color);
		padding: 16px;
		background-color: var(--shade-01);

		&.highlight {
			background-image: var(--glow-gradient);
		}
	}
</style>
