<script lang="ts">
	import type { Length } from "./types.js";
	import { constrain } from "./utils.js";
	import { createEventDispatcher } from "svelte";

	const dispatch = createEventDispatcher<{
		change: undefined;
	}>();

	// Props
	export let id: string | undefined = undefined;
	export let type: "horizontal" | "vertical" = "horizontal";
	export let pos: Length = "50%";
	export let min: Length = "0%";
	export let max: Length = "100%";
	export let disabled = false;
	export let priority: "min" | "max" = "min";

	// State
	let w = 0;
	let h = 0;
	let dragging = false;
	let container: HTMLElement;

	$: position = pos;

	// constrain position
	$: if (container) {
		const size = type === "horizontal" ? w : h;
		position = constrain(container, size, min, max, position, priority);
	}

	function update(x: number, y: number) {
		if (disabled) return;

		const { top, left } = container.getBoundingClientRect();
		const pos_px = type === "horizontal" ? x - left : y - top;
		const size = type === "horizontal" ? w : h;

		position = pos.endsWith("%") ? `${(100 * pos_px) / size}%` : `${pos_px}px`;
		dispatch("change");
	}

	function drag(node: HTMLElement, callback: (event: PointerEvent) => void) {
		const pointerdown = (event: PointerEvent) => {
			if (
				(event.pointerType === "mouse" && event.button === 2) ||
				(event.pointerType !== "mouse" && !event.isPrimary)
			)
				return;

			node.setPointerCapture(event.pointerId);

			event.preventDefault();

			dragging = true;

			const onpointerup = () => {
				dragging = false;

				node.setPointerCapture(event.pointerId);

				window.removeEventListener("pointermove", callback, false);
				window.removeEventListener("pointerup", onpointerup, false);
			};

			window.addEventListener("pointermove", callback, false);
			window.addEventListener("pointerup", onpointerup, false);
		};

		node.addEventListener("pointerdown", pointerdown, { capture: true, passive: false });

		return {
			destroy() {
				node.removeEventListener("pointerdown", pointerdown);
			},
		};
	}
</script>

<div
	data-pane={id}
	class="container {type}"
	style="--pos: {position}"
	bind:this={container}
	bind:clientWidth={w}
	bind:clientHeight={h}
>
	<div class="pane">
		<slot name="a" />
	</div>

	<!-- NOTE: Is this bad? -->
	<div class="pane">
		<slot name="b" />
	</div>

	{#if pos !== "0%" && pos !== "100%"}
		<div
			class="{type} divider"
			class:disabled
			use:drag={(e) => update(e.clientX, e.clientY)}
		/>
	{/if}
</div>

{#if dragging}
	<div class="mousecatcher" />
{/if}

<style>
	.container {
		--sp-thickness: var(--thickness, 8px);
		--sp-color: var(--color, transparent);
		display: grid;
		position: relative;
		width: 100%;
		height: 100%;
	}

	.container.vertical {
		grid-template-rows: var(--pos) 1fr;
	}

	.container.horizontal {
		grid-template-columns: var(--pos) 1fr;
	}

	.pane {
		width: 100%;
		height: 100%;
		overflow: auto;
	}

	.pane > :global(*) {
		width: 100%;
		height: 100%;
		overflow: hidden;
	}

	.mousecatcher {
		position: absolute;
		left: 0;
		top: 0;
		width: 100%;
		height: 100%;
		background: rgba(255, 255, 255, 0.0001);
	}

	.divider {
		position: absolute;
		z-index: 10;
		touch-action: none !important;
		margin-right: calc(-0.5 * var(--sp-thickness));
	}

	.divider::after {
		content: "";
		position: absolute;
		border-left: calc(var(--sp-thickness) * 0.5) solid var(--border-color, transparent);
		/*border-top: calc(var(--sp-thickness) * 0.5) solid var(--border-color, transparent);*/
	}

	.horizontal > .divider {
		padding: 0 calc(0.5 * var(--sp-thickness));
		width: 0;
		height: 100%;
		cursor: ew-resize;
		left: var(--pos);
		transform: translate(calc(-0.5 * var(--sp-thickness)), 0);
	}

	.horizontal > .divider.disabled {
		cursor: default;
	}

	.horizontal > .divider::after {
		left: 50%;
		top: 0;
		width: 1px;
		height: 100%;
	}

	.vertical > .divider {
		padding: calc(0.5 * var(--sp-thickness)) 0;
		width: 100%;
		height: 0;
		cursor: ns-resize;
		top: var(--pos);
		transform: translate(0, calc(-0.5 * var(--sp-thickness)));
	}

	.vertical > .divider.disabled {
		cursor: default;
	}

	.vertical > .divider::after {
		top: 50%;
		left: 0;
		width: 100%;
		height: 1px;
	}
</style>
