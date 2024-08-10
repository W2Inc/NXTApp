<script lang="ts">
	import Input from "$lib/components/input/input.svelte";
	import Button from "$lib/components/buttons/button.svelte";
	import { Icon, Trophy } from "svelte-hero-icons";
	import MarkdownEditor from "$lib/components/markdown/markdown-editor.svelte";
	import AwaitForm from "$lib/components/misc/await-form.svelte";
	import NodeGraph from "$lib/components/node-graph/node-graph.svelte";

	let isLoading = false;
</script>

<svelte:head>
	<title>New Goal</title>
	<meta name="description" content="Create a new goal" />
</svelte:head>

<AwaitForm bind:isLoading method="POST" action="/new?/goal">
	<div>
		<h1 class="center">
			<Icon src={Trophy} size="32px" solid />
			Creating a new goal
		</h1>
		<hr />
	</div>

	<p>
		A learning goal is a goal you set for yourself, you can create your own goals and tie
		them to a project of your choosing. They can be big or small.
	</p>

	<Input
		required
		id="project-name"
		type="text"
		placeholder="Project name..."
		name="name"
		label="Name"
		title="The name of your goal"
		autocomplete="false"
	/>

	<label for="goal-description" title="Description of your goal">
		Description
		<MarkdownEditor
			required
			id="goal-description"
			name="description"
			placeholder="Project is about..."
		/>
	</label>

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
