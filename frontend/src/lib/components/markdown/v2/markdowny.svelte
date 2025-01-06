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

	// Imports
	import type { Icon } from "lucide-svelte";
	import Tippy from "$lib/components/tippy.svelte";
	import Markdown from "svelte-exmarkdown";
	import "katex/dist/katex.min.css";
	import { plugins } from "./context.svelte";

	type Action = {
		icon: typeof Icon;
		tooltip: string;
		key: string;
		action: Function;
	};

	function insertTemplate(template: string, cursorOffset: number) {
		const start = textarea.selectionStart;
		const end = textarea.selectionEnd;
		const selected = md.substring(start, end);
		const newText =
			md.substring(0, start) + template.replace("%c", selected) + md.substring(end);
		md = newText;

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
			const start = textarea.selectionStart;
			const currentLine = md.slice(0, start).split("\n").pop() || "";

			// Check for list patterns
			const bulletMatch = currentLine.match(/^(\s*)-\s+(.*)$/);
			const checkboxMatch = currentLine.match(/^(\s*)-\s\[\s?\]\s+(.*)$/);
			const numberMatch = currentLine.match(/^(\s*)\d+\.\s+(.*)$/);

			if (bulletMatch || checkboxMatch || numberMatch) {
				event.preventDefault();
				const content = bulletMatch?.[2] || checkboxMatch?.[2] || numberMatch?.[2] || "";
				const indent = bulletMatch?.[1] || checkboxMatch?.[1] || numberMatch?.[1] || "";

				// If the current line is empty, remove the list marker
				if (!content) {
					const lineStart = md.slice(0, start).lastIndexOf("\n") + 1;
					md = md.slice(0, lineStart) + md.slice(start);
					textarea.selectionStart = textarea.selectionEnd = lineStart;
					return;
				}

				// Add new list item
				const newLine = "\n" + indent;
				if (checkboxMatch) {
					event.preventDefault();
					insertTemplate(newLine + "- [ ] ", start + newLine.length + 6);
				} else if (bulletMatch) {
					event.preventDefault();
					insertTemplate(newLine + "- ", start + newLine.length + 2);
				} else if (numberMatch) {
					const nextNumber = parseInt(currentLine) + 1;
					event.preventDefault();
					insertTemplate(
						newLine + nextNumber + ". ",
						start + newLine.length + nextNumber.toString().length + 2,
					);
				}
			}
		}
	}

	let textarea = $state<HTMLTextAreaElement>(null!);
	let mode = $state<"write" | "preview">("write");
	let md = $state("```typescript\nconsole.log('Hello, world!');\n```");
</script>

<!-- Snippets -->

{#snippet shortcut({ icon, tooltip, action }: Action)}
	{@const RenderIcon = icon}
	<li>
		<Tippy text={tooltip} onclick={() => action()}>
			<button
				type="button"
				inert
				class="bg-background hover:bg-muted grid size-8 place-content-center rounded"
			>
				<RenderIcon size={16} class="m-auto" />
			</button>
		</Tippy>
	</li>
{/snippet}

{#snippet toggle(toggleMode: "write" | "preview")}
	<li class="group/{toggleMode}">
		<button
			data-selected={mode === toggleMode}
			class="bg-background hover:bg-muted text-muted-foreground data-[selected=true]:bg-accent h-full border-r px-2 text-sm group-first/{toggleMode}:rounded-tl-sm"
			onclick={() => (mode = toggleMode)}
		>
			<span class="capitalize">{toggleMode}</span>
		</button>
	</li>
{/snippet}

<div class="mt-2 w-[500px]">
	<!-- Menu bars -->
	<div class="flex flex-wrap justify-between rounded-t border">
		<menu class="flex h-auto">
			{@render toggle("write")}
			{@render toggle("preview")}
		</menu>
		<menu class="flex gap-[1px]">
			{#each shortcuts as props}
				{@render shortcut(props)}
			{/each}
		</menu>
	</div>
	<!-- Content -->
	<div class="rounded-b border border-t-0 p-2">
		<textarea
				data-mode={mode}
				bind:this={textarea}
				draggable="false"
				class="bg-background rounded p-4 border w-full data-[mode=preview]:hidden outline-none min-h-[100px] overflow-y-auto"
				bind:value={md}
				onkeydown={handleKeydown}
		>
		</textarea>
		<div
			data-mode={mode}
			class="markdown rounded-b data-[mode=write]:hidden"
		>
			<Markdown {md} {plugins} />
		</div>
	</div>
</div>
