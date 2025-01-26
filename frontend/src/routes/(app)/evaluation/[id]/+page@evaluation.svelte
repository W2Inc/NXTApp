<script lang="ts">
	import Base from "$lib/components/base.svelte";
	import { useTreeState } from "$lib/components/code/context.svelte.js";
	import Explorer from "$lib/components/code/explorer.svelte";
	import Markdown from "$lib/components/markdown/markdown.svelte";
	import Separator from "$lib/components/ui/separator/separator.svelte";
	import * as Menubar from "$lib/components/ui/menubar";

	const { data } = $props();
	let bookmarks = $state(false);
	let fullUrls = $state(true);
	let profileRadioValue = $state("benoit");
</script>

<Base variant="splitpane">
	{#snippet left()}
		<div class="max-h-dvh overflow-auto">
			<div class="sticky top-0 bg-background z-10">
				<h1 class="px-4 py-2 text-2xl">W2Wizard Evaluation</h1>
				<Separator />

				<Separator />
			</div>

			<div class="">
				<Markdown value="Hello World!" variant="viewer" />
			</div>
		</div>
	{/snippet}

	{#snippet right()}
		{#await data.code}
			<p>Loading...</p>
		{:then code}
			<!-- {@html code} -->
			<iframe src='https://view.officeapps.live.com/op/embed.aspx?src=http%3A%2F%2Fieee802%2Eorg%3A80%2Fsecmail%2FdocIZSEwEqHFr%2Edoc' width='100%' height='100%' frameborder='0'>This is an embedded <a target='_blank' href='http://office.com'>Microsoft Office</a> document, powered by <a target='_blank' href='http://office.com/webapps'>Office Online</a>.</iframe>
		{/await}
		<!-- <embed class="h-dvh" src="/dummy.pdf" width="100%" />
			{@html code}
		{/await}
		TODO: Explorer with file tree
		<iframe src="http://docs.google.com/gview?url=https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf"
			style="width:600px; height:500px;" frameborder="0"></iframe>

		<Explorer item={tree} /> -->
		<!-- {/await} -->
	{/snippet}
</Base>
