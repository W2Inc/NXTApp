<script lang="ts">
	import { ChevronDown, File, Folder, Search } from "lucide-svelte";
	import type { FileNode } from "./types";

	interface Props {
		files: FileNode[];
		selectedFile: FileNode | null;
	}

	let { files, selectedFile }: Props = $props();

	let searchTerm = $state("");

	function handleFileClick(file: FileNode) {
		selectedFile = file;
		dispatch("fileSelect", file);
	}

	// Define custom event dispatcher
	import { createEventDispatcher } from "svelte";
	import { cn } from "$lib/utils";
	import Input from "$lib/components/ui/input/input.svelte";
	import Separator from "$lib/components/ui/separator/separator.svelte";
	const dispatch = createEventDispatcher<{
		fileSelect: FileNode;
	}>();

	// File icon mapping based on extension
	function getFileIcon(extension: string | undefined) {
		// You can extend this with more file type icons
		const iconMap: Record<string, typeof File> = {
			// Add more mappings as needed
		};

		return iconMap[extension?.toLowerCase() || ""] || File;
	}

	// Recursive search function to filter files and directories
	function filterTree(nodes: FileNode[], term: string): FileNode[] {
		if (!term.trim()) return nodes;

		return nodes
			.map((node) => {
				if (node.name.toLowerCase().includes(term.toLowerCase())) {
					return node;
				}

				if (node.type === "directory" && node.children) {
					const filteredChildren = filterTree(node.children, term);
					if (filteredChildren.length > 0) {
						return {
							...node,
							children: filteredChildren,
						};
					}
				}

				return null;
			})
			.filter(Boolean) as FileNode[];
	}

	const filteredFiles = $derived(filterTree(files, searchTerm));
</script>

<div class="file-tree">
	<Input
		type="text"
		icon={Search}
		placeholder="Search files..."
		bind:value={searchTerm}
		class="bg-background focus:ring-primary w-full rounded border py-1 pr-2 pl-8 text-sm focus:ring-1 focus:outline-none"
	/>

	<Separator class="my-2" />

	<ul class="file-tree-list">
		{#if filteredFiles.length > 0}
			{#each filteredFiles as file}
				{@render fileOrDirectory(file)}
			{/each}
		{:else}
			<li class="text-muted-foreground p-4 text-center">
				{searchTerm ? "No matching files found" : "No files available"}
			</li>
		{/if}
	</ul>
</div>

{#snippet fileOrDirectory(node: FileNode)}
	{#if node.type === "directory"}
		{@render directoryItem(node)}
	{:else}
		{@render fileItem(node)}
	{/if}
{/snippet}

{#snippet fileItem(file: FileNode)}
	<li class="file-item select-none">
		<button
			class={cn(
				"hover:bg-muted flex w-full items-center gap-2 rounded px-2 py-1 text-left",
				selectedFile?.path === file.path && "bg-muted",
			)}
			on:click={() => handleFileClick(file)}
		>
			<svelte:component this={getFileIcon(file.extension)} class="h-4 w-4" />
			<span class="truncate">{file.name}</span>
		</button>
	</li>
{/snippet}

{#snippet directoryItem(dir: FileNode)}
	<li class="directory-item select-none">
		<details open>
			<summary
				class="hover:bg-muted flex cursor-pointer items-center gap-2 rounded px-2 py-1"
			>
				<ChevronDown class="h-4 w-4 transition-transform" />
				<Folder class="h-4 w-4" />
				<span class="font-medium">{dir.name}</span>
			</summary>

			{#if dir.children && dir.children.length > 0}
				<ul class="border-muted mt-1 ml-4 border-l pl-2">
					{#each dir.children as child}
						{@render fileOrDirectory(child)}
					{/each}
				</ul>
			{/if}
		</details>
	</li>
{/snippet}

<style>
	:global(.file-tree details > summary::-webkit-details-marker) {
		display: none;
	}

	:global(.file-tree details[open] > summary .lucide-chevron-down) {
		transform: rotate(0deg);
	}

	:global(.file-tree details:not([open]) > summary .lucide-chevron-down) {
		transform: rotate(-90deg);
	}
</style>
