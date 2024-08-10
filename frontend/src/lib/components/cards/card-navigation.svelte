<script lang="ts">
	import { Icon, type IconSource } from "svelte-hero-icons";
	import type { HTMLAnchorAttributes } from "svelte/elements";

	export let icon: IconSource;
	export let description: string;
	interface $$Props extends HTMLAnchorAttributes {
		icon: IconSource;
		description: string;
	}

</script>

<!-- svelte-ignore a11y-missing-attribute -->
<a {...$$restProps}>
	<div class="center">
		<Icon src={icon} size="20" solid />
		<h1>{$$restProps.title}</h1>
	</div>
	<p>{description}</p>
</a>

<style>
	a {
		display: flex;
		flex-direction: column;
		justify-content: center;
		text-decoration: none;
		color: var(--text-01);
		transition: all 0.2s ease-in-out;
		border-radius: var(--br);
		padding: 12px;

		& h1 {
			font-size: 0.85rem;
			margin: 0;
			padding: 0;
		}

		& p {
			font-size: 0.75rem;
			color: var(--text-01);
			margin: 0;
			max-width: 256px;
			overflow: hidden;
			text-overflow: ellipsis;
			white-space: nowrap;
		}

		&:hover {
			box-shadow: var(--box-shadow);
			background-color: var(--shade-02);
		}

		&:active {
			background-color: var(--shade-03);
		}
	}
</style>
