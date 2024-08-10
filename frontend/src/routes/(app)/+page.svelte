<script lang="ts">
	import { page } from "$app/stores";
	import Galaxycard from "$lib/components/cards/card-galaxy.svelte";
	import DefaultPage from "$lib/components/homepage.svelte";
	import type { PageServerData } from "./$types";
	import Card from "$lib/components/cards/card.svelte";
	import Event from "$lib/components/cards/card-notification.svelte";
	import BG from "$assets/textures/bg.png";
	import Navgroup from "$lib/components/links/navgroup.svelte";
	import { CodeBracketSquare, UserGroup, Link as LinkIcon } from "svelte-hero-icons";
	import type { Iconlink } from "$lib/types";
	import OrgContext from "$lib/components/misc/org-context.svelte";
	import { onMount, setContext } from "svelte";
	import { recentNavs } from "$lib/stores/recent";
	import TimlineItem from "$lib/components/misc/timline-item.svelte";
	import Link from "$lib/components/links/link.svelte";
	import { PUBLIC_VERSION } from "$env/static/public";
	import Observer from "$lib/components/misc/fade/observer.svelte";
	import transformAPIData from "$lib/graph/builder";

	export let data: PageServerData;

	let navigation: Iconlink[] = [
		{
			icon: CodeBracketSquare,
			link: { href: "https://github.com/Nextdemy", title: "Github" },
		},
		{
			icon: UserGroup,
			link: { href: "https://github.com/orgs/Nextdemy/discussions", title: "Community" },
		},
	];

	let recent: Iconlink[];
	recentNavs.subscribe((value) => {
		recent = value.links.map((link) => {
			return { icon: LinkIcon, link };
		});
	});
</script>

<svelte:head>
	<title>0000000000000000000000000.com</title>
	<meta name="description" content="25x0.com - A Curiosity driven Learning Platform" />
</svelte:head>

{#if $page.data.me}
	<div class="page">
		<!-- Action navbar -->
		<aside class="actionbar">
			<!--<OrgContext user={$page.data.me.data} />-->

			<!-- TODO: Figure out what links we should add here that serve as shortcuts -->
			<Navgroup title="Social" navs={navigation} />
			{#if $recentNavs.links.length > 0}
				<Navgroup title="Recent" navs={recent} />
			{/if}
		</aside>

		<!-- Body -->
		<div class="body">
			<div class="content">
				<div class="activity">
					<!-- TODO: Fill with Activity -->
					<Galaxycard />
				</div>

				<!-- Notifications, Events, ... -->
				<aside class="notificationbar">
					<Card>
						<h1>Version: {PUBLIC_VERSION}</h1>
						<hr />
						<ul>
							<li> Added a new <Link href="/">cursus</Link></li>
							<li> Added a new <Link href="/">project</Link></li>
							<li> Removed <Link href="/">Herobrine</Link></li>
						</ul>
					</Card>
					<Event title="Example Event" BGUrl={BG} />
				</aside>
			</div>
		</div>
	</div>
{:else}
	<DefaultPage graphData={transformAPIData(data.graph)} />
{/if}

<style>
	.page {
		display: grid;
		grid-template-columns: auto 1fr;
		min-height: 100vh;

		@media only screen and (max-width: 768px) {
			display: block;
		}
	}

	div.page div.body {
		padding: 16px;
	}

	div.content {
		flex: 1;

		display: grid;
		grid-template-columns: 1fr 340px;

		margin-inline: auto;
		padding-inline: 24px;
		max-width: 1400px;
		gap: 16px;

		& .activity {
			flex: 1;
			display: flex;
			flex-direction: column;
			gap: 16px;
		}

		& aside.notificationbar {
			gap: 16px;
			display: flex;
			flex-direction: column;
		}

		@media only screen and (max-width: 1600px) {
			grid-template-columns: 1fr;
			& aside.notificationbar {
				display: none;
			}
		}
	}

	aside.actionbar {
		top: 0px;
		position: sticky;
		height: 100vh;
		background-color: var(--nav-bg-color);
		box-shadow: var(--box-shadow);
		border-right: 1px solid var(--border-color);
		min-width: var(--nav-min-width);
		padding: 16px;

		@media only screen and (max-width: 768px) {
			display: none;
		}
	}
</style>
