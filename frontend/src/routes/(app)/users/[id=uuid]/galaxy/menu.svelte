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

<div
	class="bg-card absolute z-10 flex min-h-12 flex-col items-start gap-2 rounded-br-lg border-b border-r p-2"
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

		<datalist id="ice-cream-flavors">
			<option value="Chocolate"></option>
			<option value="Coconut"></option>
			<option value="Mint"></option>
			<option value="Strawberry"></option>
			<option value="Vanilla"></option>
		</datalist>

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

		<Tabs.Root bind:value={ctx.mode} class="w-[100px]">
			<Tabs.List class="grid w-full grid-cols-2">
				<Tabs.Trigger value="2d">2D</Tabs.Trigger>
				<Tabs.Trigger value="3d">3D</Tabs.Trigger>
			</Tabs.List>
		</Tabs.Root>

		{#if ctx.mode === "3d"}
			<Button
				size="icon"
				variant="secondary"
				onclick={() => (ctx.audioPlaying = !ctx.audioPlaying)}
			>
				{#if ctx.audioPlaying}
					<Volume2 />
				{:else}
					<VolumeX />
				{/if}
			</Button>
		{/if}
	</div>
	{#if ctx.mode === "3d"}
		<div class="flex gap-2 text-xs">
			<span class="text-primary flex items-center font-bold uppercase">
				<Flask size={16} class="mr-1" />
				beta
			</span>
			<Separator orientation="vertical" />
			<span class="text-muted-foreground underline"
				>The Galaxy graph is still rough and in development</span
			>
		</div>
	{/if}
</div>
