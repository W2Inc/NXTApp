<script lang="ts">
	import { page } from "$app/stores";
	import Button from "$lib/components/buttons/button.svelte";
	import CardComment from "$lib/components/cards/card-comment.svelte";
	import CardFeedback from "$lib/components/cards/card-comment.svelte";
	import Card from "$lib/components/cards/card.svelte";
	import Pagination from "$lib/components/input/pagination.svelte";
	import MarkdownEditor from "$lib/components/markdown/markdown-editor.svelte";
	import MarkdownViewer from "$lib/components/markdown/markdown-viewer.svelte";
	import AwaitForm from "$lib/components/misc/await-form.svelte";
	import TimlineItem from "$lib/components/misc/timline-item.svelte";
	import { ArchiveBox, ChatBubbleLeft, Icon } from "svelte-hero-icons";
	import type { PageServerData } from "./$types";

	export let data: PageServerData;
</script>

<svelte:head>
	<meta
		name="[id=uuid]"
		content="{data.review.reviewer.display_name} & {data.review.user.display_name}"
	/>
</svelte:head>

<div class="page">
	<h1>
		<div>
			<Icon src={ChatBubbleLeft} solid size="32px" />
			{data.review.reviewer.display_name}'s Review for {data.review.user.display_name}
		</div>
		<div>
			<Button href="/users/{data.review.user.id}/projects/{data.review.project_slug}">
				View Project
				<Icon src={ArchiveBox} solid size="18px" />
			</Button>
		</div>
	</h1>
	<hr />
	<ul>
		{#each data.comments as comment}
			<TimlineItem>
				<CardComment data={comment} editable={true} />
			</TimlineItem>
		{/each}
	</ul>

	<AwaitForm method="post" action="">
		<MarkdownEditor />
		<Button type="submit">Submit</Button>
	</AwaitForm>
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
			@media screen and (max-width: 700px) {
				padding-left: 0;
			}
		}
	}
</style>
