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

	import Trash from "lucide-svelte/icons/trash";
	import ShieldAlert from "lucide-svelte/icons/shield-alert";
	import Terminal from "lucide-svelte/icons/terminal";
	import BookMarked from "lucide-svelte/icons/book-marked";

	import * as Dialog from "$lib/components/ui/dialog";
	import { Button, buttonVariants } from "$lib/components/ui/button/index.js";
	import * as Tabs from "$lib/components/ui/tabs";
	import * as Alert from "$lib/components/ui/alert/index.js";
	import { useForm } from "$lib/utils/form.svelte";
	import type { PageData } from "./$types";
	import { Constants } from "$lib/utils";
	import { toast } from "svelte-sonner";
	import { preview } from "$lib/utils/image.svelte";
	import { Slider } from "$lib/components/ui/slider";
	import { page } from "$app/state";
	import { useDebounce } from "$lib/utils/debounce.svelte";
	import RemoteSelector from "$lib/components/remote-selector.svelte";

	let { data } = $props();
	const { enhance, form } = useForm(data.form, {
		confirm: true,
	});

	const debounce = useDebounce(450);
	const formaction = $derived(data.entity ? `?/update` : "?/create");
	const name = $derived(form.data.gitUrl);

	let fileUpload: HTMLInputElement;

	let markdownText = $state<Promise<string>>();
	async function fetchMarkdown() {
		if (!form.data.gitUrl) return;

		let url = form.data.gitUrl.trim();
		const branch = form.data.gitBranch ?? "Norme";

		// Ensure URL ends with .git
		if (!url.endsWith(".git")) {
			toast.error("URL must end with .git");
			return;
		}

		// Remove .git suffix
		url = url.slice(0, -4);

		try {
			// Try different URL patterns based on provider
			const urlPatterns = [
				// GitHub
				url.replace("github.com", "raw.githubusercontent.com") + `/${branch}/README.md`,
				// GitLab
				`${url}/-/raw/${branch}/README.md`,
				// Gitea
				`${url}/raw/branch/${branch}/README.md`,
			];

			for (const pattern of urlPatterns) {
				try {
					const response = await fetch(pattern);
					if (response.ok) {
						markdownText = response.text();
						return;
					}
				} catch {}
			}

			toast.error("Unable to get README.md, perhaps it doesn't allow access ?");
		} catch (error) {
			toast.error("Failed to fetch README.md. Make sure the repository is accessible.");
		}
	}
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
							src={(form.data.image as string) ?? Constants.FALLBACK_IMG}
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
						aria-invalid={form.errors.name ? "true" : undefined}
						bind:value={form.data.name}
						{...form.constraints.name}
					/>
				</Control>
				<Control
					label="Max Members"
					name="maxMembers"
					errors={form.errors.maxMembers}
					description="The max amount of users that can participate on this project together"
				>
					<Input
						id="members"
						type="number"
						name="maxMembers"
						autocorrect="off"
						placeholder="2"
						autocomplete={null}
						aria-invalid={form.errors.maxMembers ? "true" : undefined}
						bind:value={form.data.maxMembers}
						{...form.constraints.maxMembers}
					/>
				</Control>
				<Separator class="my-1" />
				<Control
					label="Public"
					name="public"
					description="Set this to false if you don't wish for anyone to see this project other than maintainers."
					errors={form.errors.public}
				>
					<Switch
						id="public"
						name="public"
						aria-invalid={form.errors.public ? "true" : undefined}
						onCheckedChange={(v) => (form.data.public = v)}
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
						aria-invalid={form.errors.enabled ? "true" : undefined}
						bind:checked={form.data.enabled}
						{...form.constraints.enabled}
					/>
				</Control>
				<Separator class="my-1" />
				<Button class="w-full" type="submit" disabled={form.submitting} {formaction}>
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
						<Button type="submit" size="icon" variant="destructive" formaction="?/delete" >
							<Trash />
						</Button>
					{/if}
				</h1>
				<Separator class="my-1" />
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
				{#if data.entity}
					<Button
						variant="ghost"
						class="flex h-full w-full items-center justify-start gap-3 text-left"
					>
						<BookMarked class="text-muted-foreground" />
						<div class="flex flex-col">
							<span class="font-medium">DEMO</span>
							<span class="text-muted-foreground text-sm">DEM</span>
						</div>
					</Button>

					{#await markdownText}
						Loading...
					{:then text}
						<Markdown variant="viewer" value={text} />
					{:catch}
						Error!
					{/await}
				{:else}
					<Control
						label="Markdown"
						name="markdown"
						errors={form.errors.markdown}
						description="The markdown sheet is the project itself"
					>
						<Markdown
							variant="editor"
							placeholder="# This project is about..."
							bind:value={form.data.markdown}
							{...form.constraints.markdown}
						/>
					</Control>
					<Alert.Root>
						<Terminal class="size-4" />
						<Alert.Title>Managed</Alert.Title>
						<Alert.Description>A repository will be made for you</Alert.Description>
					</Alert.Root>
				{/if}

				<!-- <Tabs.Root value="markdown">
					<Tabs.List class="w-full">
						<Tabs.Trigger class="w-full" value="github">Github</Tabs.Trigger>
						<Tabs.Trigger class="w-full" value="thirdparty">Thirdparty</Tabs.Trigger>
						<Tabs.Trigger class="w-full" value="markdown">Markdown</Tabs.Trigger>
					</Tabs.List>
					<Tabs.Content value="github">
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
						<Control label="Git Url" name="giturl" errors={form.errors.gitUrl}>
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
									oninput={() => debounce(fetchMarkdown)}
									placeholder="https://git-provider.com/scope/repo"
									class="rounded-l-none border-l-0"
									aria-invalid={form.errors.gitUrl ? "true" : undefined}
									bind:value={form.data.gitUrl}
									{...form.constraints.gitUrl}
								/>
							</div>
						</Control>

						<Control label="Git Branch" name="gitbranch" errors={form.errors.gitBranch}>
							<Input
								id="gitbranch"
								type="text"
								name="gitbranch"
								autocorrect="off"
								autocomplete={null}
								oninput={() => debounce(fetchMarkdown)}
								placeholder="master"
								aria-invalid={form.errors.gitBranch ? "true" : undefined}
								bind:value={form.data.gitBranch}
								{...form.constraints.gitBranch}
							/>
						</Control>

						{JSON.stringify(form.data)}

						{#await markdownText}
							Loading...
						{:then text}
							<Markdown variant="viewer" value={text} />
						{:catch}
							Error!
						{/await}

						{@render gitAlert()}
					</Tabs.Content>
					<Tabs.Content value="markdown" class="max-w-50">
						<Control label="Git Branch" name="gitbranch" errors={form.errors.markdown}>
							<Markdown
								variant="editor"
								placeholder="# This project is about..."
								bind:value={form.data.markdown}
								{...form.constraints.markdown}
							/>
						</Control>
					</Tabs.Content>
				</Tabs.Root> -->
			</div>
		{/snippet}
	</Base>
</form>
