<script lang="ts">
	import Base from "$lib/components/base.svelte";
	import DataTable from "$lib/components/ui/data-table.svelte";
	import { columns } from "./columns";
	import * as Tabs from "$lib/components/ui/tabs";
	import { useQuery } from "$lib/utils/query.svelte";
	import { z } from "zod";
	import type { PageProps } from "./$types";
	import Loader from "lucide-svelte/icons/loader";
	import type {
		PaginationState,
		RowSelectionState,
		SortingState,
	} from "@tanstack/table-core";
	import { Constants } from "$lib/utils";

	const { data }: PageProps = $props();
	// const query = useQuery(
	// 	z.object({
	// 		page: z.number().default(0),
	// 		search: z.string().nullable(),
	// 		filter: z.enum(["read", "unread"]).optional().default("unread"),
	// 	}),
	// );
	let sorting = $state<SortingState>([]);
	let rowSelection = $state<RowSelectionState>({});
	let pagination = $state<PaginationState>({
		pageIndex: 0,
		pageSize: 10,
	});

	// let selectedRows = $derived()

	$effect(() => {
		// data.notifications
	});

	$inspect(pagination);
	$inspect(rowSelection);
	$inspect(data.notifications);
</script>

<svelte:head>
	<title>Notifications</title>
</svelte:head>

<Base>
	{#snippet left()}
		<Tabs.Root value={"read"} onValueChange={(v) => console.log(v)}>
			<Tabs.List class="grid w-full grid-cols-2">
				<Tabs.Trigger value="read">Read</Tabs.Trigger>
				<Tabs.Trigger value="unread">Unread</Tabs.Trigger>
			</Tabs.List>
		</Tabs.Root>
	{/snippet}
	{#snippet right()}
		<div class="px-4 py-4">
			{#await data.notifications}
				<Loader class="animate-spin" />
			{:then notifications}
				<DataTable
					searchKey="Title"
					data={notifications}
					{columns}
					bind:pagination
					bind:sorting
					bind:rowSelection
				/>
			{/await}
		</div>
	{/snippet}
</Base>
