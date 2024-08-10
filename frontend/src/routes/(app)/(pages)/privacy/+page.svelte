<script>
	import Content from "$assets/markdown/privacy.md?raw";
	import CardNotice from "$lib/components/cards/card-notice.svelte";
	import MarkdownViewer from "$lib/components/markdown/markdown-viewer.svelte";
</script>

<div>
	<CardNotice type="warning">
		<p>
			<strong>Warning:</strong> This privacy policy is a work in progress and is
			subject to change at any time.
		</p>
	</CardNotice>
	<br />
	<MarkdownViewer md={Content} />
</div>

<style>
	div {
		margin: 0 auto;
		max-width: var(--max-content-width);
		padding: 3rem 1.5rem;
	}
</style>
