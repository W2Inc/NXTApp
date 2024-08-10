<script lang="ts">
	import type { HTMLInputAttributes } from "svelte/elements";
	type $$Props = HTMLInputAttributes;
</script>

<div class="container">
	<input class="checkbox-input" type="checkbox" {...$$restProps}>
	<div class="checkbox">
		<svg
			class="checkbox-icon"
			viewBox="0 0 448 512"
			xmlns="http://www.w3.org/2000/svg"
		>
			<path
				d="M438.6 105.4c12.5 12.5 12.5 32.8 0 45.3l-256 256c-12.5 12.5-32.8 12.5-45.3 0l-128-128c-12.5-12.5-12.5-32.8 0-45.3s32.8-12.5 45.3 0L160 338.7 393.4 105.4c12.5-12.5 32.8-12.5 45.3 0z"
			/>
		</svg>
	</div>
</div>

<style>
	.container {
		position: relative;
		width: 1em;
		height: 1em;

		&:focus-within {
			border-radius: 3px;
			outline-offset: 2px;
			outline: var(--outline);
		}
	}

	.checkbox-input {
		position: absolute;
		opacity: 0;
		z-index: 1;
		top: 0;
		left: 0;
		width: 100%;
		height: 100%;
		cursor: pointer;
	}

	.checkbox {
		position: relative;
		border-radius: 4px;
		width: 100%;
		height: 100%;
		box-shadow: inset 0 0 0 1px var(--border-color);

		&::before {
			content: "";
			position: absolute;
			top: 0;
			left: 0;
			border-radius: inherit;
			width: 100%;
			height: 100%;
			background-color: var(--primary);
			opacity: 0;
			box-shadow: var(--box-shadow);
			transition-property: opacity;
			transition-duration: 0.2s;

			@at-root .checkbox-input:checked + & {
				opacity: 1;
			}
		}
	}

	.checkbox-icon {
		position: absolute;
		top: 50%;
		left: 50%;
		width: 0.75em;
		height: 0.75em;
		transform: translate(-50%, -50%) scale(0);
		fill: #fff;
		opacity: 0;
		filter: drop-shadow(0 1px 1px rgb(0 0 0 / 0.2));
		transition-property: transform, opacity;
		transition-duration: 0.2s;

		.checkbox-input:checked + .checkbox > & {
			transform: translate(-50%, -50%) scale(1);
			opacity: 1;
		}
	}
</style>
