<script>
	import Base from "$lib/components/base.svelte";
</script>

<Base>
	{#snippet left()}
		Filters
	{/snippet}
	{#snippet right()}
		<div class="p-6">
			<h1>Users</h1>
		</div>
	{/snippet}
</Base>
