<script lang="ts">
	import {page} from "$app/stores";
	import CircleHelp from "lucide-svelte/icons/circle-help";
	import Pagination from "$lib/components/pagination.svelte";
	import Taskcard from "$lib/components/cards/task-card.svelte";
	import Tippy from "$lib/components/tippy.svelte";
	import Input from "$lib/components/ui/input/input.svelte";
	import Label from "$lib/components/ui/label/label.svelte";
	import Separator from "$lib/components/ui/separator/separator.svelte";
	import {useQuery} from "$lib/utils/query.svelte";
	import {z} from "zod";
	import {useDebounce} from "$lib/utils/debounce.svelte";
	import Base from "$lib/components/base.svelte";
	import type {PageProps} from "./$types";
	import * as Tabs from "$lib/components/ui/tabs/index";
	import {SvelteSet} from "svelte/reactivity";
	import SearchApi from "$lib/components/search-api.svelte";
	import TagInput from "$lib/components/tag-input.svelte";
	import * as Avatar from "$lib/components/ui/avatar";
	import {Select} from "bits-ui";
	import EntitySearch from "$lib/components/EntitySearch.svelte";
	import {GET} from "$lib/utils";


	const {data}: PageProps = $props();
	const debounce = useDebounce();
	const query = useQuery(
		z.object({
			page: z.number().default(0),
			search: z.string().nullable(),
			filter: z.enum(["available", "subscribed"]).optional(),
		}),
	);

	let currentPage = $state(query.read("page"));

	//= Functions =//
	function setSearchQuery(value: string) {
		query.write("search", value.length > 0 ? value : null);
	}

	function setFilterQuery() {
		query.write("filter", "available");
	}

	let projects = new SvelteSet<BackendTypes["ProjectDO"]>();

	async function GET2(query: string) {
		return await GET<BackendTypes["ProjectDO"]>(`/projects?${new URLSearchParams({ name: query })}`);
	}



	let projectNames = $derived(projects.values().map((p) => p.name))

	//= Effects =//
</script>

<svelte:head>
	<title>Users Goals</title>
</svelte:head>

<Base>
	{#snippet left()}
		<EntitySearch onSelect={(e) => { }} api={(input) => { return GET2(input)}}>
			{#snippet item({ key, value })}

			{/snippet}
		</EntitySearch>


		<Input
			id="search-goal"
			type="search"
			placeholder="Search for goal"
			oninput={(e) => debounce(setSearchQuery, e.currentTarget.value.trim())}
		/>
		<Separator/>

		<Tabs.Root value="available" onValueChange={(v) => query.write("filter", v)}>
			<Tabs.List class="w-full">
				<Tabs.Trigger class="w-full" value="available">Available</Tabs.Trigger>
				<Tabs.Trigger class="w-full" value="subscribe">Subscribed</Tabs.Trigger>
			</Tabs.List>
			<Tabs.Content value="available">
			</Tabs.Content>
			<Tabs.Content value="subscribe">
			</Tabs.Content>
		</Tabs.Root>

		<SearchApi
			endpointFn={searchGoals}
			class="w-full"
			placeholder="Search for projects..."
			onSelect={(v) => {projects.add(v)}}
		>
			{#snippet item({value})}
				<div class="center-content w-full justify-between">
					<span title={value.name} class="max-w-[75%] overflow-hidden text-ellipsis font-bold">
						{value.name}
					</span>
					<Avatar.Root class="size-6">
						<Avatar.Image
							src={value.creator?.avatarUrl}
							alt="@{value.creator?.login}"
						/>
						<Avatar.Fallback>US</Avatar.Fallback>
					</Avatar.Root>
				</div>
			{/snippet}
		</SearchApi>

		<TagInput bind:items={projectNames}/>

	{/snippet}

	{#snippet right()}
		<div class="p-6">
			<nav>
				<ol class="flex flex-wrap items-center justify-between">
					<li>
						<Tippy
							text="Goals represent learning objectives that span across projects and curricula. They are
						standalone achievements that, once completed in any context, are validated across all
						curricula. This system ensures efficient learning by recognizing previously acquired
						skills across different educational paths."
						>
							<h1 class="flex items-center gap-2 text-2xl font-bold">
								Goals
								<CircleHelp size={16}/>
							</h1>
						</Tippy>
					</li>
					<li>
						<Pagination variant="default" onPage={(p) => query.write("page", p)}/>
					</li>
				</ol>
			</nav>
			<Separator class="my-2"/>
			<div class="flex flex-wrap gap-4">
				{#each data.goals as goals}
					<Taskcard href="goals/1" type="goal" title="Cursus!"/>
				{/each}
			</div>
		</div>
	{/snippet}
</Base>
