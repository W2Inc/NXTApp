<script lang="ts">
	import type { Iconlink } from "$lib/types";
	import { ArchiveBox, RocketLaunch, UserGroup } from "svelte-hero-icons";

	let navigation: Iconlink[] = [
		{
			icon: ArchiveBox,
			link: { href: `#`, title: "Projects" },
		},
		{
			icon: RocketLaunch,
			link: { href: `#`, title: "Goals" },
		},
		{
			icon: UserGroup,
			link: { href: `#`, title: "Users" },
		},
	];
</script>

<div class="content">
	<div/>
	<!-- <aside>
		<Navgroup title="Filters" navs={navigation} />
	</aside> -->
	<div class="page">
		<slot />
	</div>
</div>

<style>
	.page {
    padding: 16px;
    margin: 0 auto;
    max-width: 800px;
	}

	.content {
		display: grid;
		grid-template-columns: auto 1fr;
		grid-template-rows: 1fr;
		height: 100%;
		min-height: 100dvh;
		margin: 0 auto;

		@media only screen and (max-width: 768px) {
			grid-template-columns: unset;
			grid-template-rows: auto 1fr;
			height: unset;
			min-height: unset;
		}
	}

	aside {
		top: 0px;
		position: sticky;
		max-height: 100vh;
		display: flex;
		flex-direction: column;
		gap: 4px;
		padding: 16px;
		min-width: var(--nav-min-width);

		height: 100%;
		width: 100%;
		background-color: var(--nav-bg-color);
		border-right: 1px solid var(--border-color);
		box-shadow: var(--box-shadow);
		z-index: 1;

		@media only screen and (max-width: 768px) {
			top: unset;
			position: unset;
		}
	}
</style>
