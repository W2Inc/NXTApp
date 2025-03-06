<script lang="ts">
	import type { Snippet } from "svelte";

	let elem: HTMLDivElement;
	let tiltAngleX = $state(0);
	let tiltAngleY = $state(0);
	const { children }: { children: Snippet } = $props();

	function handleMouseMove(event: MouseEvent) {
		const rect = elem.getBoundingClientRect();
		const deltaX = event.clientX - (rect.left + rect.width / 2);
		const deltaY = event.clientY - (rect.top + rect.height / 2);

		// Calculate tilt with more natural movement
		tiltAngleX = Math.max(-15, Math.min(15, deltaY / 20));
		tiltAngleY = Math.max(-15, Math.min(15, -deltaX / 20));
	}
</script>

<svelte:window onmousemove={handleMouseMove} />

<div
	bind:this={elem}
	style="transform: rotateX({tiltAngleX}deg) rotateY({tiltAngleY}deg)"
>
	{@render children()}
</div>
