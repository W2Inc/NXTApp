<script lang="ts">
	import Check from "lucide-svelte/icons/check";
	import Loader from "lucide-svelte/icons/loader";
	import ChevronsUpDown from "lucide-svelte/icons/chevrons-up-down";
	import { tick } from "svelte";
	import * as Command from "$lib/components/ui/command/index.js";
	import * as Popover from "$lib/components/ui/popover/index.js";
	import { Button } from "$lib/components/ui/button/index.js";
	import { cn } from "$lib/utils.js";
	import { useDebounce } from "$lib/utils/debounce.svelte";

	type ProjectEntry = BackendTypes["ProjectDO"];
	let frameworks = $state.raw<ProjectEntry[]>([]);
	let open = $state(false);
	let value = $state("");
	let loading = $state(false);
	let triggerRef = $state<HTMLButtonElement>(null!);


	// We want to refocus the trigger button when the user selects
	// an item from the list so users can continue navigating the
	// rest of the form with the keyboard.
	function closeAndFocusTrigger() {
		open = false;
		tick().then(() => {
			triggerRef.focus();
		});
	}

	async function fetchProjects(query?: string) {
		loading = true;
		const params = new URLSearchParams();
		if (query) {
			params.append("q", query);
		}
		const response = await fetch(
			`/projects${params.toString() ? "?" + params.toString() : ""}`,
		);
		frameworks = (await response.json()) as BackendTypes["ProjectDO"][];
		loading = false;
	}

	$effect(() => {
		open && fetchProjects();
	});

	$inspect(loading)

	const debounce = useDebounce();
	const selectedValue = $derived(frameworks.find((f) => f.slug === value)?.name);
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
				{selectedValue || "Select a framework..."}
				<ChevronsUpDown class="ml-2 size-4 shrink-0 opacity-50" />
			</Button>
		{/snippet}
	</Popover.Trigger>
	<Popover.Content class="w-[200px] p-0">
		<Command.Root>
			<Command.Input oninput={(e) => debounce(fetchProjects, e.currentTarget.value)} placeholder="Search framework..." />
			<Command.List>
				{#if loading}
					<Command.Empty>
						<Loader class="animate-spin text-center" />
					</Command.Empty>
				{:else}
					<Command.Empty>No framework found.</Command.Empty>
					<Command.Group>
						{#each frameworks as framework}
							<Command.Item
								value={framework.slug}
								onSelect={() => {
									value = framework.slug;
									closeAndFocusTrigger();
								}}
							>
								<Check
									class={cn(
										"mr-2 size-4",
										value !== framework.slug && "text-transparent",
									)}
								/>
								{framework.name}
							</Command.Item>
						{/each}
					</Command.Group>
				{/if}
			</Command.List>
		</Command.Root>
	</Popover.Content>
</Popover.Root>

<!-- {#if frameworks.length === 0}
					<Command.Loading>
						<Loader class="animate-spin text-center" />
					</Command.Loading>
				{/if} -->
