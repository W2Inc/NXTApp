<script lang="ts">
	import Base from "$lib/components/base.svelte";
	import * as Avatar from "$lib/components/ui/avatar";
	import Button from "$lib/components/ui/button/button.svelte";
	import Separator from "$lib/components/ui/separator/separator.svelte";
	import Trophy from "lucide-svelte/icons/trophy";
	import Markdown from "$lib/components/markdown/markdown.svelte";
	import Control from "$lib/components/forms/control.svelte";
	import { Input } from "$lib/components/ui/input";
	import * as Table from "$lib/components/ui/table";
	import CircleHelp from "lucide-svelte/icons/circle-help";
	import GitBranch from "lucide-svelte/icons/git-branch";
	import ShieldAlert from "lucide-svelte/icons/shield-alert";
	import FileBox from "lucide-svelte/icons/file-box";
	import Upload from "lucide-svelte/icons/upload";

	import { Textarea } from "$lib/components/ui/textarea";
	import Trash from "lucide-svelte/icons/trash";
	import SearchApi from "$lib/components/search-api.svelte";
	import { SvelteSet } from "svelte/reactivity";
	import { Constants, encodeID } from "$lib/utils";
	import { page } from "$app/state";
	import Switch from "$lib/components/ui/switch/switch.svelte";
	import { useForm } from "$lib/utils/form.svelte.js";
	import * as Tabs from "$lib/components/ui/tabs";
	import * as Alert from "$lib/components/ui/alert/index.js";
	import { preview } from "$lib/utils/image.svelte.js";

	const { data } = $props();
	const { enhance, form } = useForm(data.form, {
		confirm: true,
	});

	// TODO: Use project DO from form as initial
	let projects = new SvelteSet<BackendTypes["ProjectDO"]>(data.entity?.projects);
	async function searchProjects(query: string) {
		const response = await fetch(
			`/projects?${new URLSearchParams({
				name: query,
			})}`,
		);

		return (await response.json()) as BackendTypes["ProjectDO"][];
	}

	const user = page.data.session!.user!;
	const formaction = $derived(data.entity ? `?/update` : "?/create");
	const warn = $derived([...projects].some((p) => p.creator?.id !== user.id));
	const rubricName = $derived.by(
		() => `@${user.username}/${form.data.name.replaceAll(" ", "-")}`,
	);

	let fileUpload: HTMLInputElement;
</script>

{#snippet gitAlert()}
	<Separator class="my-2" />
	<Alert.Root>
		<CircleHelp class="size-4" />
		<Alert.Title>Rubric structure</Alert.Title>
		<Alert.Description>
			Rubrics using git source control need have at least 2 files present in their
			repository:
			<ul class="mb-1 list-disc pl-4">
				<li class="underline">README.md</li>
				<li class="underline">RUBRIC.md</li>
			</ul>
			These files need to be present at the
			<bold class="font-bold underline">root</bold> of the repository.
		</Alert.Description>
	</Alert.Root>
{/snippet}

{#snippet ownerAlert()}
	<Separator class="my-2" />
	<Alert.Root variant="warning">
		<CircleHelp class="size-4" />
		<Alert.Title>Creating rubric for another project</Alert.Title>
		<Alert.Description>
			You've selected a project that you did not create. You are able to submit this
			rubric but the owner must approve your request first.
		</Alert.Description>
	</Alert.Root>
{/snippet}

<form method="POST" use:enhance>
	<Base variant="center-navbar">
		{#snippet left()}
			<div class="flex h-min flex-col gap-2 rounded border p-4">
				<Control label="Name" name="name" errors={form.errors.name}>
					<Input
						id="name"
						type="text"
						name="name"
						autocorrect="off"
						autocomplete={null}
						placeholder="Rubric..."
						aria-invalid={form.errors.name ? "true" : undefined}
						bind:value={form.data.name}
						{...form.constraints.name}
					/>
				</Control>
				<Separator />
				<Control
					label="Public"
					name="public"
					description="Set this to false if you don't wish for anyone to see this rubric other than the creator."
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
					description="When true, other users can subscribe to this rubric. If public is false people can still subscribe if they find the link."
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
				<Button class="w-full" type="submit" disabled={form.submitting} {formaction}>
					{#if data.entity}
						Update
					{:else}
						Create
					{/if}
				</Button>
			</div>
			<Separator class="my-2 md:hidden" />
		{/snippet}

		{#snippet right()}
			<div class="flex h-fit flex-col gap-2 rounded border p-4">
				<h1 class="center-content text-2xl">
					<FileBox />
					Rubric: <code>{rubricName}</code>
				</h1>
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
				<Separator />

				<Control
					label="Projects"
					name="description"
					description="An overview to which projects this rubric is used. Note that adding projects not made by you require the project creator to accept your request that this rubric can be used for that project."
					errors={form.errors.projects?._errors}
				>
					{#if projects.size === 0}
						<SearchApi
							endpointFn={searchProjects}
							placeholder="Search for projects..."
							onSelect={(v) => {
								projects.add(v);
							}}
						>
							{#snippet item({ value })}
								<div class="center-content w-full justify-between">
									<span
										title={value.name}
										class="max-w-[75%] overflow-hidden text-ellipsis font-bold"
									>
										{value.name}
									</span>
									<Avatar.Root class="size-6">
										<Avatar.Image
											src={value.creator?.avatarUrl}
											alt="@{value.creator?.login}"
										/>
										<Avatar.Fallback>US</Avatar.Fallback>
									</Avatar.Root>
								</div>
							{/snippet}
						</SearchApi>
					{/if}

					<Table.Root class="mt-2 border">
						<Table.Header class="rounded">
							<Table.Row>
								<Table.Head class="w-[100px]">Name</Table.Head>
								<Table.Head>Creator</Table.Head>
								<Table.Head>Description</Table.Head>
								<Table.Head class="text-right">Actions</Table.Head>
							</Table.Row>
						</Table.Header>
						<Table.Body>
							{#each projects as p, i}
								<Table.Row>
									<Table.Cell class="font-medium">
										<input hidden name="projects[]" value={form.data.projects[i]} />
										<a
											class="capitalize underline decoration-wavy"
											href="/users/{encodeID(
												page.data.session?.user?.id ?? '',
											)}/projects/{p.slug}"
											target="_blank"
											rel="noopener noreferrer"
										>
											{p.name}
										</a>
									</Table.Cell>
									<Table.Cell>
										<a
											class="text-primary underline decoration-wavy"
											href="/users/{encodeID(p.creator?.id)}"
											target="_blank"
											rel="noopener noreferrer"
										>
											{p.creator?.displayName ?? p.creator?.login}
										</a>
									</Table.Cell>
									<Table.Cell class="max">
										{p.description}
									</Table.Cell>
									<Table.Cell class="text-right">
										<Button
											type="button"
											size="icon"
											variant="destructive"
											onclick={() => projects.delete(p)}
										>
											<Trash size={16} />
										</Button>
									</Table.Cell>
								</Table.Row>
							{/each}
						</Table.Body>
					</Table.Root>
				</Control>
				{#if warn}
					{@render ownerAlert()}
				{/if}
			</div>
		{/snippet}
	</Base>
</form>
