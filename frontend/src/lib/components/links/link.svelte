<script lang="ts">
	import type { HTMLBaseAttributes } from "svelte/elements";
	import { Icon, ArrowTopRightOnSquare } from "svelte-hero-icons";

	export let href = "#";
	interface $$Props extends HTMLBaseAttributes {
		href?: string;
	}
</script>

<a {href} {...$$restProps}>
	<slot />
	<Icon src={ArrowTopRightOnSquare} size="16px" />
</a>

<style>
	a {
		display: inline-flex;
		align-items: center;
		justify-items: center;
		gap: 4px;
		color: var(--primary);

		&:focus-visible {
			outline-offset: var(--outline-offset);
			outline: var(--outline);
			border-radius: var(--br);
		}
	}
</style>
