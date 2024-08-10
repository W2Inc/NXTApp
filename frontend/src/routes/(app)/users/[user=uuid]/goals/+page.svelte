<script lang="ts">
	import Button from "$lib/components/buttons/button.svelte";
	import Input from "$lib/components/input/input.svelte";
	import { Icon, Sparkles } from "svelte-hero-icons";
	import Projectcard from "$lib/components/cards/card-project.svelte";
	import type { LayoutServerData } from "./$types";
	import Select from "$lib/components/input/select.svelte";
	import Pagination from "$lib/components/input/pagination.svelte";

	let current = 0;
	export let data: LayoutServerData;
</script>

<svelte:head>
	<title>Goals Page: {current}</title>
</svelte:head>

<datalist id="learning-goals">
	{#each data.goals as learningGoal}
		<option value={learningGoal.name} />
	{/each}
</datalist>

<div class="content">
	<aside>
		<!--<div class="search-box">
			<div>
				<label for="goal-status">Select State</label>
				<Select id="goal-status">
					<option selected value="all">All</option>
					<option value="validated">Validated</option>
					<option value="in_progress">In progress</option>
					<option value="available">Available</option>
					<option value="unavailable">Unavailable</option>
					<option value="suggested">Suggested</option>
				</Select>
			</div>

			<div>
				<label for="cursus-selection">Select Cursus</label>
				<Select id="cursus-selection">
					<option value="core">Core</option>
					<option value="piscine">C Piscine</option>
				</Select>
			</div>
			<div class="center">
				<div style="flex: 1;">
					<Input id="search-goals" list="learning-goals" placeholder="Search goals ..." />
				</div>
				<Button href="/" style="padding: 4px 8px;">
					Galaxy
					<Icon src={Sparkles} size="24px" />
				</Button>
			</div>
		</div>-->
	</aside>

	<div class="page">
		<h1>
			My Goals
			<Pagination bind:current pages={3} />
		</h1>
		<hr />
		<div class="list">
			{#each data.goals as learningGoal}
				<Projectcard
					href="./goals/{learningGoal.id}"
					name={learningGoal.name}
				/>
			{/each}
		</div>
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
