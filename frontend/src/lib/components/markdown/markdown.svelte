<script lang="ts">
	// Icons
	import Heading from "lucide-svelte/icons/heading";
	import Bold from "lucide-svelte/icons/bold";
	import Italic from "lucide-svelte/icons/italic";
	import Strikethrough from "lucide-svelte/icons/strikethrough";
	import Ident from "lucide-svelte/icons/indent-increase";
	import Code from "lucide-svelte/icons/code";
	import Link from "lucide-svelte/icons/link";
	import List from "lucide-svelte/icons/list";
	import ListOrdered from "lucide-svelte/icons/list-ordered";
	import ListChecks from "lucide-svelte/icons/list-checks";
	import Radical from "lucide-svelte/icons/radical";
	import Ellipsis from "lucide-svelte/icons/ellipsis";

	// Imports
	import type { Icon } from "lucide-svelte";
	import Markdown from "svelte-exmarkdown";
	import "katex/dist/katex.min.css";
	import { plugins } from "./context.svelte";

	// Components
	import * as Popover from "$lib/components/ui/popover";
	import IconMarkdown from "./icon-markdown.svelte";
	import Textarea from "../ui/textarea/textarea.svelte";

	// Types

	interface Props {
		value: string;
		placeholder?: string;
		variant?: "editor" | "viewer";
	}

	type Action = {
		icon: typeof Icon;
		tooltip: string;
		key: string;
		action: Function;
	};

	// State

	let textarea = $state<HTMLTextAreaElement>(null!);
	let { value = $bindable(), placeholder, variant = "editor" }: Props = $props();
	let mode = $state<"write" | "preview">(variant === "editor" ? "write" : "preview");

	function insertTemplate(template: string, cursorOffset: number) {
		const start = textarea.selectionStart;
		const end = textarea.selectionEnd;
		const selected = value.substring(start, end);
		const newText =
			value.substring(0, start) + template.replace("%c", selected) + value.substring(end);
		value = newText;

		// Set cursor position
		setTimeout(() => {
			textarea.focus();
			textarea.selectionStart = start + cursorOffset;
			textarea.selectionEnd = start + cursorOffset + selected.length;
		}, 0);
	}

	const shortcuts: Action[] = [
		{
			icon: Heading,
			tooltip: "Heading (Ctrl+H)",
			key: "h",
			action: () => insertTemplate("# %c", 2),
		},
		{
			icon: Bold,
			tooltip: "Bold (Ctrl+B)",
			key: "b",
			action: () => insertTemplate("**%c**", 2),
		},
		{
			icon: Italic,
			tooltip: "Italic (Ctrl+I)",
			key: "i",
			action: () => insertTemplate("*%c*", 1),
		},
		{
			icon: Strikethrough,
			tooltip: "Strikethrough (Ctrl+S)",
			key: "s",
			action: () => insertTemplate("~~%c~~", 2),
		},
		{
			icon: Ident,
			tooltip: "Quote (Ctrl+Q)",
			key: "q",
			action: () => insertTemplate("> %c", 2),
		},
		{
			icon: Code,
			tooltip: "Code (Ctrl+K)",
			key: "k",
			action: () => insertTemplate("`%c`", 1),
		},
		{
			icon: Link,
			tooltip: "Link (Ctrl+L)",
			key: "l",
			action: () => insertTemplate("[%c](url)", 1),
		},
		{
			icon: List,
			tooltip: "Bullet List (Ctrl+U)",
			key: "u",
			action: () => insertTemplate("- %c", 2),
		},
		{
			icon: ListOrdered,
			tooltip: "Numbered List (Ctrl+O)",
			key: "o",
			action: () => insertTemplate("1. %c", 3),
		},
		{
			icon: ListChecks,
			tooltip: "Task List (Ctrl+T)",
			key: "t",
			action: () => insertTemplate("- [ ] %c", 6),
		},
		{
			icon: Radical,
			tooltip: "Math (Ctrl+M)",
			key: "m",
			action: () => insertTemplate("$%c$", 1),
		},
	];

	function handleKeydown(event: KeyboardEvent) {
		if (event.ctrlKey) {
			const shortcut = shortcuts.find((v) => v.key === event.key);
			if (!shortcut) return;
			event.preventDefault();
			event.stopImmediatePropagation();
			shortcut.action();
			return;
		}

		if (event.key === "Enter") {
			event.preventDefault();
			const start = textarea.selectionStart;
			const end = textarea.selectionEnd;
			const currentLineStart = value.lastIndexOf("\n", start - 1) + 1;
			const currentLine = value.substring(currentLineStart, start);

			let prefix = "";
			const listPatterns = {
				checkbox: /^-\s*\[\s*[xX\s]?\s*\]/,
				unordered: /^[-*]\s/,
				ordered: /^\d+\.\s/
			};

			if (listPatterns.checkbox.test(currentLine)) {
				prefix = "- [ ] ";
			} else if (listPatterns.unordered.test(currentLine)) {
				prefix = "- ";
			} else if (listPatterns.ordered.test(currentLine)) {
				const match = currentLine.match(/^\d+/);
				if (match) {
					prefix = `${parseInt(match[0]) + 1}. `;
				}
			}

			// Clear list continuation if current line is empty
			if (!currentLine.trim()) {
				prefix = "";
			}

			// Update text and cursor position
			value = value.substring(0, start) + "\n" + prefix + value.substring(end);
			const newPosition = start + 1 + prefix.length;

			textarea.selectionStart = newPosition;
			textarea.selectionEnd = newPosition;
			textarea.focus();
		}
	}
</script>

<!-- Snippets -->

{#snippet shortcut({ icon, tooltip, action }: Action)}
	{@const RenderIcon = icon}
	<li>
		<!-- <Tippy text={tooltip} onclick={() => action()}> -->
		<button
			title={tooltip}
			type="button"
			onclick={() => action()}
			class="hover:bg-muted grid size-8 place-content-center rounded"
		>
			<RenderIcon size={16} class="m-auto" />
		</button>
		<!-- </Tippy> -->
	</li>
{/snippet}

{#snippet toggle(toggleMode: "write" | "preview")}
	<li class="group/{toggleMode} h-8">
		<button
			data-selected={mode === toggleMode}
			class="bg-background hover:bg-muted text-muted-foreground data-[selected=true]:bg-accent h-full border-r px-2 text-sm group-first/{toggleMode}:rounded-tl-sm"
			onclick={() => (mode = toggleMode)}
		>
			<span class="capitalize">{toggleMode}</span>
		</button>
	</li>
{/snippet}

<div class="mt-2 block">
	<!-- Menu bars -->
	{#if variant === "editor"}
		<div class="flex flex-wrap justify-between rounded-t border">
			<menu class="flex h-auto">
				{@render toggle("write")}
				{@render toggle("preview")}
			</menu>
			{#if mode === "write"}
				<menu class="flex items-center gap-[1px]">
					{#each shortcuts.slice(0, 5) as props}
						{@render shortcut(props)}
					{/each}
					{#if shortcuts.length > 5}
						<Popover.Root>
							<Popover.Trigger
								class="bg-background hover:bg-muted grid size-8 place-content-center rounded"
							>
								<Ellipsis size={16} class="m-auto" />
							</Popover.Trigger>
							<Popover.Content class="w-10 p-2 py-1">
								<menu class="flex flex-col items-center gap-1">
									{#each shortcuts.slice(5) as props}
										{@render shortcut(props)}
									{/each}
								</menu>
							</Popover.Content>
						</Popover.Root>
					{/if}
				</menu>
			{/if}
		</div>
	{/if}

	<!-- Content -->
	<div
		class="rounded-b border p-2 {variant === 'viewer'
			? 'rounded-t border-t'
			: 'border-t-0'}"
	>
		{#if variant === "editor"}
			<Textarea
				data-mode={mode}
				bind:ref={textarea}
				draggable="false"
				{placeholder}
				class="bg-background min-h-[100px] w-full overflow-y-auto rounded border p-4 outline-none data-[mode=preview]:hidden"
				bind:value
				name="markdown"
				required
				onkeydown={handleKeydown}
			></Textarea>
		{/if}
		<div
			data-mode={mode}
			class="markdown max-h-96 min-h-40 overflow-auto rounded-b data-[mode=write]:hidden"
		>
			<Markdown md={value} {plugins} />
		</div>
		{#if mode === "write"}
			<span class="text-muted-foreground flex items-center gap-1 text-xs">
				<IconMarkdown />
				Markdown is supported
			</span>
		{/if}
	</div>
</div>
