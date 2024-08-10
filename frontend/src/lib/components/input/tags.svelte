<script lang="ts">
	//@ts-nocheck // TODO: Remove this at some point
	import Input from "./input.svelte";
	import { Icon, XMark } from "svelte-hero-icons";
	import type { HTMLBaseAttributes } from "svelte/elements";

	let tags: string[] = [];
	export let selectedTags: string[] = [];
	interface $$Props extends HTMLBaseAttributes {
		selectedTags?: string[];
	}

	function addTag(e: KeyboardEvent) {
		if (e.key !== "Enter" || e.currentTarget?.value === "") return;
		e.preventDefault();
		if (tags.includes(e.currentTarget?.value)) return;
		selectedTags = [...selectedTags, e.currentTarget?.value];
		e.currentTarget.value = "";
	}

	function removeTag(index: number) {
		selectedTags.splice(index, 1);
		selectedTags = selectedTags;
	}
</script>

<datalist id={`${$$props.id}-datalist`}>
	{#each tags as tag}
		<option value={tag}>{tag}</option>
	{/each}
</datalist>

<div class="tag-container" {...$$restProps}>
	<ul class="tags">
		{#each selectedTags as tag, index}
			<li class="tag">
				<span>{tag}</span>
				<button
					type="button"
					tabindex="-1"
					class="tag-remove"
					on:click={() => removeTag(index)}
				>
					<Icon src={XMark} size="20px" />
				</button>
			</li>
		{/each}
	</ul>

	<Input
		maxlength={32}
		autocomplete="false"
		type="text"
		name="tag"
		list={`${$$props.id}-datalist`}
		placeholder="Add tags to categorize your project"
		on:keydown={addTag}
	/>
</div>

<style>
	div.tag-container {
		padding: 8px;
		border-radius: var(--br);
		background: var(--shade-01);
		border: 1px solid var(--border-color);
	}

	ul.tags {
		gap: 8px;
		padding: 4px;
		padding-inline-start: 0;
		max-width: inherit;
		display: inline-block;

		&:empty {
			display: none;
		}
	}

	li.tag {
		display: inline-flex;
		gap: 4px;
		padding: 2px 6px;
		border-radius: var(--br);
		background: var(--primary);
		cursor: text;
		font-size: 0.85em !important;
		outline-offset: 2px;
		outline: 2px solid transparent;
		transition: outline 0.2s ease-in-out;
		margin: 6px 10px 2px 0;
		color: white;

		& span {
			max-width: 256px;
			text-overflow: ellipsis;
			text-wrap: nowrap;
			overflow: hidden;
			white-space: nowrap;
		}

		&:last-child {
			margin-right: 0;
		}

		&:hover {
			outline-color: var(--primary);
		}

		& button {
			display: flex;
			padding: 0;
			border: none;
			align-items: center;
			background: transparent;
			cursor: pointer;
		}
	}
</style>
