<script lang="ts">
	import { page } from "$app/state";
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
	import Skeleton from "$lib/components/ui/skeleton/skeleton.svelte";

	const { data } = $props();
	const debounce = useDebounce();
	const query = useQuery(
		z.object({
			page: z.number().default(0),
			search: z.string().optional(),
			subscribed: z.boolean().default(true).optional(),
		}),
	);

	function searchProject(search: string) {
		if (search.length > 0) {
			query.write("search", search);
		} else {
			query.write("search", undefined);
		}
	}

	if (!page.data.session) {
		query.write("subscribed", true);
	}
</script>

<svelte:head>
	<title>Users Projects</title>
</svelte:head>

<Base>
	{#snippet left()}
		{#if page.data.session}
			<Tabs.Root
				value={query.read("subscribed") ? "subscribed" : "all"}
				onValueChange={(v) => query.write("subscribed", v === "subscribed")}
			>
				<Tabs.List class="w-full">
					<Tabs.Trigger class="w-full" value="subscribed">Subscribed</Tabs.Trigger>
					<Tabs.Trigger class="w-full" value="all">All</Tabs.Trigger>
				</Tabs.List>
			</Tabs.Root>
		{/if}

		<Label for="search-cursus">Search</Label>
		<Input
			id="search-cursus"
			type="search"
			value={query.read("search")}
			placeholder="Search for cursus"
			oninput={(v) => debounce(searchProject, v.currentTarget.value.trim())}
		/>
	{/snippet}

	{#snippet right()}
		<div class="p-6">
			<menu class="flex justify-between">
				<h1 class="text-2xl font-bold">Projects</h1>
				<!-- <Pagination
					pages={pagination?.pages ?? 1}
					perPage={pagination?.perPage ?? 10}
					variant="default"
					onPage={(p) => query.write("page", p)}
				/> -->
			</menu>
			<Separator class="my-2" />
			<div class="flex flex-wrap gap-4">
				{#await data.projects}
					<Skeleton class="h-28 w-28" />
				{:then projects}
					{#if projects && projects.length > 0}
						{#each projects as p}
							{#if query.read("subscribed") ?? false}
								{@const userProject = p as BackendTypes["UserProjectDO"]}
								<Taskcard
									href="projects/{userProject.project?.slug}"
									type="project"
									title={userProject.project?.name}
									state={userProject.state}
								/>
							{:else}
								{@const project = p as BackendTypes["ProjectDO"]}
								<Taskcard
									href="projects/{project.slug}"
									type="project"
									title={project.name}
								/>
							{/if}
						{/each}
					{:else}
						Nothing here...
					{/if}
				{:catch}
					Something went wrong...
				{/await}
			</div>
		</div>
	{/snippet}
</Base>
