<script lang="ts">
	import { page } from "$app/stores";
	import Button from "$lib/components/buttons/button.svelte";
	import Input from "$lib/components/input/input.svelte";
	import Tags from "$lib/components/input/tags.svelte";
	import MarkdownEditor from "$lib/components/markdown/markdown-editor.svelte";
	import AwaitForm from "$lib/components/misc/await-form.svelte";
	import type { PageServerData } from "./$types";

	let selectedTags: string[] = [];
	export let data: PageServerData;
</script>

<div class="body">
	<h1>Edit project: {data.project.name}</h1>

	<AwaitForm method="post" action="/edit/project/{data.project.id}">
		<Input
			required
			id="project-name"
			type="text"
			placeholder="Mumi"
			name="name"
			label="Name"
			title="Name of your project"
			value={data.project.name}
		/>

		<label for="project-description" title="Description of your project">
			Description
			<MarkdownEditor
				required
				id="project-description"
				name="description"
				placeholder="Project is about..."
				md={data.project.subject_markdown}
			/>
		</label>

		<!--<Input
		id="project-img"
		type="file"
		placeholder="https://example.com/image.png"
		name="img"
		label="Thumbnail"
		title="Image of your project"
	/>-->

		<label for="project-tags" title="Tags help categorize your project">
			Tags
			<input type="hidden" name="tags" bind:value={selectedTags} />
			<Tags id="project-tags" bind:selectedTags />
		</label>
		<Button type="submit">Save</Button>
	</AwaitForm>
</div>

<style>
	div.body {
		margin: 0 auto;
		max-width: var(--max-content-width);
		padding: 3rem 1.5rem;
	}
</style>
