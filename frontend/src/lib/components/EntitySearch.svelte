<script lang="ts" generics="TData">
	import * as Popover from "$lib/components/ui/popover/index.js";
	import type {Snippet} from "svelte";
	import {useDebounce} from "$lib/utils/debounce.svelte";
	import {cn} from "$lib/utils";
	import {Button} from "$lib/components/ui/button";
	import ChevronsUpDown from "lucide-svelte/icons/chevrons-up-down.svelte";
	import Input from "./ui/input/input.svelte";

	interface Props {
		placeholder?: string;
		class?: string;
		item: Snippet<[{ key: string, value: TData }]>;
		api: (input: string) => Promise<TData[]>
		onSelect: (value: { key: string, value: TData }) => void | Promise<void>;
	}

	const {item, class: className, placeholder = "Search..."}: Props = $props();
	let triggerRef = $state<HTMLButtonElement>(null!);
	let promise = $state<Promise<TData[]> | null>(null);

	const debounce = useDebounce();
</script>


<Popover.Root>
	<Popover.Trigger bind:ref={triggerRef}>
		{#snippet child({props})}
			<Button
				variant="outline"
				class={cn("w-[200px] justify-between text-muted-foreground ", className)}
				{...props}
				role="combobox"
			>
				{placeholder}
				<ChevronsUpDown class="ml-2 size-4 shrink-0 opacity-50"/>
			</Button>
		{/snippet}
	</Popover.Trigger>
	<Popover.Content class="w-[350px] p-0" align="start">
		<Input
			oninput={(e) => debounce(() => {}, e.currentTarget.value.trim())}
			class="placeholder:text-muted-foreground flex h-9 w-full rounded-md border-none bg-transparent py-3 pl-1 text-base outline-none focus-visible:ring-0 disabled:cursor-not-allowed disabled:opacity-50 md:text-sm shadow-none"
			{placeholder}
		/>
	</Popover.Content>
</Popover.Root>
