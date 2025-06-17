<script lang="ts">
	import type { VFSFile } from "../types";

	interface Props {
		file: VFSFile;
	}

	const { file }: Props = $props();
	let success = $state(true);
</script>

<div class="relative h-full w-full">
	<iframe
		title="Word Document"
		src="https://view.officeapps.live.com/op/embed.aspx?src={file.}"
		width="100%"
		height="100%"
		frameborder="0"
		onerror={() => (success = false)}
		>This is an embedded <a target="_blank" href="http://office.com">Microsoft Office</a>
		document, powered by
		<a target="_blank" href="http://office.com/webapps">Office Online</a>.</iframe
	>

	{#if !success}
		<div
			class="absolute inset-0 flex flex-col items-center justify-center bg-white/90 p-4"
		>
			<div class="max-w-md rounded border border-red-400 bg-red-100 p-4 text-red-700">
				<p class="mb-2 text-lg font-bold">Error Loading Document</p>
				<p>
					The document could not be loaded. Please check the file format or try again
					later.
				</p>
			</div>
		</div>
	{/if}
</div>
