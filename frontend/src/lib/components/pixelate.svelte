<script lang="ts">
	import type { WithChild } from "bits-ui";
	import type { ClassValue, HTMLAttributes } from "svelte/elements";
	import { Tween } from "svelte/motion";

	interface Props extends WithChild {
		delay?: number;
		duration?: number;
	}

	const { delay = 0, duration = 1150, children, ...rest }: Props = $props();

	let tween = new Tween(20, {
		duration,
		interpolate: (a, b) => t => Math.round(a + (b - a) * t)
	});

	const pixel = $derived(`#pixelate-${Math.max(0, tween.current)}`);
	$effect(() => { tween.set(0) })
</script>

<div style="filter: url({pixel}); will-change: filter;" {...rest}>
	{@render children?.()}
</div>
