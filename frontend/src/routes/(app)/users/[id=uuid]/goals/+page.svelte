<script lang="ts">
	import { page } from "$app/stores";
	import Pagination from "$lib/components/pagination.svelte";
	import Taskcard from "$lib/components/taskcard.svelte";
	import Input from "$lib/components/ui/input/input.svelte";
	import Label from "$lib/components/ui/label/label.svelte";
	import Separator from "$lib/components/ui/separator/separator.svelte";
	import * as Tabs from "$lib/components/ui/tabs/index";
	import { QueryEffect } from "$lib/query.svelte";
	import { z } from "zod";

	const queryEffect = new QueryEffect(
		$page.url.href,
		z.object({
			page: z.number().default(0),
			search: z.string().optional(),
			filter: z.enum(["available", "subscribed"]).optional(),
		}),
	);

	// The effect will automatically run when the value changes
	$effect(() => {
		console.log("Current page:", queryEffect.get("page"));
		console.log("Current search:", queryEffect.get("search"));
		console.log("Current filter:", queryEffect.get("filter"));
	});
</script>

<div class="grid h-full grid-cols-[0.20fr_1fr]">
	<aside class="bg-card flex flex-col gap-2 border-r p-6">
		<form>
			<Label for="search-cursus">Search</Label>
			<Input id="search-cursus" type="search" placeholder="Search for cursus" />
		</form>
	</aside>
	<div class="p-6">
		<menu class="flex justify-between">
			<h1>Goals</h1>
			<Pagination onPage={(p) => queryEffect.set("page", p)} />
		</menu>
		<Separator class="my-2" />
		<div class="flex flex-wrap gap-4">
			<Taskcard href="#" type="goal" title="Cursus!" />
			<Taskcard href="#" type="goal" title="Cursus!" />
			<Taskcard href="#" type="goal" title="Cursus!" />
			<Taskcard href="#" type="goal" title="Cursus!" />
			<Taskcard href="#" type="goal" title="Cursus!" />
			<Taskcard href="#" type="goal" title="Cursus!" />
		</div>
	</div>
</div>
