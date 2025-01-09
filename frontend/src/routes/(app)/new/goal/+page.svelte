<script lang="ts">
	import Base from "$lib/components/base.svelte";
	import Taskcard from "$lib/components/cards/task-card.svelte";
	import * as Accordion from "$lib/components/ui/accordion";
	import * as Avatar from "$lib/components/ui/avatar";
	import Button, { buttonVariants } from "$lib/components/ui/button/button.svelte";
	import Separator from "$lib/components/ui/separator/separator.svelte";
	import Trophy from "lucide-svelte/icons/trophy";
	import ShieldCheck from "lucide-svelte/icons/shield-check";
	import Markdown from "$lib/components/markdown/markdown.svelte";
	import { useDebounce } from "$lib/utils/debounce.svelte";
	import Control from "$lib/components/forms/control.svelte";
	import { Input } from "$lib/components/ui/input";
	import { useForm } from "$lib/components/forms/form.svelte";
	import * as Table from "$lib/components/ui/table";
	import Pen from "lucide-svelte/icons/pen";
	import CircleHelp from "lucide-svelte/icons/circle-help";

	import * as Dialog from "$lib/components/ui/dialog";
	import Tippy from "$lib/components/tippy.svelte";
	import { Textarea } from "$lib/components/ui/textarea";
	import Trash from "lucide-svelte/icons/trash";
	import Ellipsis from "lucide-svelte/icons/ellipsis";
	import * as Popover from "$lib/components/ui/popover";
	import SearchApi from "$lib/components/search-api.svelte";
	import createClient, { createPathBasedClient } from "openapi-fetch";
	import type { paths } from "$lib/api/types.js";
	import { SvelteSet } from "svelte/reactivity";
	import { encodeUUID64 } from "$lib/utils";
	import { page } from "$app/state";
	import type { paths as BackendRoutes } from "$lib/api/types";
	import { dev } from "$app/environment";
	import useAPI from "$lib/utils/api.svelte.js";

	const { data } = $props();
	const { client } = useAPI();
	const { enhance, form, errors, constraints } = useForm(data.form);

	let projects = new SvelteSet<BackendTypes["ProjectDO"]>();
	async function searchGoals(query: string) {
		const { data } = await client.GET("/projects", {
			params: {
				query: {
					"filter[name]": query,
				},
			},
		});

		return data;

		// const params = new URLSearchParams({
		// 	name: query,
		// });

		// const response = await fetch(`/projects?${params}`);
		// const json = (await response.json()) as BackendTypes["ProjectDO"][];
		// return json;
	}
</script>

<form method="POST" use:enhance>
	<Base variant="center-navbar">
		{#snippet left()}
			<div class="flex h-min flex-col gap-2 rounded border p-4">
				<img
					src="https://github.com/w2wizard.png"
					alt="logo"
					class="max-h-64 rounded border object-cover"
				/>
				<Control label="Name" name="name" errors={$errors.name}>
					<Input
						id="name"
						type="text"
						name="name"
						autocorrect="off"
						autocomplete={null}
						placeholder="Cursus..."
						aria-invalid={$errors.name ? "true" : undefined}
						bind:value={$form.name}
						{...$constraints.name}
					/>
				</Control>
				<Separator />
				<Button class="w-full" type="submit">Create</Button>
			</div>
			<Separator class="my-2 md:hidden" />
		{/snippet}

		{#snippet right()}
			<div class="flex flex-col gap-2 rounded border p-4">
				<h1 class="center-content text-3xl">
					<Trophy />
					Goal: {$form.name}
				</h1>

				<Control
					label="Goal Markdown"
					name="markdown"
					description="A more in depth styled markdown sheet to explain what this goal aims to teach someone."
					errors={$errors.description}
				>
					<Markdown
						variant="editor"
						placeholder="# This goal encompasses..."
						bind:value={$form.markdown}
						{...$constraints.markdown}
					/>
				</Control>
				<Control
					label="Summary Description"
					name="description"
					description="A short description that tells other quickly what this project is about"
					errors={$errors.description}
				>
					<Textarea
						id="description"
						name="description"
						autocomplete={null}
						placeholder="A goal where you learn..."
						required
						class="max-h-32"
						aria-invalid={$errors.description ? "true" : undefined}
						bind:value={$form.description}
						{...$constraints.description}
					/>
				</Control>
				<Separator />

				<Control
					label="Project Composition"
					name="description"
					description="You can manage which projects belong to this learning goal"
					errors={$errors.description}
				>
					<!-- <SearchGoal /> -->
					<SearchApi
						endpointFn={searchGoals}
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
								<input hidden name="projects" bind:value={$form.projects[i]} />
								<Table.Row>
									<Table.Cell class="font-medium">
										<a
											class="capitalize underline decoration-wavy"
											href="/users/{encodeUUID64(
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
											href="/users/{encodeUUID64(p.creator?.id)}"
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
				<!-- <Button variant="secondary">Add Project</Button> -->
			</div>
		{/snippet}
	</Base>
</form>
