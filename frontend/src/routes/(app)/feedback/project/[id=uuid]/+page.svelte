<script lang="ts">
	import { page } from "$app/stores";
	import Button from "$lib/components/buttons/button.svelte";
	import CardFeedback from "$lib/components/cards/card-feedback.svelte";
	import Pagination from "$lib/components/input/pagination.svelte";
	import MarkdownEditor from "$lib/components/markdown/markdown-editor.svelte";
	import TimlineItem from "$lib/components/misc/timline-item.svelte";
	import type { PageServerData } from "./$types";

	export let data: PageServerData;
</script>

<svelte:head>
	<meta name="[id=uuid]" content="Reviews" />
</svelte:head>

<div class="page">
	<Pagination pages={2} />
	<hr />
	<ul>
		{#each data.reviews as review}
			<li>
				<CardFeedback {review} />
			</li>
		{/each}
	</ul>
</div>

<style>
	div.page {
		margin: 0 auto;
		max-width: 48rem;
		padding: 3rem 1.5rem;

		& ul {
			padding: 0;
			list-style: none;

			@media screen and (max-width: 700px) {
				padding-left: 0;
			}
		}
	}
</style>
