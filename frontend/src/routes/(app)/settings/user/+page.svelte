<script lang="ts">
	import * as Avatar from "$lib/components/ui/avatar/";
	import Separator from "$lib/components/ui/separator/separator.svelte";
	import { Input } from "$lib/components/ui/input";
	import Button from "$lib/components/ui/button/button.svelte";
	import Control from "$lib/components/forms/control.svelte";
	import Trash from "lucide-svelte/icons/trash";
	import Save from "lucide-svelte/icons/save";
	import Database from "lucide-svelte/icons/database";
	import Upload from "lucide-svelte/icons/upload";
	import { dialog } from "$lib/components/dialog/state.svelte.js";
	import { Textarea } from "$lib/components/ui/textarea/index.js";
	import { mode } from "mode-watcher";
	import Markdown2 from "$lib/components/markdown/markdown.svelte";
	import { useForm } from "$lib/utils/form.svelte.js";
	import { page } from "$app/state";
	import { Constants } from "$lib/utils.js";
	import { preview } from "$lib/utils/image.svelte.js";
	import { toast } from "svelte-sonner";
	import Tippy from "$lib/components/tippy.svelte";
	import Markdown from "svelte-exmarkdown";

	const { data } = $props();
	const { enhance, form } = useForm(data.form, { confirm: true });

	// HACK: Some sort of issue where svelte-exmarkdown want it to be state... but yeah?
	let md = $state(form.data.markdown.toString())


	let fileUpload: HTMLInputElement;
	async function deleteCache() {
		const choice = await dialog.confirm({
			title: "Delete local cache ?",
			message: "Will clear things such as the local history.",
		});

		if (choice) {
			localStorage.clear();
			toast.success("Cache cleared");
		}
	}

</script>

<form method="POST" use:enhance enctype="multipart/form-data">
	<h1 class="text-2xl">Profile Settings</h1>
	<p class="text-muted-foreground text-sm">
		Configure your profile here in any way you want. You may provide a brief introduction
		about yourself, include any relevant links, and update your avatar if desired.
	</p>
	<Separator class="my-2" />
	<Control
		label="Image"
		name="image"
		description="Upload your own custom profile picture"
		errors={form.errors.image}
	>
		<div class="group relative max-w-52">
			<input
				bind:this={fileUpload}
				type="file"
				name="image"
				value=""
				class="absolute inset-0 z-10 cursor-pointer opacity-0"
			/>
			<div class="relative">
				<img
					use:preview={{ input: fileUpload }}
					src={(form.data.image as string) ?? Constants.FALLBACK_IMG}
					alt="logo"
					class="max-h-52 w-full rounded border object-cover"
				/>
				<div
					class="absolute inset-0 rounded bg-black/50 opacity-0 transition-opacity group-hover:opacity-100"
				>
					<Upload class="absolute inset-0 z-[1] m-auto size-8 text-white" />
				</div>
			</div>
		</div>
	</Control>

	<Separator class="my-2" />

	<Control label="User ID" name="id" errors={form.errors.id}>
		<Input
			id="id"
			type="text"
			name="id"
			autocorrect="off"
			autocomplete={null}
			placeholder="Cursus..."
			aria-invalid={form.errors.id ? "true" : undefined}
			bind:value={form.data.id}
			{...form.constraints.id}
		/>
	</Control>

	<!-- <fieldset class="border-input rounded border p-4">
		<legend class="text-muted-foreground rounded border px-2 text-sm font-medium">
			Configure user details
		</legend> -->
	<div class="flex gap-3">
		<Control label="Login" name="login" errors={form.errors.login}>
			<Input
				id="login"
				type="text"
				name="login"
				autocorrect="off"
				autocomplete={null}
				aria-invalid={form.errors.login ? "true" : undefined}
				bind:value={form.data.login}
				{...form.constraints.login}
			/>
		</Control>
		<Control label="Display name" name="displayName" errors={form.errors.displayName}>
			<Input
				id="displayName"
				type="text"
				name="displayName"
				autocorrect="off"
				autocomplete={null}
				placeholder="x_johnny_silverhand_x"
				aria-invalid={form.errors.displayName ? "true" : undefined}
				bind:value={form.data.displayName}
				{...form.constraints.displayName}
			/>
		</Control>
	</div>

	<Separator class="my-2" />

	<Control label="First Name" name="firstName" errors={form.errors.firstName}>
		<Input
			id="firstName"
			type="text"
			name="firstName"
			autocorrect="off"
			autocomplete="given-name"
			placeholder="John"
			aria-invalid={form.errors.firstName ? "true" : undefined}
			bind:value={form.data.firstName}
			{...form.constraints.firstName}
		/>
	</Control>

	<Control label="Last Name" name="lastName" errors={form.errors.lastName}>
		<Input
			id="lastName"
			type="text"
			name="lastName"
			autocorrect="off"
			autocomplete="family-name"
			placeholder="Doe"
			aria-invalid={form.errors.lastName ? "true" : undefined}
			bind:value={form.data.lastName}
			{...form.constraints.lastName}
		/>
	</Control>

	<Separator class="my-2" />

	<div class="grid grid-cols-2 grid-rows-2 gap-3">
		<Control
			label="Personal Website"
			name="website"
			errors={form.errors.website}
			description="Link to your personal website"
		>
			<Input
				id="website"
				type="text"
				name="website"
				autocorrect="off"
				autocomplete="off"
				placeholder="https://example.com"
				aria-invalid={form.errors.website ? "true" : undefined}
				bind:value={form.data.website}
				{...form.constraints.website}
			/>
		</Control>
		<Control
			label="𝕏 Profile"
			name="twitter"
			errors={form.errors.twitter}
			description="Link to your Twitter / 𝕏 profile"
		>
			<Input
				id="twitter"
				type="text"
				name="twitter"
				autocorrect="off"
				autocomplete="off"
				placeholder="https://twitter.com/username"
				aria-invalid={form.errors.twitter ? "true" : undefined}
				bind:value={form.data.twitter}
				{...form.constraints.twitter}
			/>
		</Control>

		<Control
			label="LinkedIn Profile"
			name="linkedin"
			errors={form.errors.linkedin}
			description="Link to your LinkedIn profile"
		>
			<Input
				id="linkedin"
				type="text"
				name="linkedin"
				autocorrect="off"
				autocomplete="off"
				placeholder="https://linkedin.com/in/username"
				aria-invalid={form.errors.linkedin ? "true" : undefined}
				bind:value={form.data.linkedin}
				{...form.constraints.linkedin}
			/>
		</Control>

		<Control
			label="GitHub Profile"
			name="github"
			errors={form.errors.github}
			description="Link to your GitHub profile"
		>
			<Input
				id="github"
				type="text"
				name="github"
				autocorrect="off"
				autocomplete="off"
				placeholder="https://github.com/username"
				aria-invalid={form.errors.github ? "true" : undefined}
				bind:value={form.data.github}
				{...form.constraints.github}
			/>
		</Control>
	</div>

	<Separator class="my-2" />

	<Control
		label="Biography"
		name="markdown"
		errors={form.errors.markdown}
		description="Here you can write about yourself"
	>
		<Markdown2
			variant="editor"
			placeholder="# This project is about..."
			bind:value={md}
			{...form.constraints.markdown}
		/>
	</Control>

	<Separator class="my-2" />

	<div class="flex justify-between gap-2">
		<div>
			<Button type="submit" loading={form.submitting}>
				<Save />
				Save
			</Button>
			<Button
				type="button"
				variant="destructive"
				disabled={form.submitting}
				onclick={deleteCache}
			>
				<Trash />
				Delete Cache
			</Button>
		</div>
		<Tippy text="Request GDRP compliant data">
			<Button
				type="button"
				disabled={form.submitting}
				href="mailto:gdpr@example.com?subject=GDRP%20Data%20request.&body=%23%20GDRP%20Data%20request%0A%0AI%20would%20like%20to%20request%20all%20my%20data."
			>
				<Database />
				GDPR Request
			</Button>
		</Tippy>
	</div>

	<!-- <Control
		label="Display Name"
		name="displayName"
		errors={form.errors.displayName}
		description="As logins cannot change your display name is a that you can use to visibly overwrite your name with"
	>
		<Input
			id="displayName"
			type="text"
			name="displayName"
			autocorrect="off"
			autocomplete={null}
			placeholder="x_johnny_silverhand_x"
			aria-invalid={form.errors.displayName ? "true" : undefined}
			bind:value={form.data.displayName}
			{...form.constraints.displayName}
		/>
	</Control> -->
</form>
