<script lang="ts">
	import type { VFSFile } from "../types";

	interface Props {
		file: VFSFile;
	}

	const { file }: Props = $props();
</script>

<div class="relative h-full w-full">
	<iframe
		title="PDF Document"
		src={file.content?.toString() || ""}
		width="100%"
		height="100%"
		style="min-height: 80vh;"
		frameborder="0"
	></iframe>
</div>
