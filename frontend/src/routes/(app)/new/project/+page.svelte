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
	import * as Popover from "$lib/components/ui/popover";

	import Trash from "lucide-svelte/icons/trash";
	import Code from "lucide-svelte/icons/code";
	import Terminal from "lucide-svelte/icons/terminal";
	import Copy from "lucide-svelte/icons/copy";
	import CopyCheck from "lucide-svelte/icons/copy-check";
	import BookMarked from "lucide-svelte/icons/book-marked";

	import * as Dialog from "$lib/components/ui/dialog";
	import { Button, buttonVariants } from "$lib/components/ui/button/index.js";
	import * as Tabs from "$lib/components/ui/tabs";
	import * as Alert from "$lib/components/ui/alert/index.js";
	import type { PageData } from "./$types";
	import { Constants } from "$lib/utils";
	import { toast } from "svelte-sonner";
	import { preview } from "$lib/utils/image.svelte";
	import { Slider } from "$lib/components/ui/slider";
	import { page } from "$app/state";
	import { useDebounce } from "$lib/utils/debounce.svelte";
	import useClipboard from "$lib/utils/clipboard.svelte";
	import { fade, scale } from "svelte/transition";
	import { useForm } from "$lib/utils/api.svelte";
	import TagInput from "$lib/components/tag-input.svelte";
	import { Search, User } from "lucide-svelte";
	import SearchApi from "$lib/components/search-api.svelte";
	import { SvelteSet } from "svelte/reactivity";

	let { data } = $props();
	const { enhance, form } = useForm(data.form, {
		confirm: true,
	});

	const debounce = useDebounce(450);
	const clipboard = useClipboard("");
	const formaction = $derived(data.entity ? `?/update` : "?/create");
	const disabled = $derived(data.entity !== null);
	let collaboratorUsers = new SvelteSet<BackendTypes["MinimalUserDTO"]>([]);

	let fileUpload: HTMLInputElement;

	const clients = [
		{
			title: "VSCode",
			url: "vscode://vscode.git/clone?url=",
		},
		{
			title: "VSCode Insiders",
			url: "vscode-insiders://vscode.git/clone?url=",
		},
		{
			title: "GitHub Desktop",
			url: "x-github-client://openRepo/",
		},
		{
			title: "GitKraken",
			url: "gitkraken://repo/clone/",
		},
		{
			title: "Sourcetree",
			url: "sourcetree://cloneRepo/",
		},
		{
			title: "Sublime Merge",
			url: "sublime-merge://new/repo/",
		},
		{
			title: "Fork",
			url: "fork://cloneRepo/",
		},
	];

	async function searchUsers(query: string) {
		const response = await fetch(`/users?${new URLSearchParams({ name: query })}`);
		return (await response.json()) as BackendTypes["MinimalUserDTO"][];
	}
</script>

{#snippet gitInstructions(gitInfo: BackendTypes["GitDO"])}
	{#snippet instruction(title: string, action: string)}
		<li class="marker:pl-4">
			{title}
			<pre class="bg-muted">
				<code>{action}</code>
			</pre>
		</li>
	{/snippet}

	<Dialog.Root>
		<Dialog.Trigger type="button" class={buttonVariants({ variant: "outline" })}>
			<Terminal />
			Clone via Terminal
		</Dialog.Trigger>
		<Dialog.Content>
			<Dialog.Header>
				<Dialog.Title>How to clone the project repository</Dialog.Title>
				<Separator />
				<Dialog.Description>
					<ol class="markdown-body space-y-2">
						{@render instruction("Clone the repository", `git clone ${gitInfo?.url}`)}
						{@render instruction(
							"Change into the project directory",
							`cd ${gitInfo?.name}`,
						)}
						{@render instruction("Make changes to files using your editor", "code .")}
						{@render instruction("Stage your changes", "git add .")}
						{@render instruction(
							"Commit your changes",
							"git commit -m 'Your commit message'",
						)}
						{@render instruction("Push changes back to repository", "git push")}
					</ol>
				</Dialog.Description>
			</Dialog.Header>
		</Dialog.Content>
	</Dialog.Root>
{/snippet}

{#snippet collaborators()}
	<Dialog.Root>
		<Dialog.Trigger type="button" class={buttonVariants({ variant: "outline" })}>
			<User />
			Manage Collaborators
		</Dialog.Trigger>
		<Dialog.Content class="max-w-xl">
			<Dialog.Header>
				<Dialog.Title>Project Collaborators</Dialog.Title>
				<Dialog.Description>
					Manage users who can access and contribute to this project.
				</Dialog.Description>
				<Separator class="my-2" />

				<!-- Search -->
				<div class="flex gap-2">
					<SearchApi
						class="w-full"
						endpointFn={searchUsers}
						placeholder="Search..."
						onSelect={(v) => {
							collaboratorUsers.add(v);
						}}
					>
						{#snippet item({ value })}
							{#if !collaboratorUsers.has(value)}
								<div class="flex w-full items-center justify-between gap-2 py-1">
									<img
										src={value?.avatarUrl || Constants.FALLBACK_IMG}
										alt={value?.displayName || value?.login}
										class="h-8 w-8 rounded-full object-cover"
									/>
									<div class="flex flex-col overflow-hidden">
										<span class="truncate font-medium"
											>{value?.displayName || value?.login}</span
										>
										<span class="text-muted-foreground truncate text-xs"
											>{value?.login}</span
										>
									</div>
								</div>
							{/if}
						{/snippet}
					</SearchApi>
				</div>

				<div class="max-h-[300px] overflow-y-auto rounded">
					<div class="divide-y">
						{#if collaboratorUsers.size === 0}
							<div class="text-muted-foreground p-4 text-center">
								No collaborators yet. Add someone to get started.
							</div>
						{:else}
							{#each collaboratorUsers as user}
								<div class="flex items-center justify-between border p-3">
									<div class="flex items-center gap-3">
										<img
											src={user?.avatarUrl || Constants.FALLBACK_IMG}
											alt={user?.displayName}
											class="h-10 w-10 rounded-full object-cover"
										/>
										<div>
											<p class="font-medium">{user?.displayName}</p>
											<p class="text-muted-foreground text-sm">Write Access</p>
										</div>
									</div>
									<Button
										variant="ghost"
										size="icon"
										class="text-destructive hover:bg-destructive/10"
										onclick={() => collaboratorUsers.delete(user)}
									>
										<Trash class="h-4 w-4" />
									</Button>
								</div>
							{/each}
						{/if}
					</div>
				</div>
				<Separator class="my-1" />
				<Button>Save</Button>
			</Dialog.Header>
		</Dialog.Content>
	</Dialog.Root>
{/snippet}

<form method="POST" use:enhance enctype="multipart/form-data">
	<input type="text" hidden name="id" value={data.entity?.id} />
	<Base variant="center-navbar">
		{#snippet left()}
			<div class="flex max-h-fit flex-col gap-1 rounded border p-4">
				<Control name="thumbnailUrl" errors={form.errors.ThumbnailUrl}>
					<div class="group relative">
						<input
							bind:this={fileUpload}
							type="file"
							name="thumbnailUrl"
							class="absolute inset-0 z-10 cursor-pointer opacity-0"
						/>
						<div class="relative">
							<img
								use:preview={{ input: fileUpload, maxSize: 1 }}
								src={(form.data.thumbnailUrl as string) ?? Constants.FALLBACK_IMG}
								alt="logo"
								class="max-h-52 w-full rounded border object-cover"
							/>
							<div
								class="absolute inset-0 bg-black/50 opacity-0 transition-opacity group-hover:opacity-100"
							>
								<Upload class="absolute inset-0 z-1 m-auto size-8 text-white" />
							</div>
						</div>
					</div>
				</Control>

				<Separator class="my-1" />
				{@render collaborators()}
				<Separator class="my-1" />
				<Control label="Name" name="name" errors={form.errors.Name}>
					<Input
						id="name"
						type="name"
						name="name"
						required
						{disabled}
						autocorrect="off"
						autocomplete={null}
						placeholder="Project..."
						aria-invalid={form.errors.Name ? "true" : undefined}
						bind:value={form.data.name}
					/>
				</Control>
				<Control
					label="Max Members"
					name="maxMembers"
					errors={form.errors.MaxMembers}
					description="The max amount of users that can participate on this project together"
				>
					<Input
						id="members"
						type="number"
						name="maxMembers"
						autocorrect="off"
						required
						placeholder="2"
						autocomplete={null}
						aria-invalid={form.errors.MaxMembers ? "true" : undefined}
						bind:value={form.data.maxMembers}
					/>
				</Control>
				<Separator class="my-1" />
				<Control
					label="Public"
					name="public"
					description="Set this to false if you don't wish for anyone to see this project other than maintainers."
					errors={form.errors.Public}
				>
					<Switch
						id="public"
						name="public"
						required
						aria-invalid={form.errors.Public ? "true" : undefined}
						onCheckedChange={(v) => (form.data.public = v)}
						bind:checked={form.data.public}
					/>
				</Control>
				<Control
					label="Enabled"
					name="enabled"
					description="When true, other users can subscribe to this project."
					errors={form.errors.Enabled}
				>
					<Switch
						id="enabled"
						name="enabled"
						required
						aria-invalid={form.errors.Enabled ? "true" : undefined}
						bind:checked={form.data.enabled}
					/>
				</Control>
				<Separator class="my-1" />
				<Button class="w-full" type="submit" disabled={form.isLoading} {formaction}>
					{#if data.entity}
						Update
					{:else}
						Create
					{/if}
				</Button>
			</div>
		{/snippet}

		{#snippet right()}
			<div>
				<h1 class="flex items-center justify-between gap-2 text-2xl font-bold">
					<span>
						Project: <code>@{page.data.session?.preferred_username}/{form.data.name}</code
						>
					</span>
					{#if data.entity}
						<form method="POST" use:enhance>
							<Button
								type="submit"
								size="icon"
								variant="destructive"
								formaction="?/delete"
							>
								<Trash />
							</Button>
						</form>
					{/if}
				</h1>
				<Separator class="my-1" />
				<p class="text-muted-foreground text-sm">
					A project is a collection of your work that includes a thumbnail image, a
					detailed markdown description of the project's proceedings, and a brief summary
					for quick reference.
				</p>
				<Separator class="my-1" />
				<Control label="Description" name="description" errors={form.errors.Description}>
					<Textarea
						id="description"
						name="description"
						placeholder="A shorter description about the project to read about it at a quick glance ..."
						required
						rows={3}
						autocomplete={null}
						aria-invalid={form.errors.Description ? "true" : undefined}
						bind:value={form.data.description}
					/>
				</Control>
				<Separator class="my-1" />
				<div>
					<Control
						label="Markdown"
						name="markdown"
						errors={form.errors.Markdown}
						description="The markdown sheet is the project itself"
					>
						<Markdown
							variant="editor"
							placeholder="# This project is about..."
							maxlength={2048}
							class="max-h-[976px] min-h-96 max-w-[] overflow-y-auto border border-t-0 px-4"
							bind:value={form.data.markdown}
						/>
					</Control>
				</div>
				<Separator class="my-1" />
				<Control
					label="Tags"
					name="tags"
					errors={form.errors.Tags}
					description="Use tags to categorize your project. Tags are used to filter for projects."
				>
					<TagInput />
				</Control>
				<Separator class="my-1" />

				{#if data.entity}
					<Control
						label="Git Repository"
						name="markdown"
						errors={form.errors.Markdown}
						description="Projects are remotely stored on a git server that allows you to track and collaborate on files"
					>
						<div
							class="flex h-full w-full items-center justify-start gap-3 rounded border p-3 text-left"
						>
							<BookMarked class="text-muted-foreground" />
							<div class="flex flex-col">
								<span class="font-medium">{data.entity.gitInfo?.name}</span>
								<span class="text-muted-foreground text-sm">
									{data.entity.gitInfo?.url}
								</span>
							</div>
							<Separator class="flex-1" />
							<Tippy text="View project repository">
								<Button href={data.entity.gitInfo?.url} type="button" variant="outline">
									View
								</Button>
							</Tippy>
							<Tippy text="Copy GIT url to clipboard">
								<Button
									type="button"
									size="icon"
									variant="outline"
									onclick={() => clipboard.copy(data.entity.gitInfo?.url)}
								>
									{#if clipboard.copied}
										<span in:scale>
											<CopyCheck />
										</span>
									{:else}
										<span in:scale>
											<Copy />
										</span>
									{/if}
								</Button>
							</Tippy>
						</div>
					</Control>
					<Separator class="my-2" />
					<Popover.Root>
						<Popover.Trigger class={buttonVariants({ variant: "outline" })}>
							<Code />
							Open in...
						</Popover.Trigger>
						<Popover.Content class="w-90">
							<div class="grid grid-cols-2 gap-2">
								{#each clients as client}
									<Button
										type="button"
										variant="outline"
										class="justify-start px-2"
										href={`${client.url}${data.entity.gitInfo?.url}`}
									>
										<Code />
										{client.title}
									</Button>
								{/each}
							</div>
						</Popover.Content>
					</Popover.Root>
					{@render gitInstructions(data.entity.gitInfo)}
				{:else}
					<Separator class="my-2" />
					<Alert.Root>
						<Terminal class="size-4" />
						<Alert.Title>Managed</Alert.Title>
						<Alert.Description>
							Once you create your project, it will be created for you. You can visit the
							repository and collaborate with others to push changes and track the project
							markdown itself.
						</Alert.Description>
					</Alert.Root>
				{/if}
			</div>
		{/snippet}
	</Base>
</form>
