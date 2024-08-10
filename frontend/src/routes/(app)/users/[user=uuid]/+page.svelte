<script lang="ts">
	import { page } from "$app/stores";
	import type { Plugin } from "svelte-exmarkdown";
	import Card from "$lib/components/cards/card.svelte";
	import Textarea from "$lib/components/input/textarea.svelte";
	import Markdown from "svelte-exmarkdown";
	import { gfmPlugin } from "svelte-exmarkdown/gfm";
	import rehypeHighlight from "rehype-highlight";
	import rehypeKatex from "rehype-katex";
	import remarkMath from "remark-math";
	import "highlight.js/styles/github.css";
	import "katex/dist/katex.min.css";
	import BG from "$assets/ProfileBG.gif";
	import Imglink from "$lib/components/links/imglink.svelte";
	import XLogo from "$assets/github/github-mark-white.svg";
	import MLogo from "$assets/socials/MastadonLogo.svg";
	import Button from "$lib/components/buttons/button.svelte";
	import { onMount } from "svelte";
	import { ArchiveBox, Home, Icon, Sparkles, Trophy, UserGroup } from "svelte-hero-icons";
	import Navgroup from "$lib/components/links/navgroup.svelte";
	import type { Iconlink } from "$lib/types";
	import type { LayoutData, PageData } from "./$types";
	import MarkdownViewer from "$lib/components/markdown/markdown-viewer.svelte";
	import IconLinkedin from "$lib/icons/icon-linkedin.svelte";
	import IconTwitter from "$lib/icons/icon-twitter.svelte";
	import IconFacebook from "$lib/icons/icon-facebook.svelte";
	import IconGithub from "$lib/icons/icon-github.svelte";
	import DemoInfo from "$assets/markdown/profile-demo.md?raw";

	export let data: PageData;

	const { user } = data;
	const navigation: Iconlink[] = [
		{
			icon: ArchiveBox,
			link: { href: `/users/${user.id}/projects`, title: "Projects" },
		},
		{
			icon: Trophy,
			link: { href: `/users/${user.id}/goals`, title: "Goals" },
		},
		{
			icon: Sparkles,
			link: { href: `/users/${user.id}/graph`, title: "Galaxy" },
		},
		{
			icon: UserGroup,
			link: { href: `/users/feedback?user=${user.id}`, title: "Reviews" },
		},
	];
	const socials = [
		{
			icon: IconGithub,
			link: user.details.github_url,
		},
		{
			icon: IconLinkedin,
			link: user.details.linkedin_url,
		},
		{
			icon: IconTwitter,
			link: user.details.twitter_url,
		},
		{
			icon: IconFacebook,
			link: user.details.facebook_url,
		},
	];
</script>

<svelte:head>
	<title>{user.data.display_name}</title>
	<meta name="description" content="Page of {user.data.display_name}" />
</svelte:head>

<div class="parent">
	<Card style="backdrop-filter: blur(4px); background-image: url({BG});">
		<div class="banner">
			<img
				src={user.details.avatar_url ?? "https://avatars.githubusercontent.com/u/63303990?v=4"}
				alt="avatar"
				height="128"
				width="128"
				style="border-radius: var(--br);"
			/>
			<div class="content">
				<h1>{user.data.display_name} <span class="display">@{user.data.login}</span></h1>
				<!--<menu>
					{#each socials as social}
						{#if social.link}
							<li>
								<a href={social.link} target="_blank" rel="noopener noreferrer">
									<svelte:component this={social.icon} />
								</a>
							</li>
						{/if}
					{/each}
				</menu>-->
			</div>
		</div>
	</Card>
	<hr />
	<div class="body">
		<aside>
			<Navgroup navs={navigation} />
		</aside>
		<div class="content">
			<Card>
				<MarkdownViewer md={DemoInfo} />
			</Card>
		</div>
	</div>
</div>

<style>
	.body {
		gap: 8px;
		display: grid;
		grid-template-columns: 25% 1fr;

		@media screen and (max-width: 768px) {
			grid-template-columns: 1fr;
			grid-template-rows: auto 1fr;
		}

		& aside nav {
			display: flex;
			flex-direction: column;
			border-radius: var(--br);
			background: var(--shade-01);
			border: 1px solid var(--border-color);
			padding: 8px;

			& a {
				padding: 8px 16px;
				border-radius: var(--br);
				backdrop-filter: blur(4px);
				color: var(--text-color);
				text-decoration: none;
				transition: none;

				&:hover {
					background: var(--shade-03);
				}

				&[data-selected="true"] {
					background: var(--primary);
					/*box-shadow: inset 0 -2px 0 var(--primary);*/
					/*box-shadow: inset 0 0 0 2px var(--primary);*/
				}
			}
		}

		& .content {
			width: 100%;
		}
	}

	.parent {
		margin: 0 auto;
		padding: 20px 16px 0;
		max-width: calc(var(--max-content-width) * 1.5);

		& .banner {
			display: flex;
			margin: 0 auto;
			padding: 16px;
			gap: 16px;
			background: hsla(0, 0%, 10%, 0.75);
			width: 100%;
			height: 100%;
			border-radius: var(--br);
			backdrop-filter: blur(4px);

			& .content {
				display: flex;
				flex-direction: column;
				gap: 8px;
				flex: 1;
			}

			& menu {
				list-style: none;
				display: inline-flex;
				align-items: center;
				min-width: 0;
				gap: 8px;

				& li > * {
					min-width: 0;
				}

				& a {
					color: white;
				}
			}

			& h1 {
				margin: 0;
				padding: 0;
				font-size: 1.55rem;
				font-family: Monaspace;
				font-weight: 600;
				color: white;
				line-height: 1;

				& span.display {
					font-size: 1rem;
					font-weight: 400;
					color: var(--text-4);
				}

				@media screen and (max-width: 768px) {
					font-size: 1.5rem;
				}
			}
		}
	}
</style>
