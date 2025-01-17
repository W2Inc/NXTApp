<script lang="ts">
	import Base from "$lib/components/base.svelte";
	import Markdown from "$lib/components/markdown/markdown.svelte";
	import Tippy from "$lib/components/tippy.svelte";
	import Label from "$lib/components/ui/label/label.svelte";
	import { Separator } from "$lib/components/ui/separator";
	import { Textarea } from "$lib/components/ui/textarea";
	import CircleHelp from "lucide-svelte/icons/circle-help";
	import { useForm } from "$lib/components/forms/form.svelte";
	import { Input } from "$lib/components/ui/input";
	import Control from "$lib/components/forms/control.svelte";

	import GitBranch from "lucide-svelte/icons/git-branch";
	import ShieldAlert from "lucide-svelte/icons/shield-alert";

	import * as Dialog from "$lib/components/ui/dialog";
	import { Button, buttonVariants } from "$lib/components/ui/button/index.js";
	import * as Tabs from "$lib/components/ui/tabs";
	import * as Alert from "$lib/components/ui/alert/index.js";

	const { data } = $props();
	const { enhance, form, errors, constraints } = useForm(data.form, {
		confirm: true
	});
</script>

<!-- {#snippet ExternalResourc()}

{/snippet} -->

<form method="POST" use:enhance>
	<input type="hidden" name="giturl" value={$form.gitUrl} />
	<input type="hidden" name="gitbranch" value={$form.gitBranch} />
	<Base variant="center-navbar">
		{#snippet left()}
			<div class="flex max-h-fit flex-col gap-1 rounded border p-4">
				<img
					src="https://github.com/w2wizard.png"
					alt="logo"
					class="max-h-64 rounded border object-cover"
				/>
				<!-- <EvalRatio /> -->
				<Separator class="my-1" />
				<Control label="Name" name="name" errors={$errors.name}>
					<Input
						id="name"
						type="name"
						name="name"
						autocorrect="off"
						autocomplete={null}
						placeholder="Project..."
						required
						aria-invalid={$errors.name ? "true" : undefined}
						bind:value={$form.name}
						{...$constraints.name}
					/>
				</Control>

				<Dialog.Root>
					<Tippy
						text="You must select a source control origin for a project to track changes and manage new versions"
					>
						<h4 class="flex items-center gap-2 font-bold">
							Source Control
							<CircleHelp size={16} />
						</h4>
					</Tippy>

					<Dialog.Trigger type="button" class={buttonVariants({ variant: "outline" })}>
						<GitBranch />
						Select Git source
					</Dialog.Trigger>
					<Dialog.Content class="sm:max-w-[425px]">
						<Dialog.Header>
							<Dialog.Title>Define Source</Dialog.Title>
							<Dialog.Description>
								Define where the source of this project is. You can always change it
								later.
							</Dialog.Description>
						</Dialog.Header>
						<Tabs.Root value="thirdparty">
							<Tabs.List class="w-full">
								<Tabs.Trigger class="w-full" value="github">Github</Tabs.Trigger>
								<Tabs.Trigger class="w-full" value="thirdparty">Thirdparty</Tabs.Trigger>
							</Tabs.List>
							<Separator class="my-1" />
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
							</Tabs.Content>
							<Tabs.Content value="thirdparty">
								<Control label="Git Url" name="giturl" errors={$errors.gitUrl}>
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
											required
											class="rounded-l-none border-l-0"
											aria-invalid={$errors.gitUrl ? "true" : undefined}
											bind:value={$form.gitUrl}
											{...$constraints.gitUrl}
										/>
									</div>
								</Control>

								<!-- Branch -->
								<Control label="Git Branch" name="gitbranch" errors={$errors.gitBranch}>
									<Input
										id="gitbranch"
										type="text"
										name="gitbranch"
										autocorrect="off"
										autocomplete={null}
										placeholder="master"
										required
										aria-invalid={$errors.gitBranch ? "true" : undefined}
										bind:value={$form.gitBranch}
										{...$constraints.gitBranch}
									/>
								</Control>
							</Tabs.Content>
						</Tabs.Root>
						<Dialog.Footer>
							<!-- <Button type="submit">Save</Button> -->
						</Dialog.Footer>
					</Dialog.Content>
				</Dialog.Root>
				<Separator class="my-1" />
				<Button type="submit">Create</Button>
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
				<Markdown
					variant="editor"
					placeholder="# This project is about..."
					bind:value={$form.markdown}
					{...$constraints.markdown}
				/>
				<Separator class="my-1" />
				<Control label="Description" name="description" errors={$errors.description}>
					<Textarea
						id="description"
						name="description"
						placeholder="A shorter description about the project to read about it at a quick glance ..."
						required
						autocomplete={null}
						aria-invalid={$errors.description ? "true" : undefined}
						bind:value={$form.description}
						{...$constraints.description}
					/>
				</Control>
			</div>
		{/snippet}
	</Base>
</form>
