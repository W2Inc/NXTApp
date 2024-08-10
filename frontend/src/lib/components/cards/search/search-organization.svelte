<script lang="ts">
	import type { components } from "$lib/api/types";

	// HACK: No longer needed in Svelte5 (which is still in alpha)
	export let result: components["schemas"]["SearchResult"];
	let data = result as components["schemas"]["Organization"];
</script>

{JSON.stringify(data)}
