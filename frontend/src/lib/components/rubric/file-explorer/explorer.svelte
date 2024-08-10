<script lang="ts">
	import { createEventDispatcher } from "svelte";
	import Folder from "./folder.svelte";
	import { ExplorerContext } from "./context";
	import { files, selectedName } from "./state";
	import { afterNavigate } from "$app/navigation";
	import { createDirs, resetFiles } from "./utils";
	import type { Stub } from "$lib/types";
	import { clientWritable } from "$lib/utils";
	import Card from "$lib/components/cards/card.svelte";
	import Button from "$lib/components/buttons/button.svelte";
	import { DocumentArrowUp, Icon } from "svelte-hero-icons";

	const dispatch = createEventDispatcher();
	const hidden = new Set([".git"]);
	const collapsed = clientWritable<Record<string, boolean>>({});
	afterNavigate(() => collapsed.set({}));

	type ULEvent = KeyboardEvent & {
		currentTarget: EventTarget & HTMLUListElement;
	};
	function onSelect(e: ULEvent) {
		if (e.key === "ArrowUp" || e.key === "ArrowDown") {
			e.preventDefault();
			const list = Array.from(e.currentTarget.querySelectorAll("li"));
			const focused = list.findIndex((li) => {
				//@ts-ignore // TODO: fix this
				return li.contains(e.target);
			});

			const d = e.key === "ArrowUp" ? -1 : +1;
			list[focused + d]?.querySelector("button")?.focus();
		}
	}

	ExplorerContext.set({
		collapsed,

		add: async (name: string, type: "file" | "directory") => {
			const basename = name.split("/").pop() as string;

			const file: Stub =
				type === "file"
					? { type, name, basename, text: true, contents: "" }
					: { type, name, basename };

			resetFiles([...$files, ...createDirs(name, $files), file]);

			if (type === "file") {
				dispatch("select", { name });
			}
		},

		select: (name) => {
			dispatch("select", { name });
		},
	});

	// TODO: Demo data
	files.set([
		{
			type: "file",
			name: "/README.md",
			basename: "README.md",
			text: true,
			contents: `GARBAGE 0`,
		},
		{
			type: "file",
			name: "/package.json",
			basename: "package.json",
			text: true,
			contents: `GARBAGE 1`,
		},
		{
			type: "file",
			name: "/picture.png",
			basename: "picture.png",
			text: true,
			// Base64 image content
			contents: `iVBORw0KGgoAAAANSUhEUgAAAAoAAAAKCAYAAACNMs+9AAAAFUlEQVR42mP8/5+hnoEIwDiqkL4KAcT9GO0U4BxoAAAAAElFTkSuQmCC`
		},
		{
			type: "directory",
			name: "/balls",
			basename: "balls",
		},
		{
			type: "file",
			name: "/balls/README2.md",
			basename: "README2.md",
			text: false,
			contents: `GARBAGE 2`,
		},
	]);
</script>

<ul role="tree" class="filetree" on:keydown={onSelect}>
	<Folder
		prefix={"/"}
		depth={0}
		directory={{
			type: "directory",
			name: "",
			basename: "/",
		}}
		contents={$files.filter((file) => !hidden.has(file.basename))}
	/>
</ul>

<style>
	.filetree {
		flex: 1;
		overflow-y: auto;
		overflow-x: hidden;
		padding: 8px 1rem;
		margin: 0;
		background: var(--shade-02);
		list-style: none;
	}

	.filetree:not(.mobile)::before {
		content: "";
		position: absolute;
		width: 0;
		height: 100%;
		top: 0;
		right: 0;
		border-right: 1px solid var(--border-color);
	}
</style>
