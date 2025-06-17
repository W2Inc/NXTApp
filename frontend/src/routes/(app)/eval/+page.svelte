<script lang="ts">
	import Base from "$lib/components/base.svelte";
	import Markdown from "$lib/components/markdown/markdown.svelte";
	import RubricViewer from "./rubric.svelte";
	import * as Tabs from "$lib/components/ui/tabs";
	import * as Card from "$lib/components/ui/card";
	import type { VFSFile } from "$lib/components/fileviews/types";
	import VFS from "$lib/components/vfs/vfs.svelte";
	import Txt from "$lib/components/vfs/views/txt.svelte";
	import VfsView from "$lib/components/vfs/vfs-view.svelte";

	// Rubric markdown
	const rubricMarkdown = $state(`# Assignment Evaluation

## 1. Code Structure
Check if the code follows proper organization patterns.
- Files are organized logically
- Components are well-separated
- Imports are organized

## 2. Functionality
Verify that all required features work as expected.
- User can create new items
- Updates are reflected in the UI
- Deletion works properly

## 3. Performance
Evaluate the application's performance.
- Loads quickly
- Responds to user input without lag
- Handles large datasets efficiently

## 4. Documentation
Review documentation quality.
- Code is well-commented
- README explains how to run the project
- API endpoints are documented if applicable`);

	let selectedFile = $state<VFSFile | null>(null);

	// Example file structure using flat paths
	const files: VFSFile[] = [
		{
			path: "/project/src/main.ts",
			content: "console.log('Hello world');",
		},
		{
			path: "/project/src/README.md",
			content: "# Project Documentation\n\nThis is a sample project.",
		},
		{
			path: "/project/diagram.png",
			content:
				"https://png.pngtree.com/png-clipart/20240924/original/pngtree-d-red-fluffy-angry-emoji-intense-but-cute-anger-on-transparent-png-image_16081655.png",
		},
		{
			path: "/changelog/v1.0.0.md",
			content: "# API Documentation\n\nDetails about the API endpoints.",
		},
		{
			path: "/project/style.css",
			content: "body { font-family: sans-serif; }\n\nh1 { color: blue; }",
		},
		{
			path: "/project/dummy.pdf",
			content: "https://research.google.com/pubs/archive/44678.pdf",
		},
		{
			path: "/word/lel.doc",
			content: "http://ieee802.org:80/secmail/docIZSEwEqHFr.doc"
		},
		{
			path: "/word/lel2.docx",
			content: "http://newteach.pbworks.com/f/ele+newsletter.docx"
		},
	];
</script>

<Base variant="splitpane">
	{#snippet left()}
		<div class="m-4 space-y-4">
			<!-- Rubric Step Navigation - placed above tabs -->
			<RubricViewer currentStep={0} md={rubricMarkdown} />

			<Tabs.Root value="files">
				<Tabs.List class="w-full">
					<Tabs.Trigger class="w-full" value="files">Files</Tabs.Trigger>
					<Tabs.Trigger class="w-full" value="rubric">Rubric</Tabs.Trigger>
				</Tabs.List>
				<Tabs.Content value="files" class="h-[calc(100vh-20rem)]">
					<VFS
						{files}
						onFileSelect={(file) => {
							selectedFile = file;
						}}
					/>

					<!-- <FileTree {files} {selectedFile} on:fileSelect={handleFileSelect} /> -->
				</Tabs.Content>
				<Tabs.Content value="rubric">
					<Card.Root>
						<Card.Header>
							<Card.Title>Full Rubric</Card.Title>
							<Card.Description>Complete evaluation criteria.</Card.Description>
						</Card.Header>
						<Card.Content>
							<Markdown variant="viewer" value={rubricMarkdown} />
						</Card.Content>
					</Card.Root>
				</Tabs.Content>
			</Tabs.Root>
		</div>
	{/snippet}

	{#snippet right()}
		{#if selectedFile}
			<VfsView file={selectedFile} />
		{:else}
			<div class="flex items-center justify-center h-full">
				<Card.Root class="max-w-md">
					<Card.Header>
						<Card.Title>No File Selected</Card.Title>
					</Card.Header>
					<Card.Content>
						<Card.Description>
							Please select a file from the list to view its contents.
						</Card.Description>
					</Card.Content>
				</Card.Root>
			</div>
		{/if}
	{/snippet}
</Base>
