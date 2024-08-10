<script lang="ts">
	import { applyAction, enhance } from "$app/forms";
	import toast from "sonner-svelte";
	import type { SubmitFunction } from "../$types";
	import { ArchiveBox, Icon } from "svelte-hero-icons";
	import AwaitForm from "$lib/components/misc/await-form.svelte";
	import Select from "$lib/components/input/select.svelte";
	import type { PageServerData } from "./$types";
	import Pagination from "$lib/components/input/pagination.svelte";
	import SearchGoal from "$lib/components/cards/search/search-goal.svelte";
	import SearchProject from "$lib/components/cards/search/search-project.svelte";

	let isLoading = false;
	let selectedSubscription: string = "goal";
	export let data: PageServerData;

	$: {
		console.log("selectedSubscription", selectedSubscription);
	}
</script>

<svelte:head>
	<title>Subscribe</title>
	<meta name="description" content="Subscribe to a project" />
</svelte:head>

<AwaitForm bind:isLoading method="POST" action="/new?/project">
	<div>
		<h1 class="center">
			<Icon src={ArchiveBox} size="32px" solid />
			Subscribing to a new project
		</h1>
		<hr />

		<label for="join-new-:entity:">Type</label>
		<Select bind:selected={selectedSubscription} id="join-new-:entity:">
			<option selected value="goal">Goal</option>
			<option value="project">Project</option>
		</Select>
		<hr />
		<Pagination />

		{#if selectedSubscription === "goal"}
			<SearchGoal result={data.goals[0]}/>
		{:else if selectedSubscription === "project"}
			<SearchProject result={data.projects[0]}/>
		{:else}
			<p>Please select a subscription type</p>
		{/if}

	</div>
</AwaitForm>

<style>

</style>
