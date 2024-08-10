<script lang="ts">
	import type { LayoutServerData } from "./$types";

	export let data: LayoutServerData;
</script>

<svelte:head>
	<meta name="[id=slug]" content={data.project.name} />
</svelte:head>

<div class="page">
	<slot />
</div>

<style>
	.page {
		padding: 16px;
		gap: 16px;
		margin-inline: auto;
		padding-inline: 24px;
		max-width: calc(var(--max-content-width) * 1.5);
	}
</style>
