<script lang="ts">
	import Self from "./explorer.svelte";
	import { useTreeState, type TreeItem } from "./context.svelte";

	interface Props {
		item: TreeItem;
	}
	const { item }: Props = $props();
	const { dropHandler, dragoverHandler, dragstartHandler } = useTreeState();
</script>

<details
	open
	id="hierarchy"
	ondragover={dragoverHandler}
	ondrop={(ev) => dropHandler(ev, item)}
>
	<summary
		id="node"
		class="text-foreground"
		draggable={item.type !== "root"}
		ondragstart={(ev) => dragstartHandler(ev, item.id)}
	>
		<span id="icon">🔖</span>
		<span id="label">{item?.name ?? "none"}</span>
		<!-- <input id="color" type="color" value="#ff0000" /> -->
	</summary>

	{#if item && item.children && item.children.length >= 1}
		<div id="body">
			<div id="bar">
				<div id="line"></div>
			</div>

			<div class="end contents">
				<div></div>
				<div class="h-4"></div>
			</div>

			<div class="contents">
				{#each item.children as child, i}
					<div
						aria-hidden="true"
						class="threadline"
						class:end={item.children.length <= 1 || i == item.children.length - 1}
					>
						<div class="branch-line"></div>
					</div>
					<Self item={child} />
				{/each}
			</div>
		</div>
	{/if}
</details>

<style>
	.end {
		background-color: hsl(var(--background));
	}
	.hover {
		border-style: dotted;
	}
	#node {
		display: inline-flex;
		padding: 0.25rem 0.5rem;
		/* border-radius: 0.5rem; */
		color: black;
		align-items: center;
		gap: 0.25rem;
		/* background: white; */
		user-select: none;
		border: 1px solid gray;
	}

	.contents {
		padding-left: 1.75rem;
		line-height: 1.0625rem;
	}
	#body {
		display: grid;
		position: relative;
		grid-template-columns: 24px 1fr;
	}
	#bar {
		position: absolute;
		top: 0;
		left: 0;
		bottom: 0;
		width: 1.5rem;
		display: flex;
		justify-items: center;
		justify-content: center;
		cursor: pointer;
		& #line {
			background-color: gray;
			width: 2px;
			height: 100%;
		}
	}
	.threadline {
		display: flex;
		justify-content: end;
		position: relative;
	}
	.branch-line {
		border-style: solid;
		border-left-width: 2px;
		border-bottom-width: 2px;
		width: calc(50% + 1px);
		height: 1rem;
		/* border-bottom-left-radius: 8px; */
		border-color: gray;
	}
</style>
