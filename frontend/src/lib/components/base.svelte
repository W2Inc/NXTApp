<script lang="ts">
	import type { Snippet } from "svelte";

	interface Props {
		left: Snippet;
		right: Snippet;
		variant?: "navbar" | "center-navbar";
	}

	const { left, right, variant = "navbar" }: Props = $props();
</script>

<!-- A sticky / fixed navbar to the side with the content taking the main chunk of page -->
{#if variant === "navbar"}
	<div class="grid grid-rows-[auto_1fr] lg:grid-cols-[auto_1fr] lg:grid-rows-1">
		<aside
			class="dark:bg-card sticky top-0 z-10 flex min-w-96 flex-col gap-2 p-4 lg:h-dvh lg:border-r"
		>
			{@render left()}
		</aside>

		<!-- Body -->
		{@render right()}
	</div>
	<!-- A center-navbar is where both navbar and center content are center aligned -->
{:else if variant === "center-navbar"}
	<div class="m-auto max-w-6xl px-4 py-2">
		<div class="grid grid-cols-1 gap-x-4 pt-4 md:grid-cols-[256px,1fr]">
			{@render left()}
			{@render right()}
		</div>
	</div>
{:else}
	<svelte:boundary>
		Unknown variant: {variant}
	</svelte:boundary>
{/if}
