<script lang="ts">
	import { MarkdownEditor } from "carta-md";
	import { cartaContext } from "./context.svelte";

	interface Props {
		value?: string;
		placeholder?: string;
	}

	let { value = $bindable(""), placeholder }: Props = $props();
</script>

<MarkdownEditor
	carta={cartaContext}
	{placeholder}
	bind:value
	mode="tabs"
	theme="github"
/>
