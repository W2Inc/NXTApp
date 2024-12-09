<script lang="ts">
	import * as Pagination from "$lib/components/ui/pagination/index";
	import ArrowRight from "lucide-svelte/icons/arrow-right";
	import ArrowLeft from "lucide-svelte/icons/arrow-left";
	import Button from "./ui/button/button.svelte";
	interface Props {
		count?: number;
		perPage?: number;
		onNext?: (p: number) => void | Promise<void>;
		onPrevious?: (p: number) => void | Promise<void>;
		onPage?: (p: number) => void | Promise<void>;
	}

	const { count = 100, perPage = 10, onNext, onPrevious, onPage }: Props = $props();
</script>

<div>
	<Pagination.Root {count} {perPage} onPageChange={onPage}>
		{#snippet children({ pages, currentPage })}
			<Pagination.Content>
				<Pagination.Item>
					<Pagination.PrevButton
						class="inline-flex h-9 w-9 items-center justify-center p-0"
						onclick={() => onPrevious && onPrevious(currentPage)}
					>
						<ArrowLeft />
					</Pagination.PrevButton>
				</Pagination.Item>
				{#each pages as page (page.key)}
					{#if page.type === "ellipsis"}
						<Pagination.Item>
							<Pagination.Ellipsis />
						</Pagination.Item>
					{:else}
						<Pagination.Item>
							<Pagination.Link {page} isActive={currentPage === page.value}>
								{page.value}
							</Pagination.Link>
						</Pagination.Item>
					{/if}
				{/each}
				<Pagination.Item>
					<Pagination.NextButton
						class="inline-flex h-9 w-9 items-center justify-center p-0"
						onclick={() => onNext && onNext(currentPage)}
					>
						<ArrowRight />
					</Pagination.NextButton>
				</Pagination.Item>
			</Pagination.Content>
		{/snippet}
	</Pagination.Root>
</div>
