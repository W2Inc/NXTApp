<script lang="ts">
	import Base from "$lib/components/base.svelte";
	import Markdown from "$lib/components/markdown/markdown.svelte";
	import * as Tabs from "$lib/components/ui/tabs";
	import * as Card from "$lib/components/ui/card";
	import FileTree from "./files.svelte";
	import FileViewer from "./viewer.svelte";
	import type { FileNode } from "./types";
	import Rubric from "./rubric.svelte";

	const value = $state("# Mhmmm");
	let selectedFile = $state<FileNode | null>(null);

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

	// Example file structure - replace with your actual data
	const files: FileNode[] = [
		{
			name: "Project",
			type: "directory",
			path: "/project",
			children: [
				{
					name: "src",
					type: "directory",
					path: "/project/src",
					children: [
						{
							name: "main.ts",
							type: "file",
							extension: "ts",
							path: "/project/src/main.ts",
							content: "console.log('Hello world');",
						},
						{
							name: "README.md",
							type: "file",
							extension: "md",
							path: "/project/src/README.md",
							content: "# Project Documentation\n\nThis is a sample project.",
						},
					],
				},
				{
					name: "diagram.png",
					type: "file",
					extension: "png",
					path: "/project/diagram.png",
					content:
						"https://png.pngtree.com/png-clipart/20240924/original/pngtree-d-red-fluffy-angry-emoji-intense-but-cute-anger-on-transparent-png-image_16081655.png", // In real usage, this would be a data URL or path
				},
			],
		},
	];

	function handleFileSelect(event: CustomEvent<FileNode>) {
		selectedFile = event.detail;
	}
</script>

<Base variant="splitpane">
	{#snippet left()}
		<div class="m-4 flex flex-col gap-2">
			<Rubric md={rubricMarkdown} currentStep={0} />
			<Tabs.Root value="files">
				<Tabs.List class="w-full">
					<Tabs.Trigger class="w-full" value="files">Files</Tabs.Trigger>
					<Tabs.Trigger class="w-full" value="rubric">Rubric</Tabs.Trigger>
				</Tabs.List>
				<Tabs.Content value="files" class="h-[calc(100vh-10rem)]">
					<FileTree {files} {selectedFile} on:fileSelect={handleFileSelect} />
				</Tabs.Content>
				<Tabs.Content value="rubric">
					<Card.Root>
						<Card.Header>
							<Card.Title>Rubric</Card.Title>
							<Card.Description>
								Evaluation criteria for the project submission.
							</Card.Description>
						</Card.Header>
						<Card.Content class="">
							<Markdown variant="viewer" {value} />
						</Card.Content>
					</Card.Root>
				</Tabs.Content>
			</Tabs.Root>
		</div>
	{/snippet}

	{#snippet right()}
		<Card.Root class="m-4 h-[calc(100dvh-2rem)]">
			<Card.Header>
				<Card.Title>{selectedFile?.name || "File Viewer"}</Card.Title>
				<Card.Description>
					{selectedFile?.path || "Select a file to view its contents"}
				</Card.Description>
			</Card.Header>
			<Card.Content class="h-[calc(100%-5rem)]">
				<FileViewer file={selectedFile} />
			</Card.Content>
		</Card.Root>
	{/snippet}
</Base>
