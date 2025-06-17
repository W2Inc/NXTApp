<script lang="ts">
	import type { VFSFile } from "../types";

	interface Props {
		file: VFSFile;
	}

	const { file }: Props = $props();
</script>

<div class="p-4">
	<pre class="bg-muted overflow-auto whitespace-pre-wrap rounded-md p-4">
		<code>{file.content?.toString() || ""}</code>
	</pre>
</div>
