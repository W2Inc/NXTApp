<script lang="ts">
	import Base from "$lib/components/base.svelte";
	import { useTreeState } from "$lib/components/code/context.svelte.js";
	import Explorer from "$lib/components/code/explorer.svelte";
	import Markdown from "$lib/components/markdown/markdown.svelte";

	const { tree } = useTreeState();
	const { data } = $props();



</script>

<Base variant="splitpane">
	{#snippet left()}
		<div class="max-h-dvh overflow-auto p-4">
			<Markdown value={data.rubric} variant="viewer" />
		</div>
	{/snippet}
	{#snippet right()}
		{#await data.code}
			<p>Loading...</p>
		{:then code}
			{@html code}
		{/await}
		<!-- TODO: Explorer with file tree -->
		<!-- <iframe src="http://docs.google.com/gview?url=https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf"
			style="width:600px; height:500px;" frameborder="0"></iframe> -->

		<!-- <embed class="h-dvh" src="/dummy.pdf" width="100%" /> -->
		<!-- <Explorer item={tree} /> -->
	{/snippet}
</Base>
