<script lang="ts">
	import Input from "$lib/components/input/input.svelte";
	import Select from "$lib/components/input/select.svelte";
	import CardOrg from "$lib/components/cards/card-org.svelte";
	import CardNotice from "$lib/components/cards/card-notice.svelte";
	import Button from "$lib/components/buttons/button.svelte";
	import { Icon, Plus, UserPlus } from "svelte-hero-icons";
	import Pagination from "$lib/components/input/pagination.svelte";
</script>

<div class="content">
	<aside>
		<Input id="search-orgs" list="projects-list" placeholder="Search Organizations ..." />
		<Pagination />
		<hr />
		<Button href="/pricing">
			Create Organization
			<Icon src={Plus} solid size="18px" />
		</Button>
		<Button href="/pricing">
			Join Organization
			<Icon src={UserPlus} solid size="18px" />
		</Button>
		<datalist>
			<option value="codam-coding-college">Codam</option>
		</datalist>
	</aside>

	<div class="page">
		<CardNotice type="info">
			<p>
				Organizations are groups of people that work together on projects.
				You can create an organization, or join an existing one.
			</p>
		</CardNotice>
		<hr />
		<CardOrg />
	</div>
</div>

<style>
	.content {
		display: grid;
		grid-template-columns: auto 1fr;
		grid-template-rows: 1fr;
		height: 100%;
		min-height: 100dvh;

		@media only screen and (max-width: 768px) {
			grid-template-columns: unset;
			grid-template-rows: unset;
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

		height: 100%;
		width: 100%;
		min-width: var(--nav-min-width);
		background-color: var(--nav-bg-color);
		border-right: 1px solid var(--border-color);
		box-shadow: var(--box-shadow);
		z-index: 1;

		@media only screen and (max-width: 768px) {
			top: unset;
			position: unset;
		}

		& nav {
			padding: 8px;
			border-radius: var(--br);
			border: 1px solid var(--border-color);
			box-shadow:
				inset 0 0 8px 1px var(--shade-01),
				inset 0 0 8px 1px var(--shade-01);
		}

		& .search-box {
			display: grid;
			grid-template-columns: 1fr;
			grid-template-rows: auto auto;
			gap: 8px;
		}

		& ul {
			list-style: none;
			grid-template-columns: 1fr auto;
			padding: 0;
			margin: 0;

			& li > * {
				display: flex;
				align-items: center;
				gap: 4px;
				color: var(--text-1);
				text-decoration: none;
				font-size: 14px;

				&:hover {
					color: var(--primary);
				}
			}
		}
	}

	.page {
		padding: 16px;
		margin-inline: auto;
		max-width: var(--max-content-width);

		& h1 {
			display: flex;
			align-items: center;
			justify-content: space-between;
		}

		& .list {
			display: flex;
			flex-wrap: wrap;
			justify-content: space-around;
			gap: 32px;

			@media only screen and (max-width: 768px) {
				justify-content: center;
			}
		}
	}
</style>
