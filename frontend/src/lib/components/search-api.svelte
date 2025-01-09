<script lang="ts" generics="TData">
	import Loader from "lucide-svelte/icons/loader";
	import ChevronsUpDown from "lucide-svelte/icons/chevrons-up-down";
	import Search from "lucide-svelte/icons/text-search";
	import { tick, type Snippet } from "svelte";
	import * as Popover from "$lib/components/ui/popover/index.js";
	import { Button } from "$lib/components/ui/button/index.js";
	import { useDebounce } from "$lib/utils/debounce.svelte";
	import Input from "./ui/input/input.svelte";

	const debounce = useDebounce();
	let triggerRef = $state<HTMLButtonElement>(null!);
	let promise = $state<Promise<TData[]> | null>(null);

	interface Props {
		open?: boolean;
		placeholder?: string;
		item: Snippet<[{ value: TData }]>;
		endpointFn: (search: string) => Promise<TData[]>;
		onSelect: (value: TData) => void | Promise<void>;
	}

	let {
		open = $bindable(false),
		placeholder = "Search...",
		endpointFn,
		onSelect,
		item,
	}: Props = $props();

	function searchFor(value: string) {
		if (value === "") return;
		promise = endpointFn(value);
	}

	function closeAndFocusTrigger() {
		open = false;
		tick().then(() => {
			triggerRef.focus();
		});
	}
</script>

<Popover.Root bind:open>
	<Popover.Trigger bind:ref={triggerRef}>
		{#snippet child({ props })}
			<Button
				variant="outline"
				class="w-[200px] justify-between"
				{...props}
				role="combobox"
				aria-expanded={open}
			>
				{placeholder}
				<ChevronsUpDown class="ml-2 size-4 shrink-0 opacity-50" />
			</Button>
		{/snippet}
	</Popover.Trigger>
	<Popover.Content class="w-[350px] p-0">
		<div class="flex items-center border-b px-3">
			<Search class="opacity-50" />
			<Input
				oninput={(e) => debounce(searchFor, e.currentTarget.value.trim())}
				class="placeholder:text-muted-foreground flex h-9 w-full rounded-md border-none bg-transparent py-3 pl-1 text-base outline-none focus-visible:ring-0 disabled:cursor-not-allowed disabled:opacity-50 md:text-sm"
				{placeholder}
			/>
		</div>
		{#if promise}
			<ul class="text-foreground overflow-hidden p-1">
				{#await promise}
					<p class="center-content m-auto w-full">
						Loading
						<Loader class="animate-spin" />
					</p>
				{:then entries}
					{#if entries.length === 0}
						<span
							class="center-content text-muted-foreground items-center justify-center p-2 text-center text-xs"
						>
							Nothing found
						</span>
					{:else}
						{#each entries as entry}
							<li>
								<Button
									variant="ghost"
									class="w-full justify-start p-0 px-2"
									onclick={() => {
										closeAndFocusTrigger();
										onSelect(entry);
									}}
								>
									{@render item({ value: entry })}
								</Button>
							</li>
						{/each}
					{/if}
				{/await}
			</ul>
		{:else}
			<span
				class="center-content text-muted-foreground items-center justify-center p-2 text-center text-xs"
			>
				Start searching by typing...
			</span>
		{/if}
	</Popover.Content>
</Popover.Root>
