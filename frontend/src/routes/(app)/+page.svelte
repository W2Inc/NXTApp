<script lang="ts">
	import "../../app.css";

	import SpotlightCard from "$lib/components/cards/spotlight-card.svelte";
	import Navgroup from "$lib/components/navgroup.svelte";
	import Link from "lucide-svelte/icons/link";
	import Github from "lucide-svelte/icons/github";
	import { useStorage } from "$lib/utils/local.svelte.js";
	import type { NamedLink } from "$lib/types.js";
	import Base from "$lib/components/base.svelte";
	import FeedCard from "$lib/components/cards/feed-card.svelte";
	import ChangelogCard from "$lib/components/cards/changelog-card.svelte";
	import { page } from "$app/state";
	import Empty from "$lib/components/empty.svelte";
	import Login from "./login.svelte";
	import { Constants } from "$lib/utils";
	import EntitySearch from "$lib/components/EntitySearch.svelte";
	import Button from "$lib/components/ui/button/button.svelte";
	import { enhance } from "$app/forms";
	import { RefreshCw } from "lucide-svelte";
	import { unstream } from "$lib/utils/stream.svelte";
	import Loader from "$lib/components/loader.svelte";

	const { data } = $props();
	const storage = useStorage();

	//= Links =//

	const links = storage.get<NamedLink[]>("app:history") ?? [];
	let recent = links.reverse().map((i) => {
		return { ...i, icon: Link };
	});

	let navigations = [
		{
			icon: Github,
			href: "#",
			title: "Our Github",
		},
	];
</script>

<svelte:head>
	<title>Home</title>
</svelte:head>

<!-- Side bar -->
{#if page.data.session}
	<Base>
		{#snippet left()}
			<Navgroup title="Links" navs={navigations} />
			{#if recent.length > 0}
				<Navgroup title="Recently Visited" navs={recent} />
			{/if}
		{/snippet}

		{#snippet right()}
			<div class="w-full">
				<div class="mx-auto flex max-w-7xl gap-2 p-4">
					<div class="w-full">
						{#await unstream(data.feed)}
							<Loader />
						{:then data}
							{@render feeds(data.items, data.page, data.pages)}
						{:catch error}
							<p>error {error.message}</p>
						{/await}
					</div>
					<aside class="sticky top-0 hidden h-min min-w-[346px] flex-col gap-2 xl:flex">
						{#await unstream(data.spotlights)}
							<Loader />
						{:then data}
							{@render spotlights(data)}
						{:catch error}
							<p>error {error.message}</p>
						{/await}
						<ChangelogCard />
					</aside>
				</div>
			</div>
		{/snippet}
	</Base>
{:else}
	<Login />
{/if}

{#snippet spotlights(data: BackendTypes["SpotlightEventDO"][])}
	{#if data.length}
		{#each data as spotlight}
			<SpotlightCard data={spotlight}/>
		{/each}
	{/if}
{/snippet}

{#snippet feeds(data: BackendTypes["FeedDO"][], page: number, pages: number)}
	{#if data.length}
		<ul class="flex-auto">
			{#each data as feed}
				<li>
					<FeedCard data={feed} />
				</li>
			{/each}
			{#if page < pages}
				<li>
					<Button class="w-full" variant="outline">Load More</Button>
				</li>
			{/if}
		</ul>
	{:else}
		<Empty />
	{/if}
{/snippet}
