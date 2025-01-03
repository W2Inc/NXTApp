<script lang="ts">
	import Base from "$lib/components/base.svelte";
	import type { IconLink } from "$lib/types";
	import Sparkles from "lucide-svelte/icons/sparkles";
	import Trophy from "lucide-svelte/icons/trophy";
	import Archive from "lucide-svelte/icons/archive";
	import Plus from "lucide-svelte/icons/plus";
	import { Button } from "$lib/components/ui/button";
	import Separator from "$lib/components/ui/separator/separator.svelte";
	import Tippy from "$lib/components/tippy.svelte";
	import CircleHelp from "lucide-svelte/icons/circle-help";

	type ReviewSelection = IconLink & {
		description: string;
	};

	const items: ReviewSelection[] = [
		{
			icon: Sparkles,
			title: "Create Cursus",
			href: `/new/cursus`,
			description:
				"Design a structured learning path with multiple projects and goals. Perfect for creating comprehensive educational programs or training sequences.",
		},
		{
			icon: Trophy,
			title: "Create Learning Goal",
			href: `/new/goal`,
			description:
				"Define specific learning objectives and milestones. Set clear, measurable targets for skills development and knowledge acquisition.",
		},
		{
			icon: Archive,
			title: "Create Project",
			href: `/new/project`,
			description:
				"Start a new project with detailed documentation and resources. Ideal for assignments, practical exercises, or hands-on learning activities.",
		},
	];
</script>

{#snippet action({ icon, href, title, description }: ReviewSelection)}
	<div class="w-full rounded border p-6">
		<div class="flex justify-between">
			<h2 class="center-content text-2xl font-bold">
				<svelte:component this={icon} size="32"></svelte:component>
				{title}
			</h2>
			<Button {href}>
				Create
				<Plus size={32} />
			</Button>
		</div>
		<Separator class="my-3" />
		<p class="text-sm text-muted-foreground">
			{description}
		</p>
	</div>
{/snippet}

<Base variant="center">
	{#snippet left()}
		<Tippy
			text="Creating a new project requires you write down a markdown sheet detailing what the project is about."
		>
			<h1 class="center-content gap-2 text-2xl font-bold">
				Create new material
				<CircleHelp size={16} />
			</h1>
		</Tippy>
		<p class="text-muted-foreground text-sm">
			A project is a collection of your work that includes a thumbnail image, a detailed
			markdown description of the project's proceedings, and a brief summary for quick
			reference.
		</p>

		<Separator class="my-3" />
	{/snippet}
	{#snippet right()}
		<div class="flex flex-col gap-2">
			{#each items as item}
				{@render action(item)}
			{/each}
		</div>
	{/snippet}
</Base>
