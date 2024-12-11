<script lang="ts">
	import { page } from "$app/stores";
	import Pagination from "$lib/components/pagination.svelte";
	import Taskcard from "$lib/components/taskcard.svelte";
	import Input from "$lib/components/ui/input/input.svelte";
	import Label from "$lib/components/ui/label/label.svelte";
	import Separator from "$lib/components/ui/separator/separator.svelte";
	import * as Tabs from "$lib/components/ui/tabs/index";
	import { useQuery } from "$lib/utils/query.svelte";
	import { z } from "zod";

	const q = useQuery(
		$page.url.href,
		z.object({
			page: z.number().default(0),
			search: z.string().optional(),
			filter: z.enum(["available", "subscribed"]).optional(),
		}),
	);
</script>

<div class="grid h-full grid-cols-[0.20fr_1fr]">
	<aside class="bg-card flex flex-col gap-2 border-r p-6">
		<Tabs.Root value="available">
			<Tabs.List class="w-full">
				<Tabs.Trigger class="w-full" value="available">Available</Tabs.Trigger>
				<Tabs.Trigger class="w-full" value="subscribe">Subscribed</Tabs.Trigger>
			</Tabs.List>
			<Tabs.Content value="available">
				<p class="text-muted-foreground text-sm">
					Search for available courses to subscribe to.
				</p>
			</Tabs.Content>
			<Tabs.Content value="subscribe">
				<p class="text-muted-foreground text-sm">
					Search for available courses to subscribe to.
				</p>
			</Tabs.Content>
		</Tabs.Root>
		<form>
			<Label for="search-cursus">Search</Label>
			<Input id="search-cursus" type="search" placeholder="Search for cursus" />
		</form>
	</aside>
	<div class="p-6">
		<menu class="flex justify-between">
			<h1 class="text-2xl font-bold">Cursus</h1>
			<Pagination onPage={(p) => q.write("page", p)} />
		</menu>
		<Separator class="my-2" />
		<div class="flex flex-wrap gap-4">
			<Taskcard href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/cursus/1" type="goal" title="Cursus!" />
			<Taskcard href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/cursus/1" type="goal" title="Cursus!" />
			<Taskcard href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/cursus/1" type="goal" title="Cursus!" />
			<Taskcard href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/cursus/1" type="goal" title="Cursus!" />
			<Taskcard href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/cursus/1" type="goal" title="Cursus!" />
			<Taskcard href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/cursus/1" type="goal" title="Cursus!" />
		</div>
	</div>
</div>
