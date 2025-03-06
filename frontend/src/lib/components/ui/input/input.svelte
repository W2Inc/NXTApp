<script lang="ts">
	import type { HTMLInputAttributes } from "svelte/elements";
	import type { WithElementRef } from "bits-ui";
	import { cn } from "$lib/utils.js";
	import { Icon } from "lucide-svelte";

	interface Props extends WithElementRef<HTMLInputAttributes> {
		icon?: typeof Icon;
	}

	let {
		ref = $bindable(null),
		value = $bindable(),
		icon,
		class: className,
		...restProps
	}: Props = $props();
</script>

{#snippet inputElem(className2?: string)}
	<input
		bind:this={ref}
		class={cn(
			"border-input placeholder:text-muted-foreground focus-visible:ring-ring read-only:bg-muted flex h-9 w-full rounded-md border bg-transparent px-3 py-1 text-sm shadow-sm transition-colors file:border-0 file:bg-transparent file:text-sm file:font-medium read-only:cursor-not-allowed focus-visible:outline-none focus-visible:ring-1 disabled:cursor-not-allowed disabled:opacity-50",
			className,
			className2,
		)}
		bind:value
		{...restProps}
	/>
{/snippet}

{#if icon}
	<!--TODO: Why? I have no idea -->
	{@const DUMMY = icon}
	<div class="flex flex-col gap-4 sm:flex-row">
		<div class="relative flex-1">
			<DUMMY
				class="text-muted-foreground absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 transform"
			/>
			{@render inputElem("pl-8")}
		</div>
	</div>
{:else}
	{@render inputElem()}
{/if}
