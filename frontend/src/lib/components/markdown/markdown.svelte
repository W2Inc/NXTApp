<script lang="ts">
	import { MarkdownEditor, Markdown } from "carta-md";
	import { cartaContext } from "./context.svelte";

	interface Props {
		value?: string;
		placeholder?: string;
		variant: "editor" | "viewer";
	}

	let { value = $bindable(""), placeholder, variant = "editor", ...rest }: Props = $props();
</script>

{#if variant === "editor"}
	<MarkdownEditor
		carta={cartaContext}
		{placeholder}
		bind:value
		mode="tabs"
		theme="github"
		textarea={{ name: "markdown", require: true, ...rest }}
	/>
{:else}
	<Markdown carta={cartaContext} bind:value theme="github" />
{/if}
