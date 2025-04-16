<script lang="ts">
	import Control from "$lib/components/forms/control.svelte";
	import { Separator } from "$lib/components/ui/separator";
	import { useForm } from "$lib/utils/api.svelte";
	import { preview } from "$lib/utils/image.svelte.js";
	import Upload from "lucide-svelte/icons/upload";
	import Button from "$lib/components/ui/button/button.svelte";
	import Trash from "lucide-svelte/icons/trash";
	import Save from "lucide-svelte/icons/save";
	import Database from "lucide-svelte/icons/database";
	import type { PageProps } from "./$types";
	import { Input } from "$lib/components/ui/input";
	import Markdown from "$lib/components/markdown/markdown.svelte";
	import { dialog } from "$lib/components/dialog/state.svelte";
	import { toast } from "svelte-sonner";
	import Thumbnail from "$lib/components/thumbnail.svelte";

	const { data }: PageProps = $props();
	const { form, enhance } = useForm(data.form, { confirm: true });
	let fileUpload: HTMLInputElement = $state(null!);
	let md = $state(form.data.markdown?.toString() ?? "");

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
		errors={form.errors.AvatarUrl}
	>
		<Thumbnail src={data.form.data.avatarUrl as string} />
	</Control>
	<Separator class="my-2" />

	<Control label="User ID" name="id">
		<Input
			id="id"
			type="text"
			name="id"
			autocorrect="off"
			autocomplete={null}
			placeholder="Cursus..."
			disabled
			readonly
			value={data.session?.user_id}
		/>
	</Control>

	<div class="flex gap-3">
		<Control label="Login" name="login">
			<Input
				id="login"
				type="text"
				name="login"
				autocorrect="off"
				autocomplete={null}
				disabled
				readonly
				value={data.session?.preferred_username}
			/>
		</Control>
		<Control label="Display name" name="displayName" errors={form.errors.DisplayName}>
			<Input
				id="displayName"
				type="text"
				name="displayName"
				autocorrect="off"
				autocomplete={null}
				placeholder="x_johnny_silverhand_x"
				aria-invalid={form.errors.DisplayName ? "true" : undefined}
				bind:value={form.data.displayName}
			/>
		</Control>
	</div>

	<Separator class="my-2" />

	<Control label="First Name" name="firstName">
		<Input
			id="firstName"
			type="text"
			name="firstName"
			autocorrect="off"
			autocomplete="given-name"
			placeholder="John"
			aria-invalid={form.errors.FirstName ? "true" : undefined}
			bind:value={form.data.firstName}
		/>
	</Control>

	<Control label="Last Name" name="lastName">
		<Input
			id="lastName"
			type="text"
			name="lastName"
			autocorrect="off"
			autocomplete="family-name"
			placeholder="Doe"
			aria-invalid={form.errors.LastName ? "true" : undefined}
			bind:value={form.data.lastName}
		/>
	</Control>

	<Separator class="my-2" />
	<div class="grid grid-cols-2 grid-rows-2 gap-3">
		<Control
			label="Personal Website"
			name="website"
			errors={form.errors.WebsiteUrl}
			description="Link to your personal website"
		>
			<Input
				id="website"
				type="text"
				name="website"
				autocorrect="off"
				autocomplete="off"
				placeholder="https://example.com"
				bind:value={form.data.websiteUrl}
				aria-invalid={form.errors.WebsiteUrl ? "true" : undefined}
			/>
		</Control>
		<Control
			label="𝕏 Profile"
			name="twitter"
			errors={form.errors.TwitterUrl}
			description="Link to your Twitter / 𝕏 profile"
		>
			<Input
				id="twitter"
				type="text"
				name="twitter"
				autocorrect="off"
				autocomplete="off"
				placeholder="https://twitter.com/username"
				bind:value={form.data.twitterUrl}
				aria-invalid={form.errors.TwitterUrl ? "true" : undefined}
			/>
		</Control>

		<Control
			label="LinkedIn Profile"
			name="linkedin"
			errors={form.errors.LinkedinUrl}
			description="Link to your LinkedIn profile"
		>
			<Input
				id="linkedin"
				type="text"
				name="linkedin"
				autocorrect="off"
				autocomplete="off"
				placeholder="https://linkedin.com/in/username"
				bind:value={form.data.linkedinUrl}
				aria-invalid={form.errors.LinkedinUrl ? "true" : undefined}
			/>
		</Control>

		<Control
			label="GitHub Profile"
			name="github"
			errors={form.errors.GithubUrl}
			description="Link to your GitHub profile"
		>
			<Input
				id="github"
				type="text"
				name="github"
				autocorrect="off"
				autocomplete="off"
				placeholder="https://github.com/username"
				bind:value={form.data.githubUrl}
				aria-invalid={form.errors.GithubUrl ? "true" : undefined}
			/>
		</Control>
	</div>

	<Separator class="my-2" />

	<Control
		label="Biography"
		name="markdown"
		description="Here you can write about yourself"
	>
		<Markdown variant="editor" placeholder="# This project is about..." bind:value={md} />
	</Control>

	<Separator class="my-2" />

	<div class="flex justify-between gap-2">
		<div>
			<Button type="submit" loading={form.isLoading}>
				<Save />
				Save
			</Button>
			<Button
				type="button"
				variant="destructive"
				disabled={form.isLoading}
				onclick={deleteCache}
			>
				<Trash />
				Delete Cache
			</Button>
		</div>
	</div>
</form>
