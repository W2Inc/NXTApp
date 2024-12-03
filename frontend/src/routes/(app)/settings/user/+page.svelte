<script lang="ts">
	import * as Avatar from "$lib/components/ui/avatar/";
	import Separator from "$lib/components/ui/separator/separator.svelte";
	import Upload from "lucide-svelte/icons/upload";
	import { Input } from "$lib/components/ui/input";
	import { Label } from "$lib/components/ui/label";
	import { superForm } from "sveltekit-superforms";
	import Button from "$lib/components/ui/button/button.svelte";
	import { toast } from "svelte-sonner";
	import Control from "$lib/components/forms/control.svelte";
	import { applyAction } from "$app/forms";
	import Trash from "lucide-svelte/icons/trash";
	import { dialog } from "$lib/components/dialog/state.svelte.js";
	import Loader from "lucide-svelte/icons/loader";
	import { useForm } from "$lib/components/forms/form.svelte.js";

	const { data } = $props();
	const { enhance, form, errors, constraints } = useForm(data.form);

	async function deleteCache() {
		await dialog.confirm({
			message: "Balls?",

			title: "Balls",
		});
	}
</script>

<h1 class="text-2xl">User</h1>
<Separator class="my-2" />
<!-- Avatar -->
<div>
	<Avatar.Root class="h-32 w-32 rounded-sm">
		<Avatar.Image src="https://github.com/w2wizard.png" alt="@w2wizard" />
		<Avatar.Fallback class="relative rounded-sm">
			US
			<!-- <input
				 type="file"
				 title="Upload a profile picture (max 5MB, png, gif, jpeg)"
				 accept="image/png, image/gif, image/jpeg"
				 class="absolute inset-0 h-32 w-32 cursor-pointer opacity-0"
				 onchange={handleImagePreview}
			 />
			 <div class="pointer-events-none absolute inset-0 flex items-center justify-center">
				 <Upload size="48" />
			 </div> -->
		</Avatar.Fallback>
	</Avatar.Root>
	<!-- TODO: Banner image -->
</div>

<form method="POST" use:enhance>
	<Control label="Name" name="name" errors={$errors.name}>
		<Input
			type="text"
			name="name"
			aria-invalid={$errors.name ? "true" : undefined}
			bind:value={$form.name}
			{...$constraints.name}
		/>
	</Control>

	<Control label="Email" name="email" errors={$errors.email}>
		<Input
			type="email"
			name="email"
			required
			aria-invalid={$errors.email ? "true" : undefined}
			bind:value={$form.email}
			{...$constraints.email}
		/>
	</Control>

	<Separator class="my-2" />
	<div class="flex gap-2">
		<Button type="submit">Submit</Button>
		<Button variant="destructive" onclick={deleteCache}>
			<Trash />
			Clear Cache
		</Button>
	</div>
</form>

<style>
	.invalid {
		color: red;
	}
</style>
