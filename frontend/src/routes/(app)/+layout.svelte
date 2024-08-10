<script lang="ts">
	import { afterNavigate, beforeNavigate } from "$app/navigation";
	import { recentNavs } from "$lib/stores/recent";
	import type { Link } from "$lib/types";
	import { page } from "$app/stores";
	import UserHeader from "$lib/components/header/user-header.svelte";
	import Header from "$lib/components/header/header.svelte";
	import { onMount } from "svelte";

	// Cache navigation routes
	afterNavigate((navigation) => {
		if (!navigation.to) return;
		if (
			navigation.to.url.host !== "github.com" &&
			navigation.to.url.host !== window.location.host
		)
			return;

		const { url } = navigation.to;
		const title = document.title;
		const link: Link = {
			title: title.charAt(0).toUpperCase() + title.slice(1),
			href: url.href,
		};

		if ($recentNavs.links.find((nav) => nav.href === link.href)) return;
		if ($recentNavs.links.length >= 5) $recentNavs.links.shift();
		recentNavs.update((value) => {
			return {
				links: [...value.links, link],
			};
		});
	});

	let headerOffset = 0;
	onMount(() => {
		const header = document.querySelector("header");
		if (header) headerOffset = header.getBoundingClientRect().height;
	});
</script>

<!--{#if $page.data.me}
	<UserHeader />
{:else}
	<Header />
{/if}

<main style="--header-height: {headerOffset}px;">
	<slot />
</main>-->

<slot />

<style>
	main {
		background: var(--background-color);
	}
</style>
