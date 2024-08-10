<script lang="ts">
	import { goto, onNavigate } from "$app/navigation";
	import Navgroup from "$lib/components/links/navgroup.svelte";
	import type { Iconlink } from "$lib/types";
	import { redirect } from "@sveltejs/kit";
	import { ArchiveBox, User, PaintBrush, Cog6Tooth, AcademicCap, Beaker  } from "svelte-hero-icons";

	let recentNavigations: Iconlink[] = [
		{
			icon: User,
			link: { href: "./profile", title: "Profile" },
		},
		{
			icon: PaintBrush,
			link: { href: "./appearance", title: "Appearance" },
		},
		{
			icon: Beaker,
			link: { href: "./features", title: "Features" },
		},
	];
</script>

<div class="layout">
	<aside>
		<h1>Menu</h1>
		<Navgroup title="Profile Settings" navs={recentNavigations} />
	</aside>
	<!-- Page slot -->
	<div class="page">
		<slot />
	</div>
</div>

<style>
	div.layout {
		display: flex;
		flex-direction: row;
		max-width: calc(var(--max-content-width) * 1.5);
		margin: 0 auto;
		padding: 32px 16px;
		gap: 16px;

		@media (max-width: 768px) {
			flex-direction: column;
			margin: none;
		}
	}

	aside {
		top: 0;
		position: sticky;
		min-width: var(--nav-min-width);
		padding: 16px;
		height: 100%;

		@media (max-width: 768px) {
			position: unset;
			top: unset;
		}
	}

	div.page {
		flex: 1;
	}
</style>
