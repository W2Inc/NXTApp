<script lang="ts">
	import SpotlightCard from "$lib/components/cards/spotlight-card.svelte";
	import Navgroup from "$lib/components/navgroup.svelte";
	import Link from "lucide-svelte/icons/link";
	import Github from "lucide-svelte/icons/github";
	import { useStorage } from "$lib/utils/local.svelte.js";
	import type { IconLink, NamedLink } from "$lib/types.js";
	import Base from "$lib/components/base.svelte";
	import Markdown from "$lib/components/markdown/markdown.svelte";
	import FeedCard from "$lib/components/cards/feed-card.svelte";
	import ChangelogCard from "$lib/components/cards/changelog-card.svelte";

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

	//= ... =//
</script>

<svelte:head>
	<title>Home</title>
</svelte:head>

<!-- Side bar -->

<Base>
	{#snippet left()}
		<Navgroup title="Links" navs={navigations} />
		{#if recent.length > 0}
			<Navgroup title="Recently Visited" navs={recent} />
		{/if}
	{/snippet}

	{#snippet right()}
		<div class="w-full">
			<div class="flex gap-2 p-4 max-w-[80rem] mx-auto">
				<ul class="flex-auto">
					{#each data.feed as feed}
						<li>
							<FeedCard data={feed} />
						</li>
					{/each}
				</ul>
				<aside class="hidden min-w-[346px] xl:flex flex-col gap-2">
					<SpotlightCard />
					<ChangelogCard />
				</aside>
			</div>
		</div>
		<!-- <div class="m-auto flex max-w-xl gap-2">
			<div class="flex-1">
				<Markdown value="Hello World!" variant="viewer" />
				<Markdown value="Hello World!" variant="editor" />
				Middle
				{#each data.feed as feed}
					<div>1: {feed.id}</div>
				{/each}
			</div>
			<aside class="hidden lg:block">
				<SpotlightCard />
			</aside>
		</div> -->
	{/snippet}
</Base>
