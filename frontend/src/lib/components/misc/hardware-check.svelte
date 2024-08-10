<script lang="ts">
	//@ts-ignore // NOTE: detect-gpu is broken, this one has no types :P
	import { getGPUTier } from 'detect-gpu-js';
	import toast from 'sonner-svelte';
	import { onMount } from 'svelte';

	// Checks if the user has a good GPU and falls back to a provided
	// fallback slot

	export let goodGPU = false;

	onMount(async () => {
		const gpuTier = await getGPUTier();
		goodGPU = gpuTier.tier >= 2;
		if (!goodGPU) {
			toast.warning("Seems like your GPU cannot render the graph. Please switch to a different device.", {
				position: "top-right"
			});
		}
	});
</script>

{#if goodGPU}
	<slot />
{:else}
	<slot name="fallback" />
{/if}
