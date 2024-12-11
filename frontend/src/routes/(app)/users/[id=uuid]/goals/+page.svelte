<script lang="ts">
	import { page } from "$app/stores";
	import CircleHelp from "lucide-svelte/icons/circle-help";
	import Pagination from "$lib/components/pagination.svelte";
	import Taskcard from "$lib/components/taskcard.svelte";
	import Tippy from "$lib/components/tippy.svelte";
	import Input from "$lib/components/ui/input/input.svelte";
	import Label from "$lib/components/ui/label/label.svelte";
	import Separator from "$lib/components/ui/separator/separator.svelte";
	import { useQuery } from "$lib/utils/query.svelte";
	import { z } from "zod";
	import { useDebounce } from "$lib/utils/debounce.svelte";

	const debounce = useDebounce();
	const query = useQuery(
		$page.url.href,
		z.object({
			page: z.number().default(0),
			search: z.string().nullable(),
			filter: z.enum(["available", "subscribed"]).optional(),
		}),
	);


	//= Functions =//
	function setSearchQuery(value: string) {
		query.write("search", value.length > 0 ? value : null);
	}

	function setFilterQuery() {
		query.write("filter", "available");
	}
	//= Effects =//
</script>

<div class="grid sm:grid-cols-1 md:h-full md:grid-cols-[256px_1fr]">
	<aside class="bg-card flex flex-col gap-2 border-r p-6">
		<Label for="search-cursus">Search</Label>
		<Input
			id="search-cursus"
			type="search"
			placeholder="Search for cursus"
			oninput={(e) => debounce(setSearchQuery, e.currentTarget.value.trim())}
		/>

		<Label for="search-cursus">Search</Label>
		<Input
			id="search-cursus"
			type="search"
			placeholder="Search for cursus"
			oninput={(e) => debounce(setFilterQuery)}
		/>
	</aside>
	<div class="p-6">
		<nav>
			<ol class="flex flex-wrap items-center justify-between">
				<li>
					<Tippy
						text="Goals represent learning objectives that span across projects and curricula. They are
						standalone achievements that, once completed in any context, are validated across all
						curricula. This system ensures efficient learning by recognizing previously acquired
						skills across different educational paths."
					>
						<h1 class="flex items-center gap-2 text-2xl font-bold">
							Goals
							<CircleHelp size={16} />
						</h1>
					</Tippy>
				</li>
				<li>
					<Pagination onPage={(p) => query.write("page", p)} />
				</li>
			</ol>
		</nav>
		<Separator class="my-2" />
		<div class="flex flex-wrap gap-4">
			<Taskcard href="/users/" type="goal" title="Cursus!" />
			<Taskcard href="/users/" type="goal" title="Cursus!" />
			<Taskcard href="/users/" type="goal" title="Cursus!" />
			<Taskcard href="/users/" type="goal" title="Cursus!" />
			<Taskcard href="/users/" type="goal" title="Cursus!" />
			<Taskcard href="/users/" type="goal" title="Cursus!" />
		</div>
	</div>
</div>
