<!-- <script lang="ts">
	import { writable } from "svelte/store";
	import { mode } from "mode-watcher";

	import "@xyflow/svelte/dist/style.css";
	import Base from "$lib/components/base.svelte";
	import Input from "$lib/components/ui/input/input.svelte";
	import Control from "$lib/components/forms/control.svelte";
	import Pen from "lucide-svelte/icons/pen";
	import Import from "lucide-svelte/icons/file-up";
	import Export from "lucide-svelte/icons/file-down";
	import CircleHelp from "lucide-svelte/icons/circle-help";
	import * as Dialog from "$lib/components/ui/dialog";
	import * as Select from "$lib/components/ui/select";
	import { Button, buttonVariants } from "$lib/components/ui/button";
	import * as Tabs from "$lib/components/ui/tabs";
	import * as Alert from "$lib/components/ui/alert";
	import Tippy from "$lib/components/tippy.svelte";
	import { Separator } from "$lib/components/ui/separator";
	import { Textarea } from "$lib/components/ui/textarea";
	import Markdown from "$lib/components/markdown/markdown.svelte";
	import Switch from "$lib/components/ui/switch/switch.svelte";
	import ProjectNode from "$lib/components/nodes/project-node.svelte";
	import { useForm } from "$lib/utils/form.svelte.js";
	import {
		SvelteFlow,
		Controls,
		Background,
		BackgroundVariant,
		Position,
		MiniMap,
		Panel,
		type ColorMode,
	} from "@xyflow/svelte";
	import "@xyflow/svelte/dist/style.css";
	const nodeDefaults = {
		sourcePosition: Position.Right,
		targetPosition: Position.Left,
	};
	let nodes = $state.raw([
		{
			id: "A",
			position: { x: 0, y: 150 },
			data: { label: "A" },
			...nodeDefaults,
		},
		{ id: "B", position: { x: 250, y: 0 }, data: { label: "B" }, ...nodeDefaults },
		{ id: "C", position: { x: 250, y: 150 }, data: { label: "C" }, ...nodeDefaults },
		{ id: "D", position: { x: 250, y: 300 }, data: { label: "D" }, ...nodeDefaults },
	]);
	let edges = $state.raw([
		{ id: "A-B", source: "A", target: "B" },
		{ id: "A-C", source: "A", target: "C" },
		{ id: "A-D", source: "A", target: "D" },
	]);
	let colorMode: ColorMode = $state("system");

	const { data } = $props();
	const { enhance, form } = useForm(data.form, {
		confirm: true,
	});
</script>

<form method="POST" use:enhance>
	<input type="hidden" name="description" value={form.data.description} />
	<input type="hidden" name="markdown" value={form.data.markdown} />
	<Base>
		{#snippet left()}
			<div>
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
				<Dialog.Root>
					<Tippy
						text="You must define a markdown sheet and a description for this cursus"
					>
						<h4 class="flex items-center gap-2 text-sm font-semibold">
							Markdown & Description
							<CircleHelp size={16} />
						</h4>
					</Tippy>

					<Dialog.Trigger
						type="button"
						style="width: 100%;"
						class={buttonVariants({ variant: "outline" })}
					>
						<Pen />
						Set Markdown
					</Dialog.Trigger>
					<Dialog.Content class="block max-w-2xl">
						<Dialog.Header>
							<Dialog.Title>Description</Dialog.Title>
							<Dialog.Description>
								You must provide a markdown sheet to descripe the cursus as well as a
								shorter description about the cursus.
							</Dialog.Description>
						</Dialog.Header>
						<Control label="Markdown" name="markdown" errors={form.errors.description}>
							<Markdown
								variant="editor"
								placeholder="# This cursus is about..."
								bind:value={form.data.markdown}
								{...form.constraints.markdown}
							/>
						</Control>
						<Control
							label="Description"
							name="description"
							errors={form.errors.description}
						>
							<Textarea
								id="description"
								name="description"
								autocomplete={null}
								placeholder="A cursus where you learn..."
								required
								class="max-h-32"
								aria-invalid={form.errors.description ? "true" : undefined}
								bind:value={form.data.description}
								{...form.constraints.description}
							/>
						</Control>
					</Dialog.Content>
				</Dialog.Root>
				<Separator class="my-3" />
				<Control
					label="Public"
					name="public"
					description="Set this to false if you don't wish for anyone to see this cursus other than the creator."
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
					description="When true, other users can subscribe to this cursus."
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
				<Separator class="my-3" />
				<Control
					label="Cursus Variant"
					name="enabled"
					description="A cursus can be of different variants such as Fixed where it uses a defined track or dynamic where the track is defined over time by usage."
					errors={form.errors.kind}
				>
					<Select.Root type="single" bind:value={form.data.kind}>
						<Select.Trigger>
							{form.data.kind}
						</Select.Trigger>
						<Select.Content>
							{#each ["Dynamic", "Fixed"] as type}
								<Select.Item value={type}>{type}</Select.Item>
							{/each}
						</Select.Content>
					</Select.Root>
				</Control>
				<Separator class="my-3" />
				<Separator class="my-3" />
				<Button class="w-full" type="submit">Create</Button>
			</div>
		{/snippet}

		{#snippet right()}
			<SvelteFlow bind:nodes bind:edges {colorMode} colorModeSSR={"dark"} fitView>
				<Controls position="top-right" />
				<Background variant={BackgroundVariant.Dots} />

				<Panel style="margin: 0px;" position="top-left" class="flex gap-2 bg-card rounded-br-lg border-b border-r p-2">
					<Select.Root type="single" bind:value={colorMode}>
						<Select.Trigger
							class={buttonVariants({
								variant: "secondary",
								class: "w-[100px] justify-between capitalize",
							})}>{colorMode}</Select.Trigger
						>
						<Select.Content>
							<Select.Item value="light">Light</Select.Item>
							<Select.Item value="dark">Dark</Select.Item>
							<Select.Item value="system">System</Select.Item>
						</Select.Content>
					</Select.Root>
					<Separator orientation="vertical" />
					<Button variant="secondary" size="icon">
						<Import />
					</Button>
					<Button variant="secondary" size="icon">
						<Export />
					</Button>
				</Panel>
			</SvelteFlow>
		{/snippet}
	</Base>
</form> -->
