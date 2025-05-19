<script>
	import Base from "$lib/components/base.svelte";
	import Input from "$lib/components/ui/input/input.svelte";
</script>

<Base>
	{#snippet left()}
		<Input />
		Content here...
	{/snippet}
	{#snippet right()}
		Content here...
	{/snippet}
</Base>
