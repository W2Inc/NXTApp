<script lang="ts">
	import type { VFSFile } from "../types";

	interface Props {
		file: VFSFile;
	}

	const { file }: Props = $props();
</script>

<div class="transparent-file-vfs flex justify-center p-4">
	<img
		src={file.content?.toString() || ""}
		alt={file.path}
		class="max-h-[calc(100vh-12rem)] max-w-full object-contain"
	/>
</div>
