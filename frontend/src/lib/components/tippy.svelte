<script lang="ts">
	import {cn} from "$lib/utils.js";
	import type {WithChild} from "bits-ui";
	import type {Snippet} from "svelte";
	import type {MouseEventHandler} from "svelte/elements";
	import * as Tooltip from "$lib/components/ui/tooltip";

	interface Props extends WithChild {
		text: string;
		class?: string;
		onclick?: MouseEventHandler<HTMLButtonElement>;
		children: Snippet;
	}

	let {text, class: className, children, onclick, child}: Props = $props();
</script>

<Tooltip.Root disableHoverableContent>
	<Tooltip.Trigger {onclick}>
		{@render children?.()}
	</Tooltip.Trigger>
	<Tooltip.Content

		class={cn(
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
