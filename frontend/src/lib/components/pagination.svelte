<script lang="ts">
	import * as Pagination from "$lib/components/ui/pagination/index";
	import * as Select from "$lib/components/ui/select";
	import ArrowRight from "lucide-svelte/icons/arrow-right";
	import ArrowLeft from "lucide-svelte/icons/arrow-left";
	import ChevronDown from "lucide-svelte/icons/chevron-down";
	import Separator from "./ui/separator/separator.svelte";
	import { buttonVariants } from "./ui/button";

	interface Props {
		/** Total number of items */
		totalItems?: number;
		/** How many per page */
		perPage?: number;
		/** Current page */
		page?: number;
		/** Display variant */
		variant?: "small" | "default";
		/** Available page size options */
		pageSizeOptions?: number[];
		/** Handle next page click */
		onNext?: (p: number) => void | Promise<void>;
		/** Handle previous page click */
		onPrevious?: (p: number) => void | Promise<void>;
		/** Handle page change */
		onPage?: (p: number) => void | Promise<void>;
		/** Handle page size change */
		onPageSizeChange?: (size: number) => void | Promise<void>;
	}

	let {
		totalItems = 0,
		perPage = $bindable(10),
		page = $bindable(1),
		variant = "default",
		pageSizeOptions = [10, 25, 50, 100],
		onNext,
		onPrevious,
		onPage,
		onPageSizeChange,
	}: Props = $props();

	const totalPages = $derived(Math.max(1, Math.ceil(totalItems / perPage)));
	function handlePageSizeChange(newSize: number) {
		perPage = newSize;
		if (page > totalPages) {
			page = totalPages;
		}
		onPageSizeChange?.(newSize);
	}
</script>

<div class="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
	{#if variant === "default"}
		<div class="hidden shrink-0 items-center gap-2 lg:flex">
			<Select.Root
				type="single"
				value={perPage.toString()}
				onValueChange={(val) => handlePageSizeChange(Number(val))}
			>
				<Select.Trigger class="h-9 w-[80px]">
					{perPage.toString()}
				</Select.Trigger>
				<Select.Content>
					<Select.Group>
						{#each pageSizeOptions as size}
							<Select.Item value={size.toString()}>
								{size}
							</Select.Item>
						{/each}
					</Select.Group>
				</Select.Content>
			</Select.Root>
		</div>
	{/if}

	<Separator orientation="vertical" class="hidden lg:block" />

	<!-- Pagination Component -->
	<div class="flex-1">
		<Pagination.Root bind:page count={totalPages} {perPage} onPageChange={onPage}>
			{#snippet children({ pages, currentPage })}
				<Pagination.Content class="flex items-center gap-2">
					<Pagination.Item>
						<Pagination.PrevButton
							class="border-input bg-background hover:bg-accent hover:text-accent-foreground focus-visible:ring-ring inline-flex h-9 items-center justify-center rounded-md border px-3 text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50"
							onclick={() => onPrevious && onPrevious(currentPage)}
							disabled={currentPage <= 1}
						>
							<ArrowLeft class="h-4 w-4" />
							{#if variant === "default"}
								<span class=" hidden sm:inline">Previous</span>
							{/if}
						</Pagination.PrevButton>
					</Pagination.Item>

					{#if variant === "default"}
						<div class="hidden items-center rounded border sm:flex">
							{#each pages as page (page.key)}
								{#if page.type === "ellipsis"}
									<Pagination.Item>
										<Pagination.Ellipsis
											class="text-muted-foreground inline-flex h-9 w-9 items-center justify-center text-sm"
										/>
									</Pagination.Item>
								{:else}
									<Pagination.Item>
										<Pagination.Link
											{page}
											isActive={currentPage === page.value}
											class={buttonVariants({
												variant: "ghost",
												class: "[data-selected]:bg-primary border-none shadow-none",
											})}
										>
											{page.value}
										</Pagination.Link>
									</Pagination.Item>
								{/if}
							{/each}
						</div>
					{:else}
						<Pagination.Item>
							<span class="px-2 text-sm font-medium">
								Page {currentPage} of {totalPages}
							</span>
						</Pagination.Item>
					{/if}

					<Pagination.Item>
						<Pagination.NextButton
							class={buttonVariants({ variant: "outline" })}
							onclick={() => onNext && onNext(currentPage)}
							disabled={currentPage >= totalPages}
						>
							{#if variant === "default"}
								<span class=" hidden sm:inline">Next</span>
							{/if}
							<ArrowRight class="h-4 w-4" />
						</Pagination.NextButton>
					</Pagination.Item>
				</Pagination.Content>
			{/snippet}
		</Pagination.Root>
	</div>
</div>
