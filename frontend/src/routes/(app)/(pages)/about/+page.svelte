<script>
	import Content from "$assets/markdown/about.md?raw";
	import MarkdownViewer from "$lib/components/markdown/markdown-viewer.svelte";
</script>

<div>
	<MarkdownViewer md="Soon™" />
</div>

<style>
	div {
		margin: 0 auto;
		max-width: var(--max-content-width);
	}
</style>
