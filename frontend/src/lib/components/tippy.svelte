<script lang="ts">
	import * as Tooltip from "$lib/components/ui/tooltip";
	import { cn } from "$lib/utils.js";
	import type { Snippet } from "svelte";

	interface Props {
		text: string;
		class?: string;
		children: Snippet;
	}

	let { text, class: className, children }: Props = $props();
</script>

<Tooltip.Root>
	<Tooltip.Trigger>
		{@render children?.()}
	</Tooltip.Trigger>
	<Tooltip.Content
		class={cn(
			"bg-muted text-muted-foreground dark:border-background max-w-64 border p-2 text-sm leading-relaxed tracking-wide shadow-xl",
			className,
		)}
	>
		<div class="[&>*:not(:last-child)]:after:content-['.'] [&>*]:block [&>*]:pb-1">
			{#each text.split(". ") as sentence}
				<span>{sentence}</span>
			{/each}
		</div>
	</Tooltip.Content>
</Tooltip.Root>
