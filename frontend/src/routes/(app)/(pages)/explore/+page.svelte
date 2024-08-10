<script>
	import Content from "$assets/markdown/explore.md?raw";
	import MarkdownViewer from "$lib/components/markdown/markdown-viewer.svelte";
</script>

<div>
	<MarkdownViewer md={Content} />
</div>

<style>
	div {
		margin: 0 auto;
		max-width: var(--max-content-width);
		padding: 3rem 1.5rem;
	}
</style>
