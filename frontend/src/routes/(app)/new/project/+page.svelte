<script lang="ts">
	import Base from "$lib/components/base.svelte";
	import Markdown from "$lib/components/markdown/markdown.svelte";
	import Tippy from "$lib/components/tippy.svelte";
	import Label from "$lib/components/ui/label/label.svelte";
	import { Separator } from "$lib/components/ui/separator";
	import { Textarea } from "$lib/components/ui/textarea";
	import CircleHelp from "lucide-svelte/icons/circle-help";
	import Upload from "lucide-svelte/icons/upload";
	import { Input } from "$lib/components/ui/input";
	import Control from "$lib/components/forms/control.svelte";
	import Switch from "$lib/components/ui/switch/switch.svelte";

	import GitBranch from "lucide-svelte/icons/git-branch";
	import ShieldAlert from "lucide-svelte/icons/shield-alert";

	import * as Dialog from "$lib/components/ui/dialog";
	import { Button, buttonVariants } from "$lib/components/ui/button/index.js";
	import * as Tabs from "$lib/components/ui/tabs";
	import * as Alert from "$lib/components/ui/alert/index.js";
	import { useForm } from "$lib/utils/form.svelte";
	import type { PageData } from "./$types";
	import { Constants } from "$lib/utils";
	import { toast } from "svelte-sonner";
	import { preview } from "$lib/utils/image.svelte";

	let { data } = $props();
	const { enhance, form } = useForm(data.form, {
		confirm: true,
	});

	const formaction = $derived(true ? `?/update` : "?/create");

	let fileUpload: HTMLInputElement;
</script>

{#snippet gitAlert()}
	<Separator class="my-2" />
	<Alert.Root variant="warning">
		<CircleHelp class="size-4" />
		<Alert.Title>Project structure</Alert.Title>
		<Alert.Description>
			Projects using git source control need have at least 2 files present in their
			repository:
			<ul class="mb-1 list-disc pl-4">
				<li class="underline">README.md</li>
				<li class="underline">PROJECT.md</li>
			</ul>
			These files need to be present at the
			<bold class="font-bold underline">root</bold> of the repository.
		</Alert.Description>
	</Alert.Root>
{/snippet}

<form method="POST" use:enhance>
	<Base variant="center-navbar">
		{#snippet left()}
			<div class="flex max-h-fit flex-col gap-1 rounded border p-4">
				<div class="group relative">
					<input
						bind:this={fileUpload}
						type="file"
						class="absolute inset-0 z-10 cursor-pointer opacity-0"
					/>
					<div class="relative">
						<img
							use:preview={{ input: fileUpload, maxSize: 1 }}
							src={form.data.thumbnailUrl ?? Constants.FALLBACK_IMG}
							alt="logo"
							class="max-h-52 w-full rounded border object-cover"
						/>
						<div
							class="absolute inset-0 bg-black/50 opacity-0 transition-opacity group-hover:opacity-100"
						>
							<Upload class="absolute inset-0 z-[1] m-auto size-8 text-white" />
						</div>
					</div>
				</div>

				<!-- <EvalRatio /> -->
				<Separator class="my-1" />
				<Control label="Name" name="name" errors={form.errors.name}>
					<Input
						id="name"
						type="name"
						name="name"
						autocorrect="off"
						autocomplete={null}
						placeholder="Project..."
						required
						aria-invalid={form.errors.name ? "true" : undefined}
						bind:value={form.data.name}
						{...form.constraints.name}
					/>
				</Control>
				<Control
					label="Public"
					name="public"
					description="Set this to false if you don't wish for anyone to see this project other than maintainers."
					errors={form.errors.public}
				>
					<Switch
						id="public"
						name="public"
						required
						aria-invalid={form.errors.public ? "true" : undefined}
						bind:checked={form.data.public}
						{...form.constraints.public}
					/>
				</Control>
				<Control
					label="Enabled"
					name="enabled"
					description="When true, other users can subscribe to this project."
					errors={form.errors.enabled}
				>
					<Switch
						id="enabled"
						name="enabled"
						required
						aria-invalid={form.errors.enabled ? "true" : undefined}
						bind:checked={form.data.enabled}
						{...form.constraints.enabled}
					/>
				</Control>
				<Separator class="my-1" />
				<Button class="w-full" type="submit" disabled={form.submitting} {formaction}>
					<!-- {#if data.entity}
						Update
					{:else}
						Create
					{/if} -->
					Create
				</Button>
			</div>
		{/snippet}

		{#snippet right()}
			<div>
				<Tippy
					text="Creating a new project requires you write down a markdown sheet detailing what the project is about."
				>
					<h1 class="flex items-center gap-2 text-2xl font-bold">
						New Project
						<CircleHelp size={16} />
					</h1>
				</Tippy>
				<p class="text-muted-foreground text-sm">
					A project is a collection of your work that includes a thumbnail image, a
					detailed markdown description of the project's proceedings, and a brief summary
					for quick reference.
				</p>
				<Separator class="my-1" />
				<Control label="Description" name="description" errors={form.errors.description}>
					<Textarea
						id="description"
						name="description"
						placeholder="A shorter description about the project to read about it at a quick glance ..."
						required
						rows={3}
						autocomplete={null}
						aria-invalid={form.errors.description ? "true" : undefined}
						bind:value={form.data.description}
						{...form.constraints.description}
					/>
				</Control>
				<Separator class="my-1" />
				<Tabs.Root value="markdown">
					<Tabs.List class="w-full">
						<Tabs.Trigger class="w-full" value="github">Github</Tabs.Trigger>
						<Tabs.Trigger class="w-full" value="thirdparty">Thirdparty</Tabs.Trigger>
						<Tabs.Trigger class="w-full" value="markdown">Markdown</Tabs.Trigger>
					</Tabs.List>
					<Tabs.Content value="github">
						<!-- TODO: Support proper github integration for more convenient access -->
						<Alert.Root variant="warning">
							<ShieldAlert class="size-4" />
							<Alert.Title>Oops!</Alert.Title>
							<Alert.Description>
								Proper integration with github as a source provider is not yet setup.
								However you can still use it by defining it as a thirdparty source.
							</Alert.Description>
						</Alert.Root>
						{@render gitAlert()}
					</Tabs.Content>
					<Tabs.Content value="thirdparty">
						<Control label="Git Url" name="giturl" errors={[]}>
							<div class="flex">
								<div class="bg-muted center-content rounded-l border px-3">
									<GitBranch size={20} />
								</div>
								<Input
									id="giturl"
									type="url"
									name="giturl"
									autocorrect="off"
									autocomplete={null}
									placeholder="https://git-provider.com/scope/repo"
									class="rounded-l-none border-l-0"
								/>
							</div>
						</Control>

						<!-- Branch -->
						<Control label="Git Branch" name="gitbranch" errors={[]}>
							<Input
								id="gitbranch"
								type="text"
								name="gitbranch"
								autocorrect="off"
								autocomplete={null}
								placeholder="master"
							/>
						</Control>
						{@render gitAlert()}
					</Tabs.Content>
					<Tabs.Content value="markdown">
						<Markdown
							variant="editor"
							placeholder="# This project is about..."
							bind:value={form.data.markdown}
							{...form.constraints.markdown}
						/>
					</Tabs.Content>
				</Tabs.Root>
			</div>
		{/snippet}
	</Base>
</form>
