<script lang="ts">
	import Base from "$lib/components/base.svelte";
	import Control from "$lib/components/forms/control.svelte";
	import { useQuery } from "$lib/utils/query.svelte";
	import { z } from "zod";
	import Search from "lucide-svelte/icons/search";
	import Button from "$lib/components/ui/button/button.svelte";
	import Input from "$lib/components/ui/input/input.svelte";
	import * as Select from "$lib/components/ui/select";
	import { useDebounce } from "$lib/utils/debounce.svelte";
	import type { PageProps } from "./$types";
	import UserResult from "$lib/components/search/user-result.svelte";

	const { data }: PageProps = $props();
	const schema = z.object({
		q: z.string({ coerce: true }).default(""),
	});

	const query = useQuery(schema);
	const debounce = useDebounce();
	const handleSearch = (v: string) => query.write("q", v);
</script>

<Base variant="center">
	{#snippet left()}
		<!-- Nothing in left side -->
	{/snippet}
	{#snippet right()}
		<div class="p-6">
			<div class="mx-auto max-w-3xl">
				<h2 class="mb-6 text-2xl font-semibold">Find what you're looking for</h2>
				<Input
					type="text"
					icon={Search}
					placeholder="Enter search terms..."
					oninput={(e) => debounce(handleSearch, e.currentTarget.value)}
				/>
				<span class="text-muted-foreground text-sm">
					Try searching for users, goals, projects or courses
				</span>
			</div>

			<!-- Results would appear here -->
			<div class="m-auto mt-8 max-w-3xl">
				{#if data.query}
					{#await data.users}
						Loading...
					{:then users}
						{#if users && users.length > 0}
							{#each users as user}
								<UserResult data={user} />
							{/each}
						{:else}
							<div class="flex flex-col items-center justify-center gap-2 py-8">
								<div class="text-muted-foreground text-4xl">🔍</div>
								<h3 class="font-semibold">No results found</h3>
							</div>
						{/if}
					{/await}
					<!-- Search results would be rendered here -->
				{:else}
					<p class="text-muted-foreground text-center">Enter a search query to begin</p>
				{/if}
			</div>
		</div>
	{/snippet}
</Base>
