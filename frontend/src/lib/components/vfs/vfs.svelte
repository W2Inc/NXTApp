<script lang="ts">
	import { ChevronDown, File, Folder, Icon, Search, FileImage, FileText, Check } from "lucide-svelte";
	import { Input } from "../ui/input";
	import { Separator } from "../ui/separator";
	import { SvelteSet } from "svelte/reactivity";
	import { cn } from "$lib/utils";
	import type { VFSFile } from "./types";
	import Button from "../ui/button/button.svelte";

	interface Props {
		files?: VFSFile[];
		onFileSelect?: (file: VFSFile) => void | Promise<void>;
	}

	interface VFSNode {
		name: string;
		type: "file" | "directory";
		content?: string | URL;
		children: VFSNode[];
	}

	let selected = $state<VFSFile | null>(null);
	const { files = [], onFileSelect }: Props = $props();

	// ====== //

	function getIcon(file: VFSNode) {
		const parts = file.name.split(".");
		return iconMap[parts.pop() || ""] || File;
	}

	let search = $state("");
	const visibleFiles = $derived.by(() => {
		if (!search.trim()) return files;
		return files.filter((file) => file.path.toLowerCase().includes(search.toLowerCase()));
	});

	function build(files: VFSFile[]): VFSNode[] {
		const nodeMap: Record<string, VFSNode> = {
			"/": { name: "/", type: "directory", children: [] },
		};

		for (const file of files) {
			const path = file.path.startsWith("/") ? file.path : `/${file.path}`;
			if (path === "/") continue;

			const segments = path.split("/").filter(Boolean);
			const fileName = segments.pop() || "";

			let currentPath = "";
			let parent = nodeMap["/"];

			for (const segment of segments) {
				currentPath += `/${segment}`;
				if (!nodeMap[currentPath]) {
					nodeMap[currentPath] = { name: segment, type: "directory", children: [] };
					parent.children.push(nodeMap[currentPath]);
				}
				parent = nodeMap[currentPath];
			}

			parent.children.push({
				name: fileName,
				type: "file",
				content: file.content,
				children: [],
			});
		}

		Object.values(nodeMap).forEach((dir) => {
			dir.children.sort((a, b) =>
				a.type !== b.type
					? a.type === "directory"
						? -1
						: 1
					: a.name.localeCompare(b.name),
			);
		});

		return [nodeMap["/"]];
	}

	const vfs = $derived(build(visibleFiles));

	// ====== //

	const iconMap: Record<string, typeof Icon> = {
		"pdf": FileText,
		"doc": FileText,
		"docx": FileText,
		"png": FileImage
	};
</script>

<div id="vfs" class="file-tree">
	<Input
		type="text"
		icon={Search}
		placeholder="Search files..."
		bind:value={search}
		class="bg-background focus:ring-primary w-full rounded border py-1 pl-8 pr-2 text-sm focus:outline-none focus:ring-1"
	/>

	<Separator class="my-2" />

	<ul>
		{#if vfs.length > 0 && vfs[0].children.length > 0}
			{#each vfs[0].children as child, i}
				{@render renderNode(child, i)}
			{/each}
		{:else}
			<li class="text-muted-foreground p-4 text-center">
				{search ? "No matching files found" : "No files available"}
			</li>
		{/if}
	</ul>
</div>
{#snippet renderNode(node: VFSNode, index: number)}
	{#if node.type === "directory"}
		<li class="directory-item">
			<details open={search.length > 0}>
				<summary
					class="hover:bg-muted flex cursor-pointer select-none items-center rounded px-2 py-1 transition-colors"
				>
					<ChevronDown class="h-4 min-w-4 transition-transform" />
					<Folder class="h-4 min-w-4" />
					<span class="truncate font-medium">{node.name}</span>
				</summary>

				{#if node.children && node.children.length > 0}
					<ul class="border-muted ml-5 mt-1 border-l pl-2">
						{#each node.children as child, i}
							{@render renderNode(child, index + i)}
						{/each}
					</ul>
				{/if}
			</details>
		</li>
	{:else}
		{@const IconComponent = getIcon(node)}
		{@const isSelected = selected?.path.endsWith(node.name)}
		<li class="file-item group">
			<Button
				variant="ghost"
				class={cn(
					"hover:bg-muted w-full select-none justify-start shadow-none relative",
					isSelected && "bg-muted/70",
				)}
				onclick={() => {
					selected = files.find((f) => f.path.endsWith(node.name)) ?? null;
					onFileSelect?.(selected!);
				}}
			>
				<IconComponent class="h-4 min-w-4" />
				<span>{node.name}</span>
			</Button>
		</li>
	{/if}
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

	:global(.file-tree) {
		font-family: 'Geist Mono', 'JetBrains Mono', 'Menlo', 'Consolas', 'Liberation Mono', monospace;
	}
</style>
