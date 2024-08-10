<script lang="ts">
	import { createEventDispatcher } from "svelte";

	let input: HTMLInputElement;
	export let value: number = 0;
	export let step: number = 1;
	export let min: number = 0;
	export let max: number = 100;

	const dispatch = createEventDispatcher();

	let toggle = true;
	function togglePassword() {
		toggle = !toggle;
		dispatch("toggle", {
			hidden: toggle,
		});
	}

	function updateSlider() {
		const minValue = +input.min || 0;
		const maxValue = +input.max || 100;
		value = (100 * +input.value) / (maxValue - minValue);
		dispatch("input", { value: +input.value });
	}
</script>

<div
	class="slider-container"
	style="--slider-value: {value}"
>
	<div class="slider-track">
		<input
			bind:this={input}
			on:input={() => updateSlider()}
			{step}
			{min}
			{max}
			class="slider-input"
			type="range"
			{value}
		/>
	</div>
	<div class="slider-thumb"></div>
</div>

<!-- TODO: Support markers -->

<style>
	.slider-container {
		--slider-value: 0;
		--slider-height: 0.375em;
		--slider-border: var(--border-color);

		display: flex;
		align-items: center;
		position: relative;
		border-radius: 3.125em;
		box-shadow: inset 0 0 0 1px var(--slider-border);

		&:focus-within:not(:active) {
			border-radius: var(--br);
			outline-offset: var(--outline-offset);
			outline: var(--outline);
		}
	}

	.slider-track {
		display: flex;
		align-items: center;
		position: relative;
		border-radius: inherit;
		height: var(--slider-height);
		width: calc(100%);
		background-color: var(--primary);
		mask-image: linear-gradient(
			to right,
			white 0 calc(var(--slider-value) * 1%),
			transparent calc(var(--slider-value) * 1%) 100%
		);
		box-shadow: var(--box-shadow);
	}

	.slider-input {
		-moz-appearance: none;
		-webkit-appearance: none;
		appearance: none;
		position: absolute;
		top: 0;
		left: 50%;
		transform: translate(-50%);
		border-radius: inherit;
		width: calc(100% + 0.875em);
		height: 100%;
		opacity: 0;
		cursor: pointer;

		@mixin thumb {
			appearance: none;
			border-radius: 50%;
			padding: 0.5em;
			width: 1.15em;
			height: 1.15em;
		}

		&::-webkit-slider-thumb {
			@include thumb;
		}

		&::-moz-range-thumb {
			@include thumb;
		}
	}

	.slider-thumb {
		display: flex;
		justify-content: center;
		align-items: center;
		position: absolute;
		left: calc(
			var(--slider-value) / 100 * (100% - var(--slider-height)) +
				var(--slider-height) / 2
		);
		transform: translateX(-50%);
		border-radius: 50%;
		width: 1em;
		height: 1em;
		background-color: var(--primary);
		pointer-events: none;
		box-shadow: var(--box-shadow);

		&::before {
			content: "";
			border-radius: inherit;
			width: 0.375em;
			height: 0.375em;
			background-color: #fff;
			transition-property: transform, opacity;
			transition-duration: 0.2s;
		}
	}
</style>
