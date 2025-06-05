<script lang="ts" generics="TData">
	import type {Snippet} from "svelte";
	import {useDebounce} from "$lib/utils/debounce.svelte";
	import {Button} from "$lib/components/ui/button";
	import Search from "lucide-svelte/icons/search";
	import CalculatorIcon from "lucide-svelte/icons/calculator";
	import CalendarIcon from "lucide-svelte/icons/calendar";
	import SmileIcon from "lucide-svelte/icons/smile";

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
	let open = $state(false);

	function handleKeydown(e: MouseEvent) {
		e.preventDefault();
		open = !open;
	}
</script>


<Button
	class="justify-start"
	onclick={handleKeydown}
	{placeholder}
	variant="outline"
>
	<Search class="size-4"/>
	<span>{placeholder}</span>
</Button>

<Command.Dialog bind:open>
	<Command.Input placeholder="Search for any entity..."/>
	<Command.List>
		<Command.Empty>No results found.</Command.Empty>
		<Command.Group heading="Results">
			<Command.Item>
				<CalendarIcon class="mr-2 size-4"/>
				<span>Calendar</span>
			</Command.Item>
			<Command.Item>
				<SmileIcon class="mr-2 size-4"/>
				<span>Search Emoji</span>
			</Command.Item>
			<Command.Item>
				<CalculatorIcon class="mr-2 size-4"/>
				<span>Calculator</span>
			</Command.Item>
		</Command.Group>
	</Command.List>
</Command.Dialog>
