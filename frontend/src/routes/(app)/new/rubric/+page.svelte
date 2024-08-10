<script lang="ts">
	import Input from "$lib/components/input/input.svelte";
	import Button from "$lib/components/buttons/button.svelte";
	import { Icon, Trophy } from "svelte-hero-icons";
	import MarkdownEditor from "$lib/components/markdown/markdown-editor.svelte";
	import AwaitForm from "$lib/components/misc/await-form.svelte";
	import Select from "$lib/components/input/select.svelte";
	import { damp } from "three/src/math/MathUtils.js";
	import type { PageServerData } from "./$types";

	let isLoading = false;
	export let data: PageServerData;
</script>

<svelte:head>
	<title>New Goal</title>
	<meta name="description" content="Create a new goal" />
</svelte:head>

<AwaitForm bind:isLoading method="POST" action="/new/rubric">
	<div>
		<h1 class="center">
			<Icon src={Trophy} size="32px" solid />
			Creating a new rubric
		</h1>
		<hr />
	</div>

	<p>
		A rubric is a set of criteria used to evaluate or grade a project, goal, or task. It
		is a scoring guide that seeks to evaluate a student's performance based on the sum of
		a full range of criteria rather than a single numerical score.
	</p>

	<Input
		required
		id="rubric-name"
		type="text"
		placeholder="Rubric name..."
		name="name"
		label="Name"
		title="The name of your rubric"
		autocomplete="false"
	/>

	<label for="rubric-project" title="The project this rubric is for">
		Project *
		<Select required id="rubric-project" name="projectID">
			{#each data.projects as project, i}
				<option selected={i === 0} value={project.id}>{project.name}</option>
			{/each}
		</Select>
	</label>

	<label for="rubric-type" title="The type of rubric">
		Review Type *
		<Select required id="rubric-type" name="type" title="The type of rubric">
			<option selected value="self">Self</option>
			<option value="async">Async</option>
			<option value="peer">Peer</option>
			<option value="auto">Automatic</option>
		</Select>
	</label>

	<label for="rubric-description" title="Description of your rubric">
		Description
		<MarkdownEditor
			required
			id="rubric-description"
			name="description"
			placeholder="Project is about..."
		/>
	</label>

	<hr />
	<div class="submit">
		<Button disabled={isLoading} style="flex: 1;" type="submit">Create</Button>
	</div>
</AwaitForm>

<style>
	div.submit {
		width: 100%;
		display: flex;

		& > * {
			flex: 1;
			width: 100%;
		}
	}
</style>
