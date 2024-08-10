<script lang="ts">
	import Logo from "$assets/logo.png";
	import {
		Icon,
		Bars3,
		ArrowLeftOnRectangle,
		Cog6Tooth,
		Plus,
		ArchiveBox,
		Trophy,
		Play,
		AcademicCap,
		Document,
	} from "svelte-hero-icons";
	import Imglink from "../links/imglink.svelte";
	import Breadcrumb from "../links/breadcrumb.svelte";
	import Overlay from "../dialogs/overlay.svelte";
	import Dropdown from "../input/dropdown.svelte";
	import Button from "../buttons/button.svelte";
	import Card from "../cards/card.svelte";
	import Search from "../input/search.svelte";
	import Profileimg from "../links/profileimg.svelte";
	import { page } from "$app/stores";
	import { dev } from "$app/environment";
	import Modal from "../dialogs/modal.svelte";

	let logoutModal: Modal;
	let overlay: Overlay;
	let user = $page.data.me!;
</script>

<Modal title="Logout" bind:this={logoutModal}>
	<form method="post" action="/auth/logout">
		<p>Are you sure you want to sign out?</p>
		<Button style="width: 100%;" type="submit">Yes</Button>
	</form>
</Modal>

<Overlay bind:this={overlay}>
	<form method="dialog">
		<Button type="submit">Ok</Button>
	</form>
</Overlay>

<header>
	<div class="start">
		<Button on:click={() => overlay.show()}>
			<Icon src={Bars3} solid size="24px" />
		</Button>
		<Imglink href="/" src={Logo} alt="logo" width={64} />
		<Breadcrumb />
	</div>

	<!-- Toolbar -->
	<div class="end">
		<div class="search center">
			<Search />
			<hr style="height: 32px;" />
		</div>

		<Dropdown toggle>
			<Card slot="summary" class="center" style="padding: 8px;">
				<Icon src={Plus} solid size="18px" />
			</Card>

			<div class="new-dropdown-body" style="right: -32px;">
				<a href="/new/rubric" title="Create a new rubric">
					<Icon src={Document} size="16px" />
					New Rubric
				</a>
				<a href="/new/project" title="Start a new project">
					<Icon src={ArchiveBox} size="16px" />
					New Project
				</a>
				<a href="/new/goal" title="Start a new learning goal">
					<Icon src={Trophy} size="16px" />
					New Goal
				</a>
				<a href="/new/cursus" title="Create your very own cursus">
					<Icon src={AcademicCap} size="16px" />
					New Cursus
				</a>

				<!-- <hr /> -->
				<!--<a href="/new/subscription" title="Subscribe to a new project, goal or cursus">
					<Icon src={DocumentPlus} size="16px" />
					Subscribe to ...
				</a>-->
			</div>
		</Dropdown>

		<Dropdown toggle>
			<span slot="summary" class="center">
				<Profileimg user={user.data} width="40" height="40" inert />
			</span>
			<div class="new-dropdown-body" style="right: -32px;">
				<a href="/settings">
					<Icon src={Cog6Tooth} solid size="16px" />
					Settings
				</a>
				{#if dev}
					<a href="/test">
						<Icon src={Play} solid size="16px" />
						Testing Components
					</a>
				{/if}
				<hr />
				<Button style="width: 100%; background-color: red;" on:click={logoutModal.show}>
					Sign out
					<Icon src={ArrowLeftOnRectangle} solid size="16px" />
				</Button>
			</div>
		</Dropdown>
	</div>
</header>

<style>
	.new-dropdown-body {
		width: 192px;
		position: absolute;
		padding: 8px;
		background: var(--shade-01);
		border: 1px solid var(--border-color);
		border-radius: var(--br);
		font-size: 0.9rem;

		/* TODO: FIX THIS!!!! DROPDOWN IS BROKEN! */
		right: -46px;

		& a {
			display: flex;
			align-items: center;
			gap: 4px;
			width: 100%;
			padding: 4px;

			border-radius: var(--br);
			color: var(--text-01);
			margin-bottom: 4px;
			text-decoration: none;

			&:last-child {
				margin-bottom: 0;
			}

			&:hover {
				color: var(--text-01);
				background: var(--shade-03);
			}
		}
	}

	header {
		width: 100%;
		display: flex;
		align-items: center;
		justify-content: space-between;
		padding: var(--padding);
		background-color: var(--nav-bg-color);
		border-bottom: 1px solid var(--border-color);
		box-shadow: var(--box-shadow);
		position: relative;
		z-index: 9999;
	}

	.start {
		display: flex;
		align-items: center;
		gap: 1em;
	}

	.end {
		display: flex;
		align-items: center;
		gap: 10px;

		& .search {
			@media only screen and (max-width: 768px) {
				display: none;
			}
		}
	}
</style>
