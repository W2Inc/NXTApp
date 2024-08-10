<script>
	import Content from "$assets/markdown/terms.md?raw";
	import CardNotice from "$lib/components/cards/card-notice.svelte";
	import MarkdownViewer from "$lib/components/markdown/markdown-viewer.svelte";
</script>

<div>
	<CardNotice type="warning">
		<p>
			<strong>Warning:</strong> These terms of service are a work in progress and are
			subject to change at any time.
		</p>
	</CardNotice>
	<br />
	<MarkdownViewer md={Content} />
</div>

<style>
	div {
		margin: 0 auto;
		max-width: 48rem;
		padding: 3rem 1.5rem;
	}
</style>
