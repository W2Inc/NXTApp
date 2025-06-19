<script lang="ts">
	import Button from "$lib/components/ui/button/button.svelte";
	import { ArrowDown, FileUp, AlertTriangle, MessageSquare } from "lucide-svelte";
	import type { VFSFile } from "../types";
	import Separator from "$lib/components/ui/separator/separator.svelte";
	import Textarea from "$lib/components/ui/textarea/textarea.svelte";
	import Badge from "$lib/components/ui/badge/badge.svelte";
	import { toast } from "svelte-sonner";

	interface Props {
		file: VFSFile;
	}

	const { file }: Props = $props();
	let success = $state(true);
	let showRubricPanel = $state(false);
	let rubricFeedback = $state("");

	function handleUpload(e: Event) {
		const input = e.target as HTMLInputElement;
		const uploadedFile = input.files?.[0];

		if (uploadedFile) {
			// Here you would implement the actual upload logic
			toast.success(`${uploadedFile.name} has been uploaded for review`);
			input.value = "";
		}
	}

	function submitFeedback() {
		// Here you would implement the logic to submit the textual feedback
		toast.success(`Your correction notes have been saved`);
		showRubricPanel = false;
	}
</script>

<div class="relative flex h-full w-full flex-col">

	<!-- Controls Menu -->
	<menu class="bg-muted sticky top-0 z-50 flex items-center gap-3 p-3 shadow">

		<!-- Download -->
		<Button
			variant="outline"
			title="Download document"
			onclick={() => {
				const link = document.createElement("a");
				link.href = file.content;
				link.download = file.name || "document.docx";
				link.click();
			}}
		>
			Download
			<ArrowDown class="ml-2 h-4 w-4" />
		</Button>

		<!-- Upload correction docx -->
		<Button variant="secondary" class="relative" title="Upload correction document">
			<FileUp class="mr-2 h-4 w-4" />
			Upload Corrections
			<input
				type="file"
				accept=".docx,.doc"
				class="absolute inset-0 cursor-pointer opacity-0"
				onchange={handleUpload}
			/>
		</Button>

		<!-- Rubric feedback toggle -->
		<Button
			variant={showRubricPanel ? "default" : "outline"}
			onclick={() => (showRubricPanel = !showRubricPanel)}
		>
			<MessageSquare class="mr-2 h-4 w-4" />
			{showRubricPanel ? "Hide Feedback" : "Add Feedback"}
		</Button>
	</menu>

		<!-- Preview Banner -->
		<div class="border-b border-amber-200 bg-amber-50 p-2 text-center text-amber-800">
			<AlertTriangle class="mr-1 inline-block h-4 w-4" />
			<span class="font-medium">Preview Only</span>
			<span class="ml-1 text-sm">— Submit corrections using the options below</span>
		</div>

	<!-- Rubric Feedback Panel -->
	{#if showRubricPanel}
		<div class="border-b bg-white p-4 shadow-sm">
			<h3 class="mb-2 font-medium">Correction Notes</h3>
			<Textarea
				placeholder="Enter your feedback and correction notes here..."
				class="mb-3 min-h-[100px]"
				bind:value={rubricFeedback}
			/>
			<div class="flex justify-end gap-2">
				<Button variant="outline" onclick={() => (showRubricPanel = false)}>
					Cancel
				</Button>
				<Button onclick={submitFeedback}>Submit Feedback</Button>
			</div>
		</div>
	{/if}

	<!-- Document Viewer -->
	<div class="relative flex-1">
		<iframe
			title="Word Document"
			src="https://view.officeapps.live.com/op/embed.aspx?src={file.content}"
			width="100%"
			height="100%"
			frameborder="0"
			onerror={() => (success = false)}
			class="absolute inset-0 inset-shadow-xl"
			>This is an embedded <a target="_blank" href="http://office.com">Microsoft Office</a
			>
			document, powered by
			<a target="_blank" href="http://office.com/webapps">Office Online</a>.
		</iframe>

		<!-- Error State -->
		{#if !success}
			<div
				class="absolute inset-0 flex flex-col items-center justify-center bg-white/90 p-4"
			>
				<div class="max-w-md rounded border border-red-400 bg-red-100 p-4 text-red-700">
					<p class="mb-2 text-lg font-bold">Error Loading Document</p>
					<p>
						The document could not be loaded. Please check the file format or try again
						later.
					</p>
				</div>
			</div>
		{/if}

		<!-- Watermark -->
		<div class="pointer-events-none absolute bottom-0 right-0 m-20">
			<div class="rotate-[-30deg] scale-150 transform text-lg font-bold text-red-500/20">
				PREVIEW ONLY
			</div>
		</div>
	</div>
</div>
