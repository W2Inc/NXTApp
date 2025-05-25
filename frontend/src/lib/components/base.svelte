<script lang="ts">
	import type { Snippet } from "svelte";
	import * as Resizable from "$lib/components/ui/resizable";

	interface Props {
		left: Snippet;
		right: Snippet;
		variant?: "navbar" | "center-navbar" | "splitpane" | "center";
	}

	const { left, right, variant = "navbar" }: Props = $props();
</script>

<!-- A sticky / fixed navbar to the side with the content taking the main chunk of page -->
{#if variant === "navbar"}
	<div
		class="grid grid-rows-[auto_1fr] lg:grid-cols-[minmax(200px,300px)_1fr] lg:grid-rows-1"
	>
		<aside class="dark:bg-card sticky top-0 flex flex-col gap-2 p-4 lg:h-dvh lg:border-r">
			{@render left()}
		</aside>

		<!-- Body -->
		{@render right()}
	</div>
	<!-- A center-navbar is where both navbar and center content are center aligned -->
{:else if variant === "center-navbar"}
	<div class="m-auto max-w-7xl px-4 py-2">
		<div class="grid grid-cols-1 gap-x-4 pt-4 md:grid-cols-[256px_1fr]">
			<div class="break-words">{@render left()}</div>
			<div class="break-words">{@render right()}</div>
		</div>
	</div>
{:else if variant === "splitpane"}
	<div class="h-full w-full">
		<Resizable.PaneGroup direction="horizontal">
			<Resizable.Pane minSize={30} maxSize={50} defaultSize={30}>
				{@render left()}
			</Resizable.Pane>
			<Resizable.Handle withHandle />
			<Resizable.Pane minSize={50} maxSize={70}>
				{@render right()}
			</Resizable.Pane>
		</Resizable.PaneGroup>
	</div>
{:else if variant === "center"}
	<div class="m-auto max-w-6xl px-4 py-2 h-full">
		{@render left()}
		{@render right()}
	</div>
{:else}
	<svelte:boundary>
		Unknown variant: {variant}
	</svelte:boundary>
{/if}
