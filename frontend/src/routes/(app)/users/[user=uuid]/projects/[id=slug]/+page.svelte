<script lang="ts">
	import { page } from "$app/stores";
	import Button from "$lib/components/buttons/button.svelte";
	import Card from "$lib/components/cards/card.svelte";
	import FeedbackThread from "$lib/components/cards/card-feedback.svelte";
	import Feedback from "$lib/components/cards/card-comment.svelte";
	import Input from "$lib/components/input/input.svelte";
	import Link from "$lib/components/links/link.svelte";
	import Profileimg from "$lib/components/links/profileimg.svelte";
	import {
		Icon,
		ArchiveBox,
		Cube,
		Document,
		UserGroup,
		ShieldCheck,
		ChatBubbleLeft,
	} from "svelte-hero-icons";
	import type { PageServerData } from "./$types";
	import Meter from "$lib/components/meters/meter.svelte";
	import MarkdownViewer from "$lib/components/markdown/markdown-viewer.svelte";
	import IconGithub from "$lib/icons/icon-github.svelte";

	export let data: PageServerData;
</script>

<div class="container">
	<Card style="height: min-content;">
		<div class="status">
			<img
				loading="eager"
				class="logo"
				height="256px"
				alt="thumbnail"
				src="https://github.com/byaliego/42-project-badges/blob/main/badges/libfte.png?raw=true"
			/>
		</div>

		<!-- Actions -->
		<hr />
		<!--<span class="label">Likes / Dislikes</span>
		<LikeDislike likes={420} dislikes={169} />-->
		<span class="label">Evaluation Types</span>
		<Meter
			self={data.reviewMakeup.self}
			tester={data.reviewMakeup.auto}
			remote={data.reviewMakeup.async}
			onsite={data.reviewMakeup.peer}
		/>

		<!-- Fixed Links -->
		<hr />
		<Link href="/">
			<Icon src={Cube} size="20px" solid />
			<span>Project</span>
		</Link>
		<br />
		<Link href="/">
			<Icon src={ArchiveBox} size="20px" solid />
			<span>Resources</span>
		</Link>

		<!-- Project custom links -->
		<hr />
		<Link href="/">
			<Icon src={Document} size="20px" solid />
			<span>Custom Resource</span>
		</Link>
	</Card>

	<!-- Body -->
	<div>
		<!-- Do I have a session? -->
		{#if $page.data.me}
			<Card>
				<!-- Team Title -->
				<h1 class="center">
					<Icon src={UserGroup} size="32px" solid />
					{data.user.data.display_name}'s {data.project.name}
				</h1>

				<!-- Team Users -->
				<div class="center">
					{#each data.usersProject.users as user}
						<Profileimg
							userID={user.id}
							avatar_url={user.avatar_url}
							width="40"
							height="40"
						/>
					{/each}
				</div>

				<hr />

				<div class="entry">
					<h4 class="review-heading">
						<div class="center">
							<Icon src={ShieldCheck} size="24px" solid />
							<span>REVIEWS: {data.reviews.length}</span>
						</div>
						<Button href="/feedback/project/{data.usersProject.id}">
							View all Reviews
							<Icon src={ChatBubbleLeft} size="18px" solid />
						</Button>
					</h4>

					<div class="corrections">
						{#each data.reviews as review}
							{#if review.reviewer}
								<FeedbackThread {review} />
							{/if}
						{/each}
					</div>
					<!--<Pagination />-->

					<hr />
					<div class="center" style="align-items: unset;">
						<!-- Thats my project! -->
						<Button style="background-color: var(--primary); color: white;" href={`${$page.url.toString()}/review`}>
							{#if data.user.id === $page.data.me.data.id}
								<b>Request a review</b>
							{:else}
								<b>Create a review</b>
							{/if}
							<Icon src={ShieldCheck} size="18px" solid />
						</Button>
						<hr />
						<!--Not me but I have a session -->
						<Button href={data.usersProject.repo_url}>
							<b>View Github</b>
							<IconGithub size={18} />
						</Button>

						<!-- TODO: Add API to launch a codespace session directly here! -->
						<!--<Button href="">
							<b>Open Codespaces</b>
							<Icon src={CodeBracketSquare} size="18px" solid />
						</Button>-->
					</div>
				</div>
			</Card>
		{/if}

		<hr />
		<Card>
			<!--<details>-->
			<!--<summary>Subject</summary>-->
			<!--<Card>-->
			<MarkdownViewer md={data.project.subject_markdown} />
			<!--</Card>-->
			<!--</details>-->
		</Card>
	</div>
</div>

<style>
	@media only screen and (min-width: 768px) {
		& div.status {
			display: none;
			font-size: 1em;
		}
	}

	span.label {
		font-size: 0.75em;
		color: var(--text-3);
	}

	h4.review-heading {
		display: flex;
		justify-content: space-between;
		align-items: center;
	}

	div.container {

		gap: 16px;
		display: grid;
		grid-template-columns: 256px 1fr;

		/* The side */
		@media only screen and (max-width: 768px) {
			grid-template-columns: 1fr;
		}

		& div.status {
			display: flex;
			flex-direction: column;
			justify-content: center;
			align-items: center;
			align-self: center;
			color: #fff;
			font-size: 2em;
			gap: 16px;
			border-radius: var(--br);
			/*background-color: rgb(190, 0, 0);*/
			width: 100%;
			height: 190px;
		}

		& .corrections {
			display: flex;
			flex-direction: column;
			gap: 16px;
			margin-bottom: 8px;
		}
	}
</style>
