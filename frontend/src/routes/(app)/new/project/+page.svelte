<script lang="ts">
	import Input from "$lib/components/input/input.svelte";
	import { applyAction, enhance } from "$app/forms";
	import type { ActionData, PageServerData, SubmitFunction } from "../$types";
	import Button from "$lib/components/buttons/button.svelte";
	import { ArchiveBox, Icon } from "svelte-hero-icons";
	import Loading from "$lib/components/misc/loading.svelte";
	import Notice from "$lib/components/cards/card-notice.svelte";
	import Tags from "$lib/components/input/tags.svelte";
	import MarkdownEditor from "$lib/components/markdown/markdown-editor.svelte";
	import type { components } from "$lib/api/types";
	import { onMount } from "svelte";
	import toast from "sonner-svelte";
	import AwaitForm from "$lib/components/misc/await-form.svelte";

	// Private //
	let selectedTags: string[] = [];
	let isLoading = false;
</script>

<svelte:head>
	<title>New Project</title>
	<meta name="description" content="Create a new project" />
</svelte:head>

<AwaitForm bind:isLoading method="POST" action="/new?/project">
	<div>
		<h1 class="center">
			<Icon src={ArchiveBox} size="32px" solid />
			Creating a new project
		</h1>
		<hr />
	</div>

	<p>
		Projects are where the work happens. To learn something new you need to work. This can
		be working on simple exercises, but also on complex projects.
	</p>

	<p>You can link your project to a learning goal once it is created.</p>

	<Notice type="info">
		<p>
			Creating a project here will create a new repository on GitHub. You can then use
			this repository to store your project files. (Markdown, PDFs, etc.)
		</p>
	</Notice>

	<!--<Input
		id="project-img"
		type="file"
		placeholder="https://example.com/image.png"
		name="thumbnail"
		label="Thumbnail"
		title="Image of your project"
		accept="image/png, image/jpeg"
	/>-->

	<Input
		required
		id="project-name"
		type="text"
		placeholder="Mumi"
		name="name"
		label="Name"
		title="Name of your project"
	/>

	<label for="project-description" title="Description of your project">
		Description
		<MarkdownEditor
			required
			id="project-description"
			name="description"
			placeholder="Project is about..."
		/>
	</label>

	<label for="project-tags" title="Tags help categorize your project">
		Tags
		<Tags id="project-tags" bind:selectedTags />
		<input type="hidden" name="tags" bind:value={selectedTags} />
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
