<script lang="ts">
	import Base from "$lib/components/base.svelte";
	import DataTable from "$lib/components/ui/data-table.svelte";
	import {columns} from "./columns";
	import {useQuery} from "$lib/utils/url.svelte";
	import type {PageProps} from "./$types";
	import Loader from "lucide-svelte/icons/loader";
	import type {PaginationState, RowSelectionState, SortingState,} from "@tanstack/table-core";
	import Separator from "$lib/components/ui/separator/separator.svelte";
	import Button from "$lib/components/ui/button/button.svelte";
	import MailOpen from "lucide-svelte/icons/mail-open";
	import RefreshCw from "lucide-svelte/icons/refresh-cw";
	import type {QueryKeys} from "./+page.server";
	import {page} from "$app/state";
	import Switch from "$lib/components/ui/switch/switch.svelte";
	import {invalidate} from "$app/navigation";
	import {useDebounce} from "$lib/utils/debounce.svelte";
	import {BookOpen, FolderOpen, MessageSquare, Target, UserPlus} from "lucide-svelte";

	// Notification kinds with their masks and UI representations
	const notificationKinds = {
		invite: {
			mask: 1 << 0,
			value: `${1 << 0}`,
			label: "Invite",
			icon: UserPlus,
		},
		project: {
			mask: 1 << 5,
			value: `${1 << 5}`,
			label: "Project",
			icon: FolderOpen,
		},
		goal: {
			mask: 1 << 6,
			value: `${1 << 6}`,
			label: "Goal",
			icon: Target,
		},
		cursus: {
			mask: 1 << 7,
			value: `${1 << 7}`,
			label: "Cursus",
			icon: BookOpen,
		},
		review: {
			mask: 1 << 8,
			value: `${1 << 8}`,
			label: "Review",
			icon: MessageSquare,
		},
	};

	const { data }: PageProps = $props();
	const debounce = useDebounce();
	const query = useQuery<QueryKeys>(page.url);

	let selected = $state<string[]>([]);
	let sorting = $state<SortingState>([]);
	let rowSelection = $state<RowSelectionState>({});
	let pagination = $state<PaginationState>({
		pageIndex: 0,
		pageSize: 10,
	});

	// Derive the rows from selection
	const rows = $derived.by(() => {
		const selectedIndexes = Object.keys(rowSelection).map(Number);
		return data.notifications.filter((_, index) => selectedIndexes.includes(index));
	});

	// Utility functions for mask handling
	function maskToSelectedValues(mask: number): string[] {
		if (!mask) return [];

		return Object.values(notificationKinds)
			.filter((kind) => (mask & kind.mask) !== 0)
			.map((kind) => kind.value);
	}

	function selectedValuesToMask(values: string[]): number {
		return values.reduce((mask, value) => {
			const intValue = parseInt(value, 10);
			return isNaN(intValue) ? mask : mask | intValue;
		}, 0);
	}

	// Action handlers
	const setRead = (v: string) =>
		debounce(() => {
			query.reset();
			query.write("read", v);
		});

	const setExclude = (v: boolean) =>
		debounce(() => {
			query.write("type", v ? "exclude" : "include");
		});

	const refresh = () => debounce(async () => await invalidate("app:notifications"));

	const setMask = (values: string[]) => {
		const mask = selectedValuesToMask(values);

		debounce(() => {
			// Only set mask if there's actually a value
			if (mask > 0) {
				query.write("mask", mask.toString());
			} else {
				query.write("mask", null);
			}
		});
	};

	// Initialize selected values from URL on load and when URL changes
	$effect(() => {
		const maskParam = query.read("mask");
		const mask = maskParam ? parseInt(maskParam, 10) : 0;

		if (!isNaN(mask) && mask > 0) {
			selected = maskToSelectedValues(mask);
		} else {
			selected = [];
		}
	});
</script>

<svelte:head>
	<title>Notifications</title>
</svelte:head>

<Base>
	{#snippet left()}
		<Tabs.Root value={query.read("read") ?? "unread"} onValueChange={setRead}>
			<Tabs.List class="grid w-full grid-cols-2">
				<Tabs.Trigger value="unread">Unread</Tabs.Trigger>
				<Tabs.Trigger value="read">Read</Tabs.Trigger>
			</Tabs.List>
		</Tabs.Root>
		<div class="flex gap-1">
			<Select.Root type="multiple" value={selected} onValueChange={setMask}>
				<Select.Trigger class="max-w-[180px]" type="button">Filter</Select.Trigger>
				<Select.Content>
					<div class="bg-muted/50 flex items-center justify-between rounded-sm px-3 py-2">
						<span class="text-xs font-medium">Exclude</span>
						<Switch
							checked={query.read("type") === "exclude"}
							onCheckedChange={setExclude}
						/>
					</div>
					<Separator class="my-1"/>
					{#each Object.values(notificationKinds) as kind}
						<Select.Item value={kind.value}>
							<kind.icon class="mr-2 h-4 w-4"/>
							{kind.label}
						</Select.Item>
					{/each}
				</Select.Content>
			</Select.Root>
			<Separator orientation="vertical"/>

			<form class="w-full" method="POST" action="?/read" enctype="multipart/form-data">
				{#each rows as row}
					<input type="hidden" name="id" value={row.id}/>
				{/each}
				<Button
					class="w-full"
					type="submit"
					onclick={() => {}}
					disabled={!rows.length}
					variant="outline"
				>
					Mark as Read
					<MailOpen/>
				</Button>
			</form>
		</div>
		<Separator/>
		<Button variant="outline" onclick={refresh}>
			<RefreshCw/>
			<span class="text-xs font-medium">Fetch Notifications</span>
		</Button>
	{/snippet}
	{#snippet right()}
		<div class="grid h-min grid-cols-1 gap-4 px-4 py-4 [&>*]:h-fit">
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
