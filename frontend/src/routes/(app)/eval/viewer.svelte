<script lang="ts">
	import { File, FileText, FileImage } from "lucide-svelte";
	import type { FileNode } from "./types";
	import Markdown from "$lib/components/markdown/markdown.svelte";

	export let file: FileNode | null = null;

	// Determine the file type based on extension
	function getFileType(
		extension: string | undefined,
	): "text" | "image" | "pdf" | "unknown" {
		if (!extension) return "unknown";

		const ext = extension.toLowerCase();
		if (["txt", "md", "js", "ts", "html", "css", "json", "svelte"].includes(ext)) {
			return "text";
		} else if (["png", "jpg", "jpeg", "gif", "webp", "svg"].includes(ext)) {
			return "image";
		} else if (ext === "pdf") {
			return "pdf";
		}

		return "unknown";
	}

	// Check if image might contain transparency
	function mightHaveTransparency(extension: string | undefined): boolean {
		if (!extension) return false;

		// File types that commonly support transparency
		return ["png", "svg", "webp", "gif"].includes(extension.toLowerCase());
	}
</script>

<div class="file-viewer">
	{#if file}
		{#if getFileType(file.extension) === "text"}
			{@render textViewer(file)}
		{:else if getFileType(file.extension) === "image"}
			{@render imageViewer(file)}
		{:else if getFileType(file.extension) === "pdf"}
			{@render pdfViewer(file)}
		{:else}
			{@render unknownFileViewer(file)}
		{/if}
	{:else}
		<div class="text-muted-foreground flex h-full items-center justify-center">
			<div class="text-center">
				<File class="mx-auto mb-4 h-12 w-12" />
				<p>Select a file to view its contents</p>
			</div>
		</div>
	{/if}
</div>

{#snippet textViewer(file: FileNode)}
	<div class="p-4">
		{#if file.extension === "md"}
			<Markdown variant="viewer" value={file.content || ""} />
		{:else}
			<pre class="bg-muted overflow-auto rounded-md p-4 whitespace-pre-wrap"><code
					>{file.content || ""}</code
				></pre>
		{/if}
	</div>
{/snippet}

{#snippet imageViewer(file: FileNode)}
	<div class="flex justify-center p-4">
		<div class={mightHaveTransparency(file.extension) ? "checkerboard-bg" : ""}>
			<img
				src={file.content || ""}
				alt={file.name}
				class="max-h-[calc(100vh-12rem)] max-w-full object-contain"
			/>
		</div>
	</div>
{/snippet}

{#snippet pdfViewer(file: FileNode)}
	<div class="h-full p-4">
		<iframe
			src={file.content || ""}
			title={file.name}
			class="h-[calc(100vh-12rem)] w-full rounded-md border-0"
		></iframe>
	</div>
{/snippet}

{#snippet unknownFileViewer(file: FileNode)}
	<div class="text-muted-foreground flex h-full items-center justify-center">
		<div class="text-center">
			<FileText class="mx-auto mb-4 h-12 w-12" />
			<p>Unable to preview this file type</p>
			<p class="text-sm">({file.extension || "unknown"} format)</p>
		</div>
	</div>
{/snippet}

<style>
	.checkerboard-bg {
		border-radius: 1rem;
		background-color: white;
		background-image:
			linear-gradient(45deg, #f0f0f0 25%, transparent 25%),
			linear-gradient(-45deg, #f0f0f0 25%, transparent 25%),
			linear-gradient(45deg, transparent 75%, #f0f0f0 75%),
			linear-gradient(-45deg, transparent 75%, #f0f0f0 75%);
		background-size: 20px 20px;
		background-position:
			0 0,
			0 10px,
			10px -10px,
			-10px 0px;
		display: inline-block; /* Only take up as much space as needed */
	}
</style>
