<script lang="ts">
	import { page } from "$app/stores";
	import Button from "$lib/components/buttons/button.svelte";
	import Card from "$lib/components/cards/card.svelte";
	import DemoInfo from "$assets/markdown/goal-demo.md?raw";
	import {
		ArchiveBox,
		Cube,
		Document,
		Icon,
		ShieldCheck,
		Trophy,
		UserGroup,
	} from "svelte-hero-icons";
	import "highlight.js/styles/github.css";
	import "katex/dist/katex.min.css";
	import type { PageServerData } from "./$types";
	import Projectcard from "$lib/components/cards/card-project.svelte";
	import MarkdownViewer from "$lib/components/markdown/markdown-viewer.svelte";
	import Accordion from "$lib/components/misc/accordion.svelte";

	export let data: PageServerData;
	let projects = data.goal.project_ids;
</script>

<svelte:head>
	<title>Goal: {$page.params.goal}</title>
	<meta name="[goal=slug]" content={$page.params.goal} />
</svelte:head>

<div class="container">
	<!-- Side Menu -->
	<Card style="height: min-content;">
		<!-- Grade -->
		<img
			loading="eager"
			class="logo"
			src="https://github.com/byaliego/42-project-badges/blob/main/badges/libfte.png?raw=true"
		/>
		<!-- Actions -->
		<hr />
		<form method="post" action="/">
			<Button style="width: 100%;" type="submit">
				<b>Register</b>
				<Icon src={ShieldCheck} size="18px" solid />
			</Button>
		</form>
	</Card>

	<!-- Body -->
	<div class="main-content">
		<Card>
			<!-- Team Title -->
			<h1 class="center">
				<Icon src={Trophy} size="32px" solid />
				Goal: {data.goal.name}
			</h1>

			<!-- Team Users -->

			{#if projects}
				<Accordion title="Projects" style="padding-inline: 8px;">
					<div class="list">
						{#each projects as i}
							<Projectcard
								href="/users/{$page.params.user}/projects/{i}"
								name="Project {i}"
							/>
						{/each}
					</div>
				</Accordion>
			{/if}
			<div class="entry">
				<hr />
				<MarkdownViewer md={DemoInfo} />
			</div>
		</Card>
	</div>
</div>

<style>
	.list {
		display: flex;
		flex-wrap: wrap;
		justify-content: space-around;
		margin-block: 16px;
		gap: 32px;

		@media only screen and (max-width: 768px) {
			justify-content: center;
		}
	}

	div.container {
		padding: 16px;
		gap: 1rem;
		margin-inline: auto;
		padding-inline: 24px;
		max-width: calc(var(--max-content-width) * 1.5);

		display: grid;
		grid-template-columns: 256px 1fr;

		/* The side */
		@media only screen and (max-width: 768px) {
			grid-template-columns: 1fr;
		}

		& img.logo {
			display: flex;
			flex-direction: column;
			justify-content: center;
			align-items: center;
			align-self: center;
			color: #fff;
			font-size: 2em;
			gap: 8px;
			border-radius: var(--br);
			width: 100%;
			object-fit: cover;
			object-position: center;

			background: linear-gradient(-45deg, #ee7752, #e73c7e, #23a6d5, #23d5ab);
			background-size: 400% 400%;
			animation: gradient 15s ease infinite;

			@keyframes gradient {
				0% {
					background-position: 0% 50%;
				}
				50% {
					background-position: 100% 50%;
				}
				100% {
					background-position: 0% 50%;
				}
			}
		}
	}

	.entry {
		padding-inline: 8px;

		& pre {
			display: grid;
			background-color: red;
		}

		& code {
			max-width: min-content;
		}
	}
</style>
