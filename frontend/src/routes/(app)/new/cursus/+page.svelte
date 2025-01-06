<script lang="ts">
	import { writable } from "svelte/store";
	import { SvelteFlow, Background, Controls, MiniMap, type Edge, Position, type Node } from "@xyflow/svelte";
	import { mode } from "mode-watcher";

	import "@xyflow/svelte/dist/style.css";
	import Base from "$lib/components/base.svelte";
	import Input from "$lib/components/ui/input/input.svelte";
	import Control from "$lib/components/forms/control.svelte";
	import { useForm } from "$lib/components/forms/form.svelte";
	import Pen from "lucide-svelte/icons/pen";
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
	import SearchProject from "$lib/components/search-project.svelte";
	import ProjectNode from "$lib/components/nodes/project-node.svelte";

  const nodeTypes = {
    selectorNode: ProjectNode
  };

  const bgColor = writable('#1A192B');

  const initialNodes: Node[] = [
    {
      id: '1',
      type: 'input',
      data: { label: 'An input node' },
      position: { x: 0, y: 50 },
      sourcePosition: Position.Right
    },
    {
      id: '2',
      type: 'selectorNode',
      data: { color: bgColor },
      style: 'border: 1px solid #777; padding: 10px;',
      position: { x: 300, y: 50 }
    },
    {
      id: '3',
      type: 'output',
      data: { label: 'Output A' },
      position: { x: 650, y: 25 },
      targetPosition: Position.Left
    },
    {
      id: '4',
      type: 'output',
      data: { label: 'Output B' },
      position: { x: 650, y: 100 },
      targetPosition: Position.Left
    }
  ];

  const initialEdges: Edge[] = [
    {
      id: 'e1-2',
      source: '1',
      target: '2',
      animated: true,
      style: 'stroke: #fff;'
    },
    {
      id: 'e2a-3',
      source: '2',
      target: '3',
      sourceHandle: 'a',
      animated: true,
      style: 'stroke: #fff;'
    },
    {
      id: 'e2b-4',
      source: '2',
      target: '4',
      sourceHandle: 'b',
      animated: true,
      style: 'stroke: #fff;'
    }
  ];

  const nodes = writable<Node[]>(initialNodes);
  const edges = writable(initialEdges);



	const { data } = $props();
	const { enhance, form, errors, constraints } = useForm(data.form);

</script>

<form method="POST" use:enhance>
	<input type="hidden" name="description" value={$form.description} />
	<input type="hidden" name="markdown" value={$form.markdown} />
	<Base>
		{#snippet left()}
			<div>
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
					<Dialog.Content class="max-w-2xl block">
						<Dialog.Header>
							<Dialog.Title>Description</Dialog.Title>
							<Dialog.Description>
								You must provide a markdown sheet to descripe the cursus as well as a
								shorter description about the cursus.
							</Dialog.Description>
						</Dialog.Header>
						<!-- Branch -->
						<Control label="Markdown" name="markdown" errors={$errors.description}>
							<Markdown
								variant="editor"
								placeholder="# This cursus is about..."
								bind:value={$form.markdown}
								{...$constraints.markdown}
							/>
						</Control>
						<Control label="Description" name="description" errors={$errors.description}>
							<Textarea
								id="description"
								name="description"
								autocomplete={null}
								placeholder="A cursus where you learn..."
								required
								class="max-h-32"
								aria-invalid={$errors.description ? "true" : undefined}
								bind:value={$form.description}
								{...$constraints.description}
							/>
						</Control>
					</Dialog.Content>
				</Dialog.Root>
				<Separator class="my-3" />
				<Control
					label="Public"
					name="public"
					description="Set this to false if you don't wish for anyone to see this cursus other than the creator."
					errors={$errors.public}
				>
					<Switch
						id="public"
						name="public"
						required
						aria-invalid={$errors.public ? "true" : undefined}
						bind:checked={$form.public}
						{...$constraints.public}
					/>
				</Control>
				<Control
					label="Enabled"
					name="enabled"
					description="When true, other users can subscribe to this cursus."
					errors={$errors.enabled}
				>
					<Switch
						id="enabled"
						name="enabled"
						required
						aria-invalid={$errors.enabled ? "true" : undefined}
						bind:checked={$form.enabled}
						{...$constraints.enabled}
					/>
				</Control>
				<Separator class="my-3" />
				<Control
					label="Cursus Variant"
					name="enabled"
					description="A cursus can be of different variants such as Fixed where it uses a defined track or dynamic where the track is defined over time by usage."
					errors={$errors.kind}
				>
					<Select.Root type="single" bind:value={$form.kind}>
						<Select.Trigger>
							{$form.kind}
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
			<div style=" color: black;">
				<SvelteFlow {nodes} {edges} {nodeTypes} colorMode={$mode ?? "system"} minZoom={0} fitView>
					<Background />
					<Controls position="top-right" />
				</SvelteFlow>
			</div>
		{/snippet}
	</Base>
</form>

<style>
	.scramble-button {
		position: absolute;
		right: 10px;
		top: 30px;
		z-index: 4;
	}
</style>
