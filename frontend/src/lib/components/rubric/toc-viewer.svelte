<script lang="ts">
	import { ArrowLeft, ArrowRight, ArrowUpTray, Check, DocumentMagnifyingGlass, Icon } from "svelte-hero-icons";
	import Button from "../buttons/button.svelte";
	import Dropdown from "../input/dropdown.svelte";
	import { createEventDispatcher } from "svelte";

	export let selected = 0;
	export let parts: string[] = [];

	const dispatch = createEventDispatcher<{
		finish: void;
		selection: {
			selected: number;
			previous: number;
		};
	}>();

	function next() {
		if (selected === parts.length) return;
		selected = Math.min(parts.length, selected + 1);
		dispatch("selection", {
			selected,
			previous: selected - 1,
		});
	}

	function prev() {
		if (selected === 0) return;
		selected = Math.max(0, selected - 1);
		dispatch("selection", {
			selected,
			previous: selected + 1,
		});
	}

	function set(part: number) {
		const previous = selected;
		selected = Math.max(0, Math.min(parts.length, part));
		dispatch("selection", {
			selected,
			previous,
		});
	}
</script>

<div class="center rubric-toc">
	<Button disabled={selected === 0} on:click={prev}>
		<Icon src={ArrowLeft} size="16" />
	</Button>
	<div class="selection">
		<Dropdown>
			<strong slot="text">Part {selected + 1}: {parts[selected]}</strong>
			<menu class="dropdown-body">
				{#each parts as part, i}
					<li>
						<button class:selected={i === selected} on:click={() => set(i)}>
							Part {i + 1}: {part}
						</button>
					</li>
				{/each}
				<!-- E.g: "Finish" button -->
				<slot name="extra" />
			</menu>
		</Dropdown>
	</div>
	<Button disabled={selected === parts.length - 1} on:click={next}>
		<Icon src={ArrowRight} size="16" />
	</Button>
</div>

<style>
	div.rubric-toc {
		& div.selection {
			flex: 1;
			height: 100%;
			background-color: var(--shade-02);
			border-radius: var(--br);
			border: 1px solid var(--border-color);
		}

		& [slot="text"] {
			padding: 4px;
			flex: 1;
		}

		& menu.dropdown-body {
			background: var(--shade-02);
			border: 1px solid var(--border-color);
			border-radius: var(--br);
			font-size: 0.9rem;
			list-style: none;
			padding: 8px;
			display: flex;
			gap: 8px;
			flex-direction: column;
		}
	}

	li {
		border-radius: var(--br);
		cursor: pointer;

		&:hover {
			color: var(--primary);
			background: var(--shade-03);
		}
	}

	button {
		font-family: "Monaspace";
		appearance: none;
		background: none;
		border: none;
		text-align: left;
		vertical-align: baseline;
		font-size: 1rem;
		cursor: inherit;
		padding: 4px;
		color: var(--foreground-color);
		width: 100%;
		border-radius: var(--br);

		&.selected {
			color: var(--primary);
		}
	}
</style>
