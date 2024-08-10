<script lang="ts">
	import "katex/dist/katex.min.css";
	import "highlight.js/styles/gradient-dark.min.css";
	import plugins from "./plugins";
	import Markdown, { getAstNode, type Parser } from "svelte-exmarkdown";
	import { onMount } from "svelte";
	import { createParser } from "svelte-exmarkdown/utils";
	import { getComponentsMap } from "svelte-exmarkdown/contexts";

	let engine: Markdown;
	export let md: string = "";
</script>

<!--<pre>{JSON.stringify($componentsContextValue)}</pre>-->
<div class="markdown-render-body">
	<Markdown bind:this={engine} {md} {plugins} />
</div>

<style>
</style>
