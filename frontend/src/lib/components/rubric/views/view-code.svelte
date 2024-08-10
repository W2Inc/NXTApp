<script lang="ts">
	import { onDestroy, onMount } from "svelte";
	import type * as Monaco from "monaco-editor/esm/vs/editor/editor.api";
	import { selectedFile } from "../file-explorer/state";

	let isMounted = false;
	let editor: Monaco.editor.IStandaloneCodeEditor;
	let monaco: typeof Monaco;
	let editorContainer: HTMLElement;

	onMount(async () => {
		monaco = (await import("./monaco")).default;
		const editor = monaco.editor.create(editorContainer, {
			language: "cpp",
			theme: "vs-dark",
			automaticLayout: true,
			fontFamily: "Monaspace",
		});

		selectedFile.subscribe((file) => {
			if (!editor || !monaco || !file) return;

			const model = monaco.editor.createModel(file.contents, "cpp");
			editor.setModel(model);
		});
	});

	onDestroy(() => {
		monaco?.editor.getModels().forEach((model) => model.dispose());
		editor?.dispose();
	});
</script>


<div bind:this={editorContainer} />

<style>
	div {
		width: 100%;
		height: 100%;
	}
</style>
