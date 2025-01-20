<script lang="ts">
	import Control from "$lib/components/forms/control.svelte";
	import Input from "$lib/components/ui/input/input.svelte";
	import { useForm } from "$lib/utils/form.svelte";
	import * as Table from "$lib/components/ui/table";
	import Button from "$lib/components/ui/button/button.svelte";

	const { data } = $props();
	const { enhance, form } = useForm(data.form, {
		confirm: true,
	});
</script>

<form method="post" use:enhance>
	<Control label="Demo" name="demo" errors={form.errors.demo}>
		<Input name="demo" type="text" bind:value={form.data.demo} />
	</Control>

	{#each form.data.projects as project, i}
		<Control label={`Project ${i + 1}`} name="projects[]" errors={form.errors.projects}>
			<Input name="projects[]" type="text" bind:value={form.data.projects[i]} />
		</Control>
	{/each}

	<button type="submit">Submit</button>
</form>
