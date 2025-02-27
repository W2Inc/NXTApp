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

	import { Textarea } from "$lib/components/ui/textarea";
	import Trash from "lucide-svelte/icons/trash";
	import SearchApi from "$lib/components/search-api.svelte";
	import { SvelteSet } from "svelte/reactivity";
	import { encodeID } from "$lib/utils";
	import { page } from "$app/state";
	import Switch from "$lib/components/ui/switch/switch.svelte";
	import { useForm } from "$lib/utils/form.svelte.js";

	const { data } = $props();
	const { enhance, form } = useForm(data.form, {
		confirm: true,
	});

	// TODO: Use project DO from form as initial
	let projects = new SvelteSet<BackendTypes["ProjectDO"]>(data.entity?.projects);
	async function searchGoals(query: string) {
		const response = await fetch(
			`/projects?${new URLSearchParams({
				name: query,
			})}`,
		);

		return (await response.json()) as BackendTypes["ProjectDO"][];
	}

	const formaction = $derived(
		data.entity ? `?/update` : "?/create",
	);
</script>

<form method="POST" use:enhance enctype="multipart/form-data">
	<Base variant="center-navbar">
		{#snippet left()}
			<div class="flex h-min flex-col gap-2 rounded border p-4">
				<img
					src="https://github.com/w2wizard.png"
					alt="logo"
					class="max-h-64 rounded border object-cover"
				/>
				<Control label="Name" name="name" errors={form.errors.name}>
					<Input
						id="name"
						type="text"
						name="name"
						autocorrect="off"
						autocomplete={null}
						placeholder="Cursus..."
						aria-invalid={form.errors.name ? "true" : undefined}
						bind:value={form.data.name}
						{...form.constraints.name}
					/>
				</Control>
				<Separator />
				<Control
					label="Public"
					name="public"
					description="Set this to false if you don't wish for anyone to see this goal other than the creator."
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
					description="When true, other users can subscribe to this goal. If public is false people can still subscribe if they find the link."
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
			<div class="flex flex-col gap-2 rounded border p-4">
				<h1 class="center-content text-3xl">
					<Trophy />
					Goal: {form.data.name}
				</h1>

				<Control
					label="Goal Markdown"
					name="markdown"
					description="A more in depth styled markdown sheet to explain what this goal aims to teach someone."
					errors={form.errors.markdown}
				>
					<Markdown
						variant="editor"
						placeholder="# This goal encompasses..."
						bind:value={form.data.markdown}
						{...form.constraints.markdown}
					/>
				</Control>
				<Control
					label="Summary Description"
					name="description"
					description="A short description that tells other quickly what this project is about"
					errors={form.errors.description}
				>
					<Textarea
						id="description"
						name="description"
						autocomplete={null}
						placeholder="A goal where you learn..."
						required
						class="max-h-32"
						aria-invalid={form.errors.description ? "true" : undefined}
						bind:value={form.data.description}
						{...form.constraints.description}
					/>
				</Control>
				<Separator />

				<Control
					label="Project Composition"
					name="description"
					description="You can manage which projects belong to this learning goal"
					errors={form.errors.projects?._errors}
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
				<!-- <Button variant="secondary">Add Project</Button> -->
			</div>
		{/snippet}
	</Base>
</form>
