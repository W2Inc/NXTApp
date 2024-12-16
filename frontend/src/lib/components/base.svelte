<script lang="ts">
	import type { Snippet } from "svelte";

	interface Props {
		left: Snippet;
		right: Snippet;
	}

	const { left, right }: Props = $props();
</script>

<div class="grid grid-rows-[auto_1fr] lg:grid-rows-1 lg:grid-cols-[auto_1fr]">
	<aside
		class="sticky top-0 flex min-w-96 flex-col gap-2 p-4 dark:bg-card lg:h-dvh lg:border-r"
	>
		{@render left()}
	</aside>

	<!-- Body -->
	{@render right()}
</div>
