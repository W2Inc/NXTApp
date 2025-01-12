<script lang="ts">
	import * as Avatar from "$lib/components/ui/avatar/";
	import Separator from "$lib/components/ui/separator/separator.svelte";
	import { Input } from "$lib/components/ui/input";
	import Button from "$lib/components/ui/button/button.svelte";
	import Control from "$lib/components/forms/control.svelte";
	import Trash from "lucide-svelte/icons/trash";
	import { dialog } from "$lib/components/dialog/state.svelte.js";
	import { useForm } from "$lib/components/forms/form.svelte.js";
	import { Textarea } from "$lib/components/ui/textarea/index.js";
	import { mode } from "mode-watcher";
	import Markdown from "$lib/components/markdown/markdown.svelte";

	// import { Carta, MarkdownEditor } from "carta-md";
	// import "carta-md/default.css"; /* Default theme */

	// const carta = new Carta();
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
<p class="text-muted-foreground text-sm">
	Configure your profile here in anyway you want.
</p>
<Separator class="my-2" />
<Avatar.Root class="h-32 w-32 rounded-sm">
	<Avatar.Image src={data.session?.user?.image} alt="@w2wizard" />
	<Avatar.Fallback class="relative rounded-sm">US</Avatar.Fallback>
</Avatar.Root>
<Separator class="my-2" />

<form method="POST" use:enhance>
	<Control label="User ID" name="id" errors={$errors.id}>
		<Input
			autocorrect="off"
			autocomplete={null}
			type="text"
			name="id"
			readonly
			value={$form.id}
		/>
	</Control>

	<Control label="Email" name="email" errors={$errors.email}>
		<Input
			id="email"
			type="email"
			name="email"
			autocorrect="off"
			autocomplete={null}
			placeholder="main@example.com"
			required
			aria-invalid={$errors.email ? "true" : undefined}
			bind:value={$form.email}
			{...$constraints.email}
		/>
	</Control>

	<div class="flex gap-4">
		<Control label="First name" name="first" errors={$errors.firstName}>
			<Input
				id="first"
				type="text"
				name="first"
				autocorrect="off"
				autocomplete={null}
				placeholder="Sony"
				aria-invalid={$errors.firstName ? "true" : undefined}
				bind:value={$form.firstName}
				{...$constraints.firstName}
			/>
		</Control>
		<Control label="Last Name" name="last" errors={$errors.lastName}>
			<Input
				id="last"
				type="text"
				name="last"
				autocorrect="off"
				autocomplete={null}
				placeholder="Ercison"
				aria-invalid={$errors.lastName ? "true" : undefined}
				bind:value={$form.lastName}
				{...$constraints.lastName}
			/>
		</Control>
	</div>

	<div class="flex gap-4">
		<Control label="Username" name="username" errors={$errors.userName}>
			<Input
				id="username"
				type="text"
				name="username"
				readonly
				autocorrect="off"
				autocomplete={null}
				placeholder="x_silverhand_x"
				aria-invalid={$errors.userName ? "true" : undefined}
				bind:value={$form.userName}
				{...$constraints.userName}
			/>
		</Control>
		<Control label="Display Name" name="display" errors={$errors.displayName}>
			<Input
				id="display"
				type="text"
				name="display"
				autocorrect="off"
				autocomplete={null}
				placeholder="Johnny Silverhand"
				aria-invalid={$errors.displayName ? "true" : undefined}
				bind:value={$form.displayName}
				{...$constraints.displayName}
			/>
		</Control>
	</div>

	<Separator class="my-2" />

	<div class="flex gap-4">
		<Control label="Github" name="github" errors={$errors.github}>
			<Input
				id="github"
				type="url"
				name="github"
				placeholder="https://github.com/"
				aria-invalid={$errors.github ? "true" : undefined}
				bind:value={$form.github}
				{...$constraints.github}
			/>
		</Control>
		<Control label="LinkedIn" name="linkedin" errors={$errors.linkedin}>
			<Input
				id="linkedin"
				type="url"
				name="last"
				placeholder="https://linkedin.com/"
				aria-invalid={$errors.linkedin ? "true" : undefined}
				bind:value={$form.linkedin}
				{...$constraints.linkedin}
			/>
		</Control>
	</div>

	<div class="flex gap-4">
		<Control label="Custom Website" name="website" errors={$errors.website}>
			<Input
				type="url"
				id="website"
				name="website"
				placeholder="https://website.com/"
				aria-invalid={$errors.website ? "true" : undefined}
				bind:value={$form.website}
				{...$constraints.website}
			/>
		</Control>
		<Control label="𝕏 / Twitter" name="twitter" errors={$errors.twitter}>
			<Input
				type="url"
				id="twitter"
				name="twitter"
				placeholder="https://x.com/"
				aria-invalid={$errors.twitter ? "true" : undefined}
				bind:value={$form.twitter}
				{...$constraints.twitter}
			/>
		</Control>
	</div>

	<Separator class="my-2" />

	<div class="grid w-full gap-1.5">
		<Markdown value="" placeholder="Write about yourself" />
	</div>

	<Separator class="my-2" />
	<div class="flex gap-2">
		<Button type="submit">Submit</Button>
		<Button variant="destructive" onclick={deleteCache}>
			<Trash />
			Clear Cache
		</Button>
	</div>
</form>
