<script lang="ts">
	import type { VFSFile } from "./types";
	import Png from "./views/png.svelte";
	import Txt from "./views/txt.svelte";
	import type { Component } from "svelte";
	import Word from "./views/word.svelte";
	import Pdf from "./views/pdf.svelte";

	interface Props {
		file: VFSFile;
	}

	const { file }: Props = $props();
	const views: Record<string, Component<{ file: VFSFile }>> = {
		"ts": Txt,
		"txt": Txt,
		"png": Png,
		"doc": Word,
		"docx": Word,
		"pdf": Pdf,
	};

	const extension = $derived(file.path.split(".").pop() ?? "txt");
	const View = $derived(views[extension] ?? views["txt"]);
</script>

<div class="checkerboard-bg h-full">
	<View {file} />
</div>

<style>
	:global(.transparent-file-vfs) {
		display: inline-block;
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
	}
</style>
