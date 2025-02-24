<script lang="ts">
	import SpotlightCard from "$lib/components/cards/spotlight-card.svelte";
	import Navgroup from "$lib/components/navgroup.svelte";
	import Link from "lucide-svelte/icons/link";
	import Github from "lucide-svelte/icons/github";
	import { useStorage } from "$lib/utils/local.svelte.js";
	import type { IconLink, NamedLink } from "$lib/types.js";
	import Base from "$lib/components/base.svelte";
	import Markdown from "$lib/components/markdown/markdown.svelte";

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
		<form method="post">
			<button type="submit">Wtf</button>
		</form>

		<div class="m-auto flex max-w-xl gap-2">
			<div class="flex-1">
				<Markdown value="Hello World!" variant="viewer" />
				<Markdown value="Hello World!" variant="editor" />
				Middle
				{#each data.feed as feed}
					<div>{feed.id}</div>
				{/each}
			</div>
			<aside class="hidden lg:block">
				<SpotlightCard />
			</aside>
		</div>
	{/snippet}
</Base>
