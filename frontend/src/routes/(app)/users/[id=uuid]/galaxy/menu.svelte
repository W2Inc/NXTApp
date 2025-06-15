<script lang="ts">
	import Search from "lucide-svelte/icons/search";
	import Volume2 from "lucide-svelte/icons/volume-2";
	import VolumeX from "lucide-svelte/icons/volume-x";
	import Scan from "lucide-svelte/icons/scan";
	import Flask from "lucide-svelte/icons/flask-conical";

	import { Input } from "$lib/components/ui/input";
	import * as Tabs from "$lib/components/ui/tabs";
	import * as Select from "$lib/components/ui/select";
	import { getContext } from "svelte";
	import { Button } from "$lib/components/ui/button";
	import Separator from "$lib/components/ui/separator/separator.svelte";

	const ctx = getContext<GalaxyCTX>("audio");

	interface Props {
		onCursusChange: (cursus: string) => void;
		search: string;
		cursus: string;
		cursi: string[];
	}

	let {
		search = $bindable(),
		onCursusChange,
		cursi,
		cursus = $bindable(cursi.at(0)!),
	}: Props = $props();

	const availableCursi = cursi.map((c) => {
		return {
			value: c,
			label: c.toWellFormed().replaceAll("-", " "),
		};
	});

	const cursusContent = $derived(
		availableCursi.find((c) => c.value === cursus)?.label ?? "Select a cursus",
	);

	$effect(() => {
		cursus = cursi.at(0)!;
	});
</script>

<div id="kaka" class="flex absolute z-10">
	<div
		class="bg-card flex min-h-12 flex-col items-start gap-2 rounded-br-lg border-r border-b p-2"
	>
		<div class="flex items-center gap-2">
			<Input
				type="search"
				icon={Search}
				bind:value={search}
				list="ice-cream-flavors"
				placeholder="Search goals..."
				class="bg-background w-42"
			/>

			<Select.Root type="single" bind:value={cursus} onValueChange={onCursusChange}>
				<Select.Trigger class="bg-background w-[120px] capitalize">
					{cursusContent}
				</Select.Trigger>
				<Select.Content>
					{#each availableCursi as option}
						<Select.Item value={option.value}>
							{option.label}
						</Select.Item>
					{/each}
				</Select.Content>
			</Select.Root>
		</div>
		<div class="flex gap-2 text-xs">
			<span class="text-primary flex items-center font-bold uppercase">
				<Flask size={16} class="mr-1" />
				beta
			</span>
			<Separator orientation="vertical" />
			<span class="text-muted-foreground underline">
				The graph is currently experimental
			</span>
		</div>
	</div>
	<span id="breadcrumbs">
		<!--Teleport Breadcrumbs-->
	</span>
</div>
