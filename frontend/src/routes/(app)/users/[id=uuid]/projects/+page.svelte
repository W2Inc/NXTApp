<script lang="ts">
	import { page } from "$app/state";
	import Pagination from "$lib/components/pagination.svelte";
	import Taskcard from "$lib/components/cards/task-card.svelte";
	import Input from "$lib/components/ui/input/input.svelte";
	import Label from "$lib/components/ui/label/label.svelte";
	import Search from "lucide-svelte/icons/search";
	import Archive from "lucide-svelte/icons/archive";
	import Separator from "$lib/components/ui/separator/separator.svelte";
	import * as Tabs from "$lib/components/ui/tabs/index";
	import { useQuery } from "$lib/utils/url.svelte";

	import { z } from "zod";
	import Base from "$lib/components/base.svelte";

	import { useDebounce } from "$lib/utils/debounce.svelte.js";
	import Skeleton from "$lib/components/ui/skeleton/skeleton.svelte";
	import type { PageProps } from "./$types";
	import Empty from "$lib/components/empty.svelte";
	import * as Alert from "$lib/components/ui/alert";
	import type { QueryKeys } from "./+page.server";

	const { data }: PageProps = $props();
	const debounce = useDebounce();
	const query = useQuery<QueryKeys>(page.url);
</script>

<svelte:head>
	<title>Users Projects</title>
</svelte:head>

<Base>
	{#snippet left()}
		<Input
			id="search-cursus"
			type="search"
			icon={Search}
			value={query.read("search")}
			placeholder="Search for cursus"
			oninput={(v) =>
				debounce((q: string) => query.write("filter", q), v.currentTarget.value.trim())}
		/>
		{#if data.session && data.isCurrentUser}
			<Separator />
			<Tabs.Root
				value={query.read("filter") ?? "subscribed"}
				onValueChange={(v) => query.write("filter", v)}
			>
				<Tabs.List class="w-full">
					<Tabs.Trigger class="w-full" value="subscribed">Subscribed</Tabs.Trigger>
					<Tabs.Trigger class="w-full" value="all">Available</Tabs.Trigger>
				</Tabs.List>
			</Tabs.Root>
		{/if}
	{/snippet}

	{#snippet right()}
		<div class="p-6">
			<menu class="flex justify-between">
				<h1 class="text-2xl font-bold">Projects</h1>
				<Pagination variant="default" onPage={(p) => query.write("page", p)} />
			</menu>
			<Separator class="my-2" />
			<div class="flex flex-wrap gap-4">
				{#if data.projects.length === 0}
					<Empty />
				{/if}
				{#key query.read("filter")}
					{#if query.read("filter") === "subscribed" || !data.isCurrentUser}
						{#each data.projects as up}
							{@const userProject = up as BackendTypes["UserProjectDO"]}
							<Taskcard
								href="projects/{userProject.project?.slug}"
								type="project"
								title={userProject.project?.name ?? "Unknown"}
								state={userProject.state}
							/>
						{/each}
					{:else}
						{#each data.projects as p}
							{@const project = p as BackendTypes["ProjectDO"]}
							<Taskcard
								href="projects/{project.slug}"
								type="project"
								title={project.name}
							/>
						{/each}
					{/if}
				{/key}
			</div>
		</div>
	{/snippet}
</Base>
