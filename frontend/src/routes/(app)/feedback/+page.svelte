<script lang="ts">
	import { page } from "$app/stores";
	import CardFeedback from "$lib/components/cards/card-comment.svelte";
	import Card from "$lib/components/cards/card.svelte";
	import Pagination from "$lib/components/input/pagination.svelte";
	import MarkdownEditor from "$lib/components/markdown/markdown-editor.svelte";
	import MarkdownViewer from "$lib/components/markdown/markdown-viewer.svelte";
	import TimlineItem from "$lib/components/misc/timline-item.svelte";
	import FeedbackThread from "$lib/components/cards/card-feedback.svelte";
	import type { PageServerData } from "./$types";

	export let data: PageServerData;
</script>

<div class="page">
	<h1>
		All Reviews
		<Pagination pages={3} current={0} />
	</h1>
	<hr />
	<ul>
		{#each data.reviews as review}
			<li>
				<FeedbackThread {review} />
			</li>
		{/each}
	</ul>
</div>

<style>
	h1 {
		display: flex;
		justify-content: space-between;
		align-items: center;
	}

	div.page {
		margin: 0 auto;
		max-width: 48rem;
		padding: 3rem 1.5rem;

		& ul {
			list-style: none;
			padding: 0;
			@media screen and (max-width: 700px) {
				padding-left: 0;
			}
		}
	}
</style>
