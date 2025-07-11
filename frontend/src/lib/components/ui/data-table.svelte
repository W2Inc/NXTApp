<script lang="ts" generics="TData, TValue">
	import {
		type ColumnDef,
		type ColumnFiltersState,
		getCoreRowModel,
		getFilteredRowModel,
		getPaginationRowModel,
		getSortedRowModel,
		type PaginationState,
		type RowSelectionState,
		type SortingState,
	} from "@tanstack/table-core";
	import {createSvelteTable, FlexRender} from "$lib/components/ui/data-table/index.js";
	import {Button, buttonVariants} from "./button";
	import Separator from "./separator/separator.svelte";
	import ArrowLeft from "lucide-svelte/icons/arrow-left";
	import ArrowRight from "lucide-svelte/icons/arrow-right";
	import {Input} from "./input";
	import Search from "lucide-svelte/icons/search";
	import {Constants} from "$lib/utils";
	import * as Select from "$lib/components/ui/select";
	import * as Table from "$lib/components/ui/table";

	type DataTableProps<TData, TValue> = {
		data: TData[];
		columns: ColumnDef<TData, TValue>[];
		searchKey: string;
		placeholder?: string;
		sorting: SortingState;
		pagination: PaginationState;
		rowSelection: RowSelectionState;
	};

	// const paginationOptions = [20, 40, 60, 80];

	const paginationOptions = $derived(Array.from({ length: 4 }, (_, i) => (i + 1) * Constants.PER_PAGE));
	let {
		data,
		columns,
		pagination = $bindable(),
		sorting = $bindable(),
		rowSelection = $bindable(),
		placeholder = "Search...",
		searchKey
	}: DataTableProps<TData, TValue> = $props();

	let columnFilters = $state<ColumnFiltersState>([]);
	const table = createSvelteTable({
		get data() {
			return data;
		},
		columns,
		state: {
			get pagination() {
				return pagination;
			},
			get sorting() {
				return sorting;
			},
			get columnFilters() {
				return columnFilters;
			},
			get rowSelection() {
				return rowSelection;
			},
		},
		onPaginationChange: (updater) => {
			if (typeof updater === "function") {
				pagination = updater(pagination);
			} else {
				pagination = updater;
			}

			// NOTE(W2): Reset row selection on pagination change
			// That way we can cull the selections and keep requests small
			rowSelection = {};
		},
		onSortingChange: (updater) => {
			if (typeof updater === "function") {
				sorting = updater(sorting);
			} else {
				sorting = updater;
			}
		},
		onColumnFiltersChange: (updater) => {
			if (typeof updater === "function") {
				columnFilters = updater(columnFilters);
			} else {
				columnFilters = updater;
			}
		},
		onRowSelectionChange: (updater) => {
			if (typeof updater === "function") {
				rowSelection = updater(rowSelection);
			} else {
				rowSelection = updater;
			}
		},
		getCoreRowModel: getCoreRowModel(),
		getSortedRowModel: getSortedRowModel(),
		getPaginationRowModel: getPaginationRowModel(),
		getFilteredRowModel: getFilteredRowModel(),
	});
</script>

<div class="rounded-md border shadow-sm">
	<div
		class="bg-card sticky top-0 z-10 flex flex-col items-start gap-2 rounded-t-md border-b p-2 md:flex-row md:items-center"
	>
		<div class="relative w-full md:max-w-60">
			<Search
				class="text-muted-foreground absolute left-3 top-1/2 size-4 -translate-y-1/2"
			/>
			<Input
				{placeholder}
				value={(table.getColumn(searchKey)?.getFilterValue() as string) ?? ""}
				onchange={(e) => table.getColumn(searchKey)?.setFilterValue(e.currentTarget.value)}
				oninput={(e) => table.getColumn(searchKey)?.setFilterValue(e.currentTarget.value)}
				class="pl-9 inset-shadow-sm shadow-none"
			/>
		</div>

		<Separator class="flex-1" />
		<div class="flex items-center gap-1">
			<span class="text-muted-foreground hidden flex-1 text-xs lg:block">
				{table.getFilteredSelectedRowModel().rows.length} of{" "}
				{table.getFilteredRowModel().rows.length} row(s) selected
			</span>
			<Separator orientation="vertical" class="h-4" />
			<span class="text-xs font-medium">
				Page {pagination.pageIndex + 1} of {table.getPageCount()}
			</span>
			<Separator orientation="vertical" class="h-4" />
			<span class="text-sm">Items:</span>
			<Select.Root type="single" onValueChange={(v) => table.setPageSize(Number(v))}>
				<Select.Trigger
					class={buttonVariants({
						size: "sm",
						variant: "outline",
						class: "text-foreground w-20 select-none justify-between inset-shadow-sm shadow-none",
					})}>{pagination.pageSize}</Select.Trigger
				>
				<Select.Content>
					{#each paginationOptions as option}
						<Select.Item value={option.toString()}>
							{option}
						</Select.Item>
					{/each}
				</Select.Content>
			</Select.Root>
			<Separator orientation="vertical" class="h-6" />
			<Button
				variant="outline"
				size="sm"
				class="select-none inset-shadow-sm shadow-none"
				onclick={() => table.previousPage()}
				disabled={!table.getCanPreviousPage()}
			>
				<ArrowLeft />
				Previous
			</Button>
			<Button
				variant="outline"
				size="sm"
				class="select-none inset-shadow-sm shadow-none"
				onclick={() => table.nextPage()}
				disabled={!table.getCanNextPage()}
			>
				Next
				<ArrowRight />
			</Button>
		</div>
	</div>
	<div>
		<Table.Root>
			<Table.Header>
				{#each table.getHeaderGroups() as headerGroup (headerGroup.id)}
					<Table.Row class="bg-card">
						{#each headerGroup.headers as header (header.id)}
							<Table.Head>
								{#if !header.isPlaceholder}
									<FlexRender
										content={header.column.columnDef.header}
										context={header.getContext()}
									/>
								{/if}
							</Table.Head>
						{/each}
					</Table.Row>
				{/each}
			</Table.Header>
			<Table.Body>
				{#each table.getRowModel().rows as row (row.id)}
					<Table.Row class="bg-card" data-state={row.getIsSelected() && "selected"}>
						{#each row.getVisibleCells() as cell (cell.id)}
							<Table.Cell>
								<FlexRender
									content={cell.column.columnDef.cell}
									context={cell.getContext()}
								/>
							</Table.Cell>
						{/each}
					</Table.Row>
				{:else}
					<Table.Row>
						<Table.Cell colspan={columns.length} class="h-24 text-center">
							No results.
						</Table.Cell>
					</Table.Row>
				{/each}
			</Table.Body>
		</Table.Root>
	</div>
</div>
