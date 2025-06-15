<script lang="ts">
	import { ChevronLeft, ChevronRight } from "lucide-svelte";
	import * as Select from "$lib/components/ui/select/index.js";
	import * as Button from "$lib/components/ui/button/index.js";
	import Separator from "$lib/components/ui/separator/separator.svelte";

	let { md, currentStep = 0 }: { md: string; currentStep: number } = $props();

	interface RubricStep {
		id: number;
		title: string;
		content: string;
		level: number;
	}

	// Parse markdown content to extract steps based on headers
	function parseMarkdownSteps(markdown: string): RubricStep[] {
		const lines = markdown.split("\n");
		const steps: RubricStep[] = [];
		let currentContent: string[] = [];
		let currentId = -1;

		for (let i = 0; i < lines.length; i++) {
			const line = lines[i];
			const headerMatch = line.match(/^(#+)\s+(.+)$/);

			if (headerMatch) {
				// If we already have a step in progress, save it
				if (currentId >= 0) {
					steps[currentId].content = currentContent.join("\n");
					currentContent = [];
				}

				const level = headerMatch[1].length; // Number of # symbols
				const title = headerMatch[2];
				currentId = steps.length;

				steps.push({
					id: currentId,
					title,
					content: "",
					level,
				});
			} else if (currentId >= 0) {
				// Add content to current step
				currentContent.push(line);
			}
		}

		// Don't forget the last step
		if (currentId >= 0 && currentContent.length) {
			steps[currentId].content = currentContent.join("\n");
		}

		return steps;
	}

	const steps = $derived(parseMarkdownSteps(md));
	const currentStepData = $derived(
		steps[currentStep] || { id: 0, title: "", content: "", level: 1 },
	);
	const isFirstStep = $derived(currentStep === 0);
	const isLastStep = $derived(currentStep === steps.length - 1);

	// Navigate to previous/next step
	function goToPrevStep() {
		if (!isFirstStep) {
			currentStep--;
		}
	}

	function goToNextStep() {
		if (!isLastStep) {
			currentStep++;
		}
	}

	function handleStepChange(event: CustomEvent) {
		const selectedId = Number(event.detail);
		currentStep = selectedId;
	}

	// Format step titles for the dropdown
	function formatStepTitle(step: RubricStep): string {
		// Add indentation based on header level
		const indent = "  ".repeat(step.level - 1);
		return `${indent}${step.title}`;
	}
</script>

<div class="flex flex-col gap-2">
	<div class="flex items-center justify-between">
		<h3 class="text-lg font-medium">Evaluation</h3>
		<div class="flex items-center gap-2">
			<span class="text-muted-foreground text-sm">
				{currentStep + 1} of {steps.length}
			</span>
		</div>
	</div>

	<Separator class="my-1" />

	<div class="flex items-center gap-2">
		<Select.Root
			type="single"
			value={currentStep.toString()}
			onValueChange={() => handleStepChange}
		>
			<Select.Trigger class="flex-1">
				{steps.length ? currentStepData.title : "No steps available"}
			</Select.Trigger>
			<Select.Content>
				{#each steps as step}
					<Select.Item value={step.id.toString()}>{formatStepTitle(step)}</Select.Item>
				{/each}
			</Select.Content>
		</Select.Root>

		<div class="flex gap-1">
			<Button.Root
				variant="outline"
				size="icon"
				disabled={isFirstStep}
				onclick={goToPrevStep}
			>
				<ChevronLeft class="h-4 w-4" />
			</Button.Root>
			<Button.Root
				variant="outline"
				size="icon"
				disabled={isLastStep}
				onclick={goToNextStep}
			>
				<ChevronRight class="h-4 w-4" />
			</Button.Root>
		</div>
	</div>
</div>
