<script lang="ts">
	import Card from "$lib/components/cards/card.svelte";
	import SearchUser from "$lib/components/cards/search/search-user.svelte";
	import Input from "$lib/components/input/input.svelte";
	import Pagination from "$lib/components/input/pagination.svelte";
	import type { PageServerData } from "./$types";

	export let data: PageServerData;
</script>

<svelte:head>
	<title>Users</title>
	<meta name="description" content="Users" />
</svelte:head>

<div class="content">
	<!-- TODO: We really ought to share a single layout for this kinda shit -->
	<aside>
		<!--<Input id="search-orgs" list="users-list" placeholder="Search users ..." />
		<datalist>
			<option value="codam-coding-college">Codam</option>
		</datalist>-->
	</aside>

	<div class="page">
		<h1>
			Users
			<Pagination />
		</h1>
		<hr />
		<div class="users-container">
			{#each data.users as user}
				<Card>
					<SearchUser result={user} />
				</Card>
			{/each}
		</div>
	</div>
</div>

<style>

	h1 {
		display: flex;
		justify-content: space-between;
	}
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
		max-height: 100dvh;
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
	}

	.page {
		padding: 16px;
		/*margin-inline: auto;*/
		/*max-width: var(--max-content-width);*/

		& .users-container {
			display: grid;
			gap: 8px;
			grid-template-columns: repeat(auto-fill, minmax(400px, 1fr));

			@media only screen and (max-width: 768px) {
				grid-template-columns: 1fr;
			}
		}

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
