<script lang="ts">
	import { page } from "$app/stores";
	import Notice from "$lib/components/cards/card-notice.svelte";
	import { onMount } from "svelte";
	import type { PageServerData } from "./$types";
	import Card from "$lib/components/cards/card.svelte";
	import Searchcard from "$lib/components/cards/search/search-card.svelte";
	import type { components } from "$lib/api/types";
	import { afterNavigate, invalidate } from "$app/navigation";
	import Error from "$assets/textures/error.gif";

	export let data: PageServerData;

	// HACK: Svelte 5 will have a better way to do this
	let results: components["schemas"]["SearchResult"][];
	$: results = data.results as components["schemas"]["SearchResult"][];
</script>

<svelte:head>
	<title>Search: {$page.url.searchParams.get("q")}</title>
	<meta name="description" content="Search for projects, users, and more" />
</svelte:head>

{#if data.valid}
	<h1>Results for: {$page.url.searchParams.get("q")}</h1>
{:else}
	<h1>Invalid search query</h1>
{/if}

<Notice type="warning">
	<p>
		<strong>Warning:</strong> This page is still a work in progress and may not work as expected.
	</p>
</Notice>
<hr />

{#if data.valid}
	<div class="results">
		{#if results.length > 0}
			{#each results as result}
				<Searchcard data={result} type={result.type} />
			{/each}
		{:else}
		<div class="none">
				<strong>No results found</strong>
				<img src={Error} alt="Error" />
			</div>
		{/if}
	</div>
{/if}

<style>
	div.results {
		display: grid;
		gap: 8px;

		& .none {
			display: grid;
			justify-items: center;
			width: 100%;

			& img {
				border-radius: var(--br);
			}
		}
	}
</style>
