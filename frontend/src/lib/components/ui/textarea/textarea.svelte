<script lang="ts">
	import type { WithElementRef, WithoutChildren } from "bits-ui";
	import type { HTMLTextareaAttributes } from "svelte/elements";
	import { cn } from "$lib/utils.js";

	let {
		ref = $bindable(null),
		value = $bindable(""),
		class: className,
		maxlength,
		...restProps
	}: WithoutChildren<WithElementRef<HTMLTextareaAttributes>> = $props();
</script>

<div>
	<textarea
		bind:this={ref}
		bind:value
		class={cn(
			"border-input placeholder:text-muted-foreground focus-visible:ring-ring flex min-h-[60px] w-full rounded-md border bg-transparent px-3 py-2 text-sm shadow-sm focus-visible:outline-none focus-visible:ring-1 disabled:cursor-not-allowed disabled:opacity-50",
			className,
		)}
		{...restProps}
	></textarea>
	<div class="flex justify-between flex-row-reverse">
		{#if maxlength && value && typeof value === "string"}
			<span
				class="inline-flex items-center gap-1 text-xs"
				class:text-muted-foreground={value.length <= maxlength}
				class:text-destructive={value.length > maxlength}
			>
				{value.length} / {maxlength}
			</span>
		{/if}
	</div>
</div>
