<script lang="ts">
	import { page } from "$app/stores";
	import Pagination from "$lib/components/pagination.svelte";
	import Taskcard from "$lib/components/cards/task-card.svelte";
	import Input from "$lib/components/ui/input/input.svelte";
	import Label from "$lib/components/ui/label/label.svelte";
	import Separator from "$lib/components/ui/separator/separator.svelte";
	import * as Tabs from "$lib/components/ui/tabs/index";
	import { useQuery } from "$lib/utils/query.svelte";
	import { z } from "zod";
	import Base from "$lib/components/base.svelte";

	const query = useQuery(
		$page.url.href,
		z.object({
			page: z.number().default(0),
			search: z.string().optional(),
			filter: z.enum(["available", "subscribed"]).optional(),
		}),
	);

	let currentPage = $state(query.read("page"));
</script>

<svelte:head>
	<title>Users Cursi</title>
</svelte:head>

<Base>
	{#snippet left()}
		<Tabs.Root value="available">
			<Tabs.List class="w-full">
				<Tabs.Trigger class="w-full" value="available">Available</Tabs.Trigger>
				<Tabs.Trigger class="w-full" value="subscribe">Subscribed</Tabs.Trigger>
			</Tabs.List>
			<Tabs.Content value="available">
				<p class="text-sm text-muted-foreground">
					Search for available courses to subscribe to.
				</p>
			</Tabs.Content>
			<Tabs.Content value="subscribe">
				<p class="text-sm text-muted-foreground">
					Search for available courses to subscribe to.
				</p>
			</Tabs.Content>
		</Tabs.Root>
		<form>
			<Label for="search-cursus">Search</Label>
			<Input id="search-cursus" type="search" placeholder="Search for cursus" />
		</form>
	{/snippet}

	{#snippet right()}
		<div class="p-6">
			<menu class="flex justify-between">
				<h1 class="text-2xl font-bold">Cursus</h1>
				<Pagination
					variant="default"
					page={currentPage}
					onPage={(p) => query.write("page", p)}
				/>
			</menu>
			<Separator class="my-2" />
			<div class="flex flex-wrap gap-4">
				<Taskcard href="./cursus/1" type="goal" title="Cursus!" />
				<Taskcard href="./cursus/1" type="goal" title="Cursus!" />
				<Taskcard href="./cursus/1" type="goal" title="Cursus!" />
				<Taskcard href="./cursus/1" type="goal" title="Cursus!" />
				<Taskcard href="./cursus/1" type="goal" title="Cursus!" />
				<Taskcard href="./cursus/1" type="goal" title="Cursus!" />
			</div>
		</div>
	{/snippet}
</Base>
