<script lang="ts">
	import { page } from "$app/state";
	import Pagination from "$lib/components/pagination.svelte";
	import Taskcard from "$lib/components/cards/task-card.svelte";
	import Input from "$lib/components/ui/input/input.svelte";
	import Label from "$lib/components/ui/label/label.svelte";
	import Search from "lucide-svelte/icons/search";
	import Archive from "lucide-svelte/icons/archive";
	import Separator from "$lib/components/ui/separator/separator.svelte";
	import * as Avatar from "$lib/components/ui/avatar";
	import * as Tabs from "$lib/components/ui/tabs/index";
	import { useQuery } from "$lib/utils/query.svelte";
	import { z } from "zod";
	import Base from "$lib/components/base.svelte";

	import { useDebounce } from "$lib/utils/debounce.svelte.js";
	import Skeleton from "$lib/components/ui/skeleton/skeleton.svelte";
	import type { PageProps } from "./$types";
	import Empty from "$lib/components/empty.svelte";
	import Button from "$lib/components/ui/button/button.svelte";
	import Tippy from "$lib/components/tippy.svelte";
	import { encodeID } from "$lib/utils";

	const { data }: PageProps = $props();
	const debounce = useDebounce(500);
	const query = useQuery(
		z.object({
			page: z.number().default(0),
			search: z.string().optional(),
		}),
	);

	function searchProject(search: string) {
		if (search.length > 0) {
			query.write("search", search);
		} else {
			query.write("search", undefined);
		}
	}

	const formatter = new Intl.DateTimeFormat("en-US", {
		day: "numeric",
		month: "long",
		year: "numeric",
	});
</script>

{#snippet userEntry(user: BackendTypes["UserDO"])}
	<Tippy text={`${user.login} - Joined on ${formatter.format(new Date(user.createdAt))}`}>
		<Button data-sveltekit-preload-data="tap" href="/users/{encodeID(user.id)}" variant="outline" class="flex h-auto w-48 flex-col gap-0 overflow-hidden p-0 transition-all duration-300 hover:scale-105">
			<Avatar.Root class="h-48 w-full rounded-none border-0">
				<Avatar.Image
					class="h-full w-full rounded-none object-cover"
					src={user.avatarUrl}
					alt={user.login}
				/>
				<Avatar.Fallback
					class="bg-primary/10 text-primary h-full w-full rounded-none text-4xl font-semibold uppercase"
				>
					{user.login.charAt(0)}
				</Avatar.Fallback>
			</Avatar.Root>
			<div class="w-full border-t p-2 text-center">
				<p class="text-muted-foreground text-sm">{user?.displayName ?? user?.login}</p>
			</div>
		</Button>
	</Tippy>
{/snippet}

<Base>
	{#snippet left()}
		<div class="flex flex-col gap-4">
			<Input
				id="search-cursus"
				type="search"
				icon={Search}
				value={query.read("search")}
				placeholder="Search for users"
				oninput={(v) => debounce(searchProject, v.currentTarget.value.trim())}
			/>

			<!-- TODO: Order filters -->
			<!-- TODO: Filter User status (active, anon) -->
		</div>
	{/snippet}
	{#snippet right()}
		<div class="p-6">
			<menu class="flex justify-between">
				<h1 class="text-2xl font-bold">Users</h1>
				<Pagination page={1} variant="default" onPage={(p) => query.write("page", p)} />
			</menu>
			<Separator class="my-2" />
			<div class="flex flex-wrap gap-4">
				{#await data.users}
					<Skeleton class="h-14 w-32" />
				{:then users}
					{#if users.length === 0}
						<Empty />
					{/if}
					{#each users as user}
						{@render userEntry(user)}
					{/each}
				{/await}
			</div>
		</div>
	{/snippet}
</Base>
