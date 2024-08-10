<script lang="ts">
	import { Icon } from "svelte-hero-icons";
	import type { Iconlink } from "$lib/types";

	export let title: string | undefined = undefined;
	export let navs: Iconlink[];
</script>

{#if title}
	<p>{title}</p>
	<hr />
{/if}

<menu>
	{#each navs as nav}
		<li>
			<a href={nav.link.href}>
				<Icon src={nav.icon} size="22px" solid/>
				{nav.link.title}
			</a>
		</li>
	{/each}
</menu>

<style>
	p {
		font-size: 14px;
		font-weight: 600;
		margin-top: 16px;
	}

	li {
		list-style: none;
		border-radius: var(--br);
		transition: background-color var(--transition);
		display: flex;
		justify-content: space-around;
		align-items: center;
		cursor: pointer;

		&:focus-within {
			outline-offset: var(--outline-offset);
			outline: var(--outline);
		}

		& a {
			padding: 8px 8px 8px 4px;
			flex: 1;
			display: flex;
			align-items: center;
			gap: 8px;
			font-size: 16px;
			text-decoration: none;
			color: var(--text-1);
			border-radius: var(--br);
			outline: none;
		}

		&::before {
			content: "";
			display: block;
			position: relative;
			left: -2px;
			width: 6px;
			height: 20px;
			border-radius: var(--br);
			background-color: var(--primary);
			opacity: 0;
			transition: opacity var(--transition);
		}

		&:hover::before {
			opacity: 1;
		}

		&:hover {
			background-color: var(--shade-02);
		}
	}
</style>
