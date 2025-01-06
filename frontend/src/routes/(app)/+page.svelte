<script lang="ts">
	import SpotlightCard from "$lib/components/cards/spotlight-card.svelte";
	import Navgroup from "$lib/components/navgroup.svelte";
	import Link from "lucide-svelte/icons/link";
	import Github from "lucide-svelte/icons/github";
	import { useStorage } from "$lib/utils/local.svelte.js";
	import type { IconLink, NamedLink } from "$lib/types.js";
	import Base from "$lib/components/base.svelte";
	import Markdowny from "$lib/components/markdown/v2/markdowny.svelte";

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
		<div class="m-auto flex max-w-xl gap-2">
			<div class="flex-1">
				<Markdowny />
				Middle
				{#each Array(200).fill(0) as _, i}
					<div>{i + 1}</div>
				{/each}
			</div>
			<aside class="hidden lg:block">
				<SpotlightCard />
			</aside>
		</div>
	{/snippet}
</Base>
