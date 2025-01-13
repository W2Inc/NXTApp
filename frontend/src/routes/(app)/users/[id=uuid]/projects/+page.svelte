<script lang="ts">
	import { page } from "$app/stores";
	import Pagination from "$lib/components/pagination.svelte";
	import Taskcard from "$lib/components/cards/task-card.svelte";
	import Input from "$lib/components/ui/input/input.svelte";
	import Label from "$lib/components/ui/label/label.svelte";
	import Separator from "$lib/components/ui/separator/separator.svelte";
	import * as Tabs from "$lib/components/ui/tabs/index";
	import { useQuery } from "$lib/utils/query.svelte";
	import { z } from "zod";
	import Base from "$lib/components/base.svelte";
	import { invalidate, invalidateAll } from "$app/navigation";
	import { tick } from "svelte";
	import { useDebounce } from "$lib/utils/debounce.svelte.js";

	const { data } = $props();
	const debounce = useDebounce();
	const query = useQuery(
		$page.url.href,
		z.object({
			page: z.number().default(0),
			q: z.string().optional(),
			filter: z.enum(["available", "subscribed"]).optional(),
		}),
	);

	let pagination = $state<PaginationMeta>();
	$effect(() => {
		if (data.projects) {
			// data.projects.then((result) => (pagination = result.pagination));
		}
	});

	function searchProject(search: string) {
		if (search.length > 0) {
			query.write("q", search);
		}
	}
</script>

<svelte:head>
	<title>Users Projects</title>
</svelte:head>

<Base>
	{#snippet left()}
		<Tabs.Root
			value="subscribed"
			onValueChange={(v) => query.write("filter", v as "available" | "subscribed")}
		>
			<Tabs.List class="w-full">
				<Tabs.Trigger class="w-full" value="subscribed">Subscribed</Tabs.Trigger>
				<Tabs.Trigger class="w-full" value="available">Available</Tabs.Trigger>
			</Tabs.List>
		</Tabs.Root>
		<form>
			<Label for="search-cursus">Search</Label>
			<Input
				id="search-cursus"
				type="search"
				placeholder="Search for cursus"
				oninput={(v) => debounce(searchProject, v.currentTarget.value.trim())}
			/>
		</form>
	{/snippet}

	{#snippet right()}
		<div class="p-6">
			<menu class="flex justify-between">
				<h1 class="text-2xl font-bold">Projects</h1>
				<Pagination
					pages={pagination?.pages ?? 1}
					perPage={pagination?.perPage ?? 10}
					variant="default"
					onPage={(p) => query.write("page", p)}
				/>
			</menu>
			<Separator class="my-2" />
			<div class="flex flex-wrap gap-4">
				{#await data.projects}
					Loading...
				{:then projects}
					{#if projects.data.length > 0}
						{#each projects.data as project}
							<Taskcard
								href="projects/{project.slug}"
								type="project"
								title={project.name}
							/>
						{/each}
					{:else}
						Nothing...
					{/if}
				{/await}
			</div>
		</div>
	{/snippet}
</Base>
