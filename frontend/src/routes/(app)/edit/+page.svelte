<script lang="ts">
	import Input from "$lib/components/input/input.svelte";
	import Select from "$lib/components/input/select.svelte";
</script>

<div class="content">
	<aside>
		<div class="search-box">
			<div>
				<label for="cursus-selection">Cursus</label>
				<Select id="cursus-selection">
					<option selected value="core">Goals</option>
					<option value="core">Rubrics</option>
					<option value="core">Cursus</option>
					<option value="core">Projects</option>
				</Select>
			</div>

			<Input
				id="search-projects"
				list="projects-list"
				placeholder="Search projects ..."
			/>
		</div>
	</aside>

	<div class="page">
		<h1>Show everything!</h1>
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
