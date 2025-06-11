<script lang="ts">
	import CircleHelp from "lucide-svelte/icons/circle-help";
	import Pagination from "$lib/components/pagination.svelte";
	import Taskcard from "$lib/components/cards/task-card.svelte";
	import Tippy from "$lib/components/tippy.svelte";
	import Input from "$lib/components/ui/input/input.svelte";
	import Separator from "$lib/components/ui/separator/separator.svelte";
	import {useQuery} from "$lib/utils/url.svelte";
	import {useDebounce} from "$lib/utils/debounce.svelte";
	import Base from "$lib/components/base.svelte";
	import type {PageProps} from "./$types";
	import {page} from "$app/state";
	import type {QueryKeys} from "./+page.server";
	import {encodeID} from "$lib/utils";
	import * as Tabs from "$lib/components/ui/tabs";

	const debounce = useDebounce();
	const query = useQuery<QueryKeys>(page.url);
	const { data }: PageProps = $props();
</script>

<svelte:head>
	<title>Users Goals</title>
</svelte:head>

<Base>
	{#snippet left()}
		<Input
			id="search-goal"
			type="search"
			placeholder="Search for goal"
			value={query.read("search")}
			oninput={(e) =>
				debounce((v: string) => query.write("search", v), e.currentTarget.value.trim())}
		/>
		<Separator />

		<Tabs.Root value={query.read("filter") ?? "subscribed"} onValueChange={(v) => query.write("filter", v)}>
			<Tabs.List class="w-full">
				<Tabs.Trigger class="w-full" value="available">Available</Tabs.Trigger>
				<Tabs.Trigger class="w-full" value="subscribed">Subscribed</Tabs.Trigger>
			</Tabs.List>
			<Tabs.Content value="available"></Tabs.Content>
			<Tabs.Content value="subscribed"></Tabs.Content>
		</Tabs.Root>
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
								<CircleHelp size={16} />
							</h1>
						</Tippy>
					</li>
					<li>
						<Pagination
							variant="default"
							pages={data.XPages}
							onPage={(p) => query.write("page", p)}
							onPageSizeChange={(s) => query.write("size", s)}
						/>
					</li>
				</ol>
			</nav>
			<Separator class="my-2" />
			<div class="flex flex-wrap gap-4">
				{#if query.read("filter") === "available"}
					{#each data.goals as entity}
						{@const goal = entity as BackendTypes["LearningGoalDO"]}
						<Taskcard href="goals/{encodeID(goal.id)}" type="goal" title={goal.name} />
					{/each}
				{:else}
					{#each data.goals as entity}
						{@const goal = entity as BackendTypes["UserGoalDO"]}
						<Taskcard href="goals/{encodeID(goal.goalId)}" type="goal" title={goal.name ?? "Unknown"} />
					{/each}
				{/if}
			</div>
		</div>
	{/snippet}
</Base>
