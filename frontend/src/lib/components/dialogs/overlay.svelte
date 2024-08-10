<script lang="ts">
	import Logo from "$assets/logo.png";
	import {
		Icon,
		XMark,
		ArchiveBox,
		Home,
		User,
		Trophy,
		Sparkles,
		PencilSquare,
		BuildingOffice2,
		RocketLaunch,
		Cog8Tooth,
		Users,
	} from "svelte-hero-icons";
	import Button from "../buttons/button.svelte";
	import Search from "../input/search.svelte";
	import Navgroup from "../links/navgroup.svelte";
	import type { Iconlink } from "$lib/types";
	import { page } from "$app/stores";
	import Link from "../links/link.svelte";
	import OrgContext from "../misc/org-context.svelte";

	let modal: HTMLDialogElement;

	export function show() {
		modal.showModal();
	}
	export function hide() {
		modal.close();
	}
	export function toggle() {
		modal.open ? close() : show();
	}

	const publicLinks: Iconlink[] = [
		//{
		//	icon: RocketLaunch,
		//	link: { href: "/feedback", title: "Explore Feedback" },
		//},
	];

	const contentLinks: Iconlink[] = [
		{
			icon: User,
			link: { href: `/users/${$page.data.me?.data.id}`, title: "Profile" },
		},
		{
			icon: ArchiveBox,
			link: { href: `/users/${$page.data.me?.data.id}/projects`, title: "Projects" },
		},
		{
			icon: Trophy,
			link: { href: `/users/${$page.data.me?.data.id}/goals`, title: "Goals" },
		},
		{
			icon: Sparkles,
			link: { href: `/users/${$page.data.me?.data.id}/graph`, title: "Galaxy" },
		},
	];

	const personalLinks: Iconlink[] = [
		{
			icon: Home,
			link: { href: "/", title: "Home" },
		},
		//{
		//	icon: BuildingOffice2,
		//	link: { href: "/organizations", title: "Organizations" },
		//},
		//{
		//	icon: PencilSquare,
		//	link: { href: "/edit", title: "Edit" },
		//},
		{
			icon: Cog8Tooth,
			link: { href: "/settings", title: "Settings" },
		},
		{
			icon: Users,
			link: { href: "/users", title: "Users" },
		},
	];
</script>

<dialog bind:this={modal}>
	<div class="modal-header">
		<img src={Logo} alt="NextDemy logo" width="64px" />
		<form method="dialog">
			<Button type="submit" value="close">
				<Icon src={XMark} size="18px" solid />
			</Button>
		</form>
	</div>

	<div class="search-input">
		<Search style="width: 100%;" />
		<!--{#if $page.data.me}
			<hr />
			<OrgContext user={$page.data.me.data} />
		{/if}-->
	</div>

	<nav>
		<Navgroup title="Personal" navs={personalLinks} />
		<Navgroup title="Profile" navs={contentLinks} />
		<!--<hr />
		<Navgroup navs={publicLinks} />-->
	</nav>

	<div class="footer">
		<p>Made with ❤️ by © 2023 Nextdemy, B.V.</p>
		<ul>
			<li><Link href="/terms">Terms</Link></li>
			<li><Link href="/privacy">Privacy</Link></li>
			<li><Link href="/pricing">Pricing</Link></li>
		</ul>
	</div>
</dialog>

<style>
	div.modal-header {
		display: flex;
		justify-content: space-between;
		align-items: center;
	}

	.search-input {
		display: none;

		@media screen and (max-width: 768px) {
			display: block;
		}
	}

	dialog {
		display: flex;
		flex-direction: column;
		user-select: none;
		position: fixed;

		min-height: 100%;
		min-width: 425px;

		padding: var(--padding);
		background-color: var(--shade-01);

		border: 1px solid var(--border-color);
		border-bottom-right-radius: var(--br);
		border-top-right-radius: var(--br);
		border-left: none;

		& nav {
			flex: 1;
		}

		& div.footer {
			display: flex;
			flex-direction: column;
			gap: 0.5rem;
			justify-content: center;
			padding: 0.5rem 0;

			& p {
				margin: 0;
				font-size: 0.75rem;
				color: var(--text-2);

				display: inline-flex;
				align-items: center;
			}

			& p span.power {
				display: inline-flex;
				margin: 0 0.25rem;
				width: 14px;
			}

			& ul {
				padding: 0;
			}

			& li {
				display: inline-block;
				margin-right: 0.5rem;
				font-size: 0.95rem;
			}
		}

		&:not([open]) {
			pointer-events: none;
			opacity: 0;
		}

		&::backdrop {
			backdrop-filter: blur(2px);
		}

		/* Media query that checks for width or height */
		@media screen and (max-width: 425px), screen and (max-height: 512px) {
			min-width: 100%;
			min-height: 100%;
			border-radius: 0;
			border-right: none;
		}
	}
</style>
