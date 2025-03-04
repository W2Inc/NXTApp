<script lang="ts">
	import Base from "$lib/components/base.svelte";
	import Markdown from "$lib/components/markdown/markdown.svelte";
	import Skeleton from "$lib/components/ui/skeleton/skeleton.svelte";
	import type { PageProps } from "./$types";

	const { data }: PageProps = $props();
</script>

<div class="h-dvh">
	<Base variant="center">
		{#snippet left()}
			<!-- IGNORE -->
		{/snippet}
		{#snippet right()}
			<div class="h-full">
				{#await data.md}
					<div class="h-dvh space-y-6">
						<!-- Header section -->
						<div class="flex flex-col gap-2">
							<Skeleton class="h-10 w-2/3 rounded-md" />
							<div class="flex items-center gap-2">
								<Skeleton class="h-4 w-24 rounded-full" />
								<Skeleton class="h-4 w-32 rounded-full" />
							</div>
						</div>

						<!-- Main content -->
						<div class="space-y-3">
							<Skeleton class="h-5 w-full rounded-md" />
							<Skeleton class="h-5 w-[95%] rounded-md" />
							<Skeleton class="h-5 w-full rounded-md" />
							<Skeleton class="h-5 w-[90%] rounded-md" />
						</div>

						<!-- Feature section -->
						<div class="border-t border-gray-100 pt-4 dark:border-gray-800">
							<Skeleton class="mb-3 h-7 w-48 rounded-md" />
							<div class="grid grid-cols-1 gap-2">
								<div class="flex items-start gap-2">
									<Skeleton class="mt-1 h-4 w-4 rounded-full" />
									<Skeleton class="h-4 w-full rounded-md" />
								</div>
								<div class="flex items-start gap-2">
									<Skeleton class="mt-1 h-4 w-4 rounded-full" />
									<Skeleton class="h-4 w-[92%] rounded-md" />
								</div>
								<div class="flex items-start gap-2">
									<Skeleton class="mt-1 h-4 w-4 rounded-full" />
									<Skeleton class="h-4 w-[85%] rounded-md" />
								</div>
							</div>
						</div>

						<!-- Bug fix section -->
						<div class="border-t border-gray-100 pt-4 dark:border-gray-800">
							<Skeleton class="mb-3 h-7 w-36 rounded-md" />
							<div class="grid grid-cols-1 gap-2">
								<div class="flex items-start gap-2">
									<Skeleton class="mt-1 h-4 w-4 rounded-full" />
									<Skeleton class="h-4 w-full rounded-md" />
								</div>
								<div class="flex items-start gap-2">
									<Skeleton class="mt-1 h-4 w-4 rounded-full" />
									<Skeleton class="h-4 w-[88%] rounded-md" />
								</div>
							</div>
						</div>
					</div>
				{:then md}
					<Markdown fullheight variant="viewer" value={md as string} />
				{/await}
			</div>
		{/snippet}
	</Base>
</div>
