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
	import Tilter from "$lib/components/tilter.svelte";
	import Landing from "./landing.svelte";
	import { page } from "$app/state";

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
				<div class="mx-auto flex max-w-[80rem] gap-2 p-4">
					{#if data.feed.length == 0}
						<div
							class="flex w-full flex-col items-center justify-center p-12 text-lg text-gray-400 opacity-75"
						>
							<Tilter>
								<span class="mb-2 text-4xl">🤔</span>
							</Tilter>
							<p>Strange, there's nothing here?</p>
						</div>
					{:else}
						<ul class="flex-auto">
							{#each data.feed as feed}
								<li>
									<FeedCard data={feed} />
								</li>
							{/each}
						</ul>
					{/if}
					<aside
						class="sticky top-0 hidden h-min min-w-[346px] flex-col gap-2 pt-2 xl:flex"
					>
						<SpotlightCard />
						<ChangelogCard />
					</aside>
				</div>
			</div>
		{/snippet}
	</Base>
{:else}
	<Landing />
{/if}
