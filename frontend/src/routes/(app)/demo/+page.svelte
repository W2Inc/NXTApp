<script lang="ts">
	import Control from "$lib/components/forms/control.svelte";
	import Input from "$lib/components/ui/input/input.svelte";
	import { useForm } from "$lib/utils/form.svelte";
	import Button from "$lib/components/ui/button/button.svelte";

	const { data } = $props();
	const { enhance, form } = useForm(data.form, {
		confirm: true,
	});
</script>

<form method="post" use:enhance>
	<!-- Control is basically a label but also lets me do descriptions on fields and show errors -->
	<Control label="Demo" name="demo" errors={form.errors.demo}>
		<Input name="demo" type="text" bind:value={form.data.demo} />
	</Control>

	<Control label="Projects" name="projects[]" errors={form.errors.projects}>
		{#each form.data.projects as project, i}
			<Input name="projects[]" type="text" bind:value={form.data.projects[i]} />
		{/each}
	</Control>

	<Button type="submit">Submit</Button>
</form>
