<script lang="ts">
	import * as Tooltip from "$lib/components/ui/tooltip";
	import { cn } from "$lib/utils.js";
	import type { Snippet } from "svelte";
	import type { HTMLButtonAttributes, MouseEventHandler } from "svelte/elements";

	interface Props {
		text: string;
		class?: string;
		onclick?: MouseEventHandler<HTMLButtonElement>;
		children: Snippet;
	}

	let { text, class: className, children, onclick }: Props = $props();
</script>

<Tooltip.Root disableHoverableContent>
	<Tooltip.Trigger {onclick}>
		{@render children?.()}
	</Tooltip.Trigger>
	<Tooltip.Content

		class={cn(
			"bg-muted text-muted-foreground dark:border-background max-w-64 border p-2 text-sm leading-relaxed tracking-wide shadow-xl",
			className,
		)}
	>
		<div
			class="[[&>*:not(:last-child)]:pb-1 [&>*:not(:last-child)]:after:content-['.'] *:block"
		>
			{#each text.split(". ") as sentence}
				<span>{sentence}</span>
			{/each}
		</div>
	</Tooltip.Content>
</Tooltip.Root>
