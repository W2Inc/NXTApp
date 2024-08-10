<script lang="ts">
	import { Icon, type IconSource } from "svelte-hero-icons";

	export let depth = 0;
	export let basename = "";
	export let icon: IconSource;
	export let selected = false;
</script>

<li
	role="treeitem"
	style="--depth: {depth};"
	aria-selected={selected ? "true" : undefined}
	on:keydown
>
	<button class="basename" on:click>
		<Icon src={icon} size="14px" />
		{basename}
	</button>
</li>

<style>
	/** TODO: Make this a component to avoid duplicated CSS*/
	li {
		--inset: calc((var(--depth) * 1.2rem) + 4px);

		border-radius: var(--br);
		transition: background-color var(--transition);
		display: flex;
		justify-content: space-around;
		align-items: center;
		cursor: pointer;
		height: 2rem;
		color: var(--text-1);

		/*&::before {
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
		}*/

		&:focus-within {
			outline: var(--outline);
			outline-offset: -2px;
			border-radius: var(--br);
		}

		/*&:hover::before {
			opacity: 1;
		}*/

		&:hover {
			background-color: var(--shade-02);
		}
	}

	button {
		background-size: 1.2rem 1.2rem;
		background-position: 0 45%;
		background-repeat: no-repeat;
		background-color: transparent;
		outline: none;
	}

	.basename {
		display: block;
		position: relative;
		margin: 0;
		padding: 0 1rem 0 calc(8px + var(--inset));
		font-size: 14px;
		font-family: inherit;
		color: inherit;
		flex: 1;
		height: 100%;
		text-align: left;
		border: 2px solid transparent;
		white-space: nowrap;
		overflow: hidden;
		line-height: 1;

		&:focus-visible {
			outline: none;
		}
	}

	[aria-selected="true"] {
		color: var(--primary);
	}

	/*[aria-selected="true"]::after {
		content: "";
		position: absolute;
		width: 1rem;
		height: 1rem;
		top: 0.3rem;
		right: calc(-0.6rem - 2px);
		background-color: var(--shade-03);
		border: 1px solid var(--shade-04);
		transform: translate(0, 0.2rem) rotate(45deg);
		z-index: 2;
	}*/
</style>
