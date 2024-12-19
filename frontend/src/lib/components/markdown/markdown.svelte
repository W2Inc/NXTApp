<script lang="ts">
	import { MarkdownEditor, Markdown } from "carta-md";
	import { cartaContext } from "./context.svelte";

	interface Props {
		value?: string;
		placeholder?: string;
		variant: "editor" | "viewer";
	}

	let { value = $bindable(""), placeholder, variant = "editor" }: Props = $props();
</script>

{#if variant === "editor"}
	<MarkdownEditor
		carta={cartaContext}
		{placeholder}
		bind:value
		mode="tabs"
		theme="github"
	/>
{:else}
	<Markdown carta={cartaContext} bind:value theme="github" />
{/if}
