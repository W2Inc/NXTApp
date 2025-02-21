import { Checkbox } from "$lib/components/ui/checkbox";
import { renderComponent, renderSnippet } from "$lib/components/ui/data-table";
import type { ColumnDef } from "@tanstack/table-core";
import DataTableActions from "./data-table-actions.svelte";
import DataTableSortDate from "./data-table-sort-date.svelte";
import { createRawSnippet } from "svelte";

// This type is used to define the shape of our data.
// You can use a Zod schema here if you want.
export type BackendNotification = {
	id: string;
	title: string;
	createdAt: Date;
	type: "invite" | "notification";
};

export const columns: ColumnDef<BackendNotification>[] = [
	{
		id: "select",
		header: ({ table }) =>
			renderComponent(Checkbox, {
				checked: table.getIsAllPageRowsSelected(),
				indeterminate:
					table.getIsSomePageRowsSelected() && !table.getIsAllPageRowsSelected(),
				onCheckedChange: (value) => table.toggleAllRowsSelected(!!value),
				"aria-label": "Select all",
			}),
		cell: ({ row }) =>
			renderComponent(Checkbox, {
				checked: row.getIsSelected(),
				onCheckedChange: (value) => row.toggleSelected(!!value),
				"aria-label": "Select row",
			}),
		enableSorting: false,
		enableHiding: false,
	},
	{
		accessorKey: "title",
		header: "Title",
	},
	{
		header: "Type",
		cell: ({ row }) => {
			const typeCell = createRawSnippet<[string]>((value) => {
				return {
					render: () => `<div class="capitalize">${value()}</div>`,
				};
			});

			return renderSnippet(typeCell, row.original.type);
		},
	},
	{
		accessorKey: "createdAt",
		cell: ({ row }) => {
			const formatter = new Intl.DateTimeFormat("en-US", {
				dateStyle: "long",
				timeStyle: "short",
			});

			const amountCellSnippet = createRawSnippet<[string]>((getAmount) => {
				const amount = getAmount();
				return {
					render: () => `<div class="font-medium">${amount}</div>`,
				};
			});

			return renderSnippet(
				amountCellSnippet,
				formatter.format(row.getValue("createdAt")),
			);
		},
		header: ({ column }) =>
			renderComponent(DataTableSortDate, {
				onclick: () => column.toggleSorting(column.getIsSorted() === "asc"),
			}),
	},
	{
		id: "actions",
		header: () => {
			const actionHeader = createRawSnippet<[string]>((value) => {
				return {
					render: () => `<div class="text-right pr-4">${value()}</div>`,
				};
			});

			return renderSnippet(actionHeader, "Action");
		},
		cell: ({ row }) => {
			return renderComponent(DataTableActions, {
				id: row.original.id,
				type: row.original.type,
			});
		},
	},
];
