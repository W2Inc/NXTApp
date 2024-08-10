<script lang="ts">
	import Content from "$assets/markdown/terms.md?raw";
	import CardFeedback from "$lib/components/cards/card-feedback.svelte";
	import CardNotice from "$lib/components/cards/card-notice.svelte";
	import SearchProject from "$lib/components/cards/search/search-project.svelte";
	import Pagination from "$lib/components/input/pagination.svelte";
	import type { PageServerData } from "./$types";

	export let data: PageServerData;
</script>

<div>
	<h1>
		Recent Reviews
		<Pagination />
	</h1>
	<hr />
	<ul class:oops={data.reviews.length === 0}>
		{#if data.reviews.length === 0}
			<iframe
				width="560"
				height="315"
				src="https://www.youtube.com/embed/FeAY1__jE6E?si=FD5XC8JiPGoHjRuu"
				title="YouTube video player"
				frameborder="0"
				allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share"
			></iframe>
		{:else}
			{#each data.reviews as review}
				<li>
					<!--<CardFeedback data={review} />-->
				</li>
			{/each}
		{/if}
	</ul>
</div>

<style>
	div {
		margin: 0 auto;
		max-width: 48rem;
		padding: 3rem 1.5rem;
	}

	ul {
		list-style: none;
		padding: 0;

		&.oops {
			display: grid;
			place-items: center;
		}
	}

	h1 {
		display: flex;
		align-items: center;
		justify-content: space-between;
		padding: 0;
	}
</style>
