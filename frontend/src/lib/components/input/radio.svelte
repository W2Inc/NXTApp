<script lang="ts">
	export let name: string = "radio";
	export let value: string = "";
</script>

<div class="container">
	<input tabindex="0" class="radio-input" type="radio" {name} {value} />
	<div class="radio">
		<div class="radio-dot" />
	</div>
</div>

<style>
	.container {
		position: relative;
		width: 16px;
		height: 16px;

		&:focus-within {
			border-radius: 3px;
			outline-offset: 2px;
			outline: var(--outline);
		}
	}

	input {
		opacity: 0;
		position: absolute;
		z-index: 1;
		top: 0;
		left: 0;
		width: 100%;
		height: 100%;
		cursor: pointer;
	}

	.radio {
		position: relative;
		border-radius: 50%;
		width: 100%;
		height: 100%;
		box-shadow: inset 0 0 0 1px var(--border-color);

		& .radio-input:checked + & {
			border-color: transparent;
		}

		&::before {
			content: "";
			position: absolute;
			top: 0;
			left: 0;
			border-radius: inherit;
			width: 100%;
			height: 100%;
			background-color: var(--primary);
			background-size: 60px;
			opacity: 0;
			box-shadow: var(--box-shadow);
			transition-property: opacity;
			transition-duration: 0.2s;

			@at-root .radio-input:checked + & {
				opacity: 1;
			}
		}
	}

	.radio-dot {
		position: absolute;
		top: 50%;
		left: 50%;
		border-radius: inherit;
		width: 0.375em;
		height: 0.375em;
		transform: translate(-50%, -50%) scale(0);
		background-color: #fff;
		opacity: 0;
		filter: drop-shadow(0 1px 1px rgb(0 0 0 / 0.2));
		transition-property: transform, opacity;
		transition-duration: 0.2s;

		.radio-input:checked + .radio > & {
			transform: translate(-50%, -50%) scale(1);
			opacity: 1;
		}
	}
</style>
