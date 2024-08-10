<script lang="ts">
	import type { HTMLInputAttributes, HTMLInputTypeAttribute } from "svelte/elements";
	import Password from "./actions/password.svelte";
	import Url from "./actions/url.svelte";

	export let label: string | undefined = undefined;
	interface $$Props extends HTMLInputAttributes {
		label?: string;
	}

	let inputElement: HTMLInputElement;
	const inputType: HTMLInputTypeAttribute = $$props["type"];

	function togglePassword(isHidden: boolean) {
		inputElement.type = isHidden ? "password" : "text";
	}
	function copyToClipboard() {
		navigator.clipboard.writeText(inputElement.value);
	}
</script>

<!-- HTML -->

<div class="container">
	{#if label}
		<label for={$$props.id} title={$$props.title}>
			{$$props.required ? `${label} *` : label}
		</label>
	{/if}

	<span class="parent">
		<input bind:this={inputElement} on:input on:keydown on:change {...$$restProps} />
		{#if inputType == "url"}
			<Url on:click={() => copyToClipboard()} />
		{:else if inputType == "password"}
			<Password on:toggle={(e) => togglePassword(e.detail.hidden)} />
		{/if}
	</span>
</div>

<!-- Styling -->

<style>

	.container {
		display: grid;
	}

	.parent {
		display: flex;
		color: var(--foreground-color);
		/* height: 100%; */

		& input {
			color: var(--foreground-color);
			appearance: none;
			border: none;
			background: none;
			outline: none;
			padding: var(--padding);
			width: 100%;
			border-radius: var(--br);

			&::-webkit-slider-thumb {
				display: none;
			}

			/* WTF */
			&::-webkit-calendar-picker-indicator {
				color: var(--foreground-color);
			}

			&::file-selector-button {
				appearance: none;
				padding: 8px;
				background-color: var(--shade-01);
				cursor: pointer;
				border-radius: var(--br);
				transition: background-color var(--transition);
				box-shadow: var(--box-shadow);
				border: 1px solid var(--border-color);
				color: var(--foreground-color);

				&:hover {
					background-color: var(--shade-02);
				}
			}

			&[type="password"]::-ms-reveal {
				display: none;
			}

			&[type="search"] {
				/* clears the ‘X’ from Internet Explorer */
				&::-ms-clear {
					display: none;
					height: 0;
					width: 0;
				}

				&::-ms-reveal {
					display: none;
					height: 0;
					width: 0;
				}
				/* clears the ‘X’ from Chrome */
				&::-webkit-search-decoration,
				&::-webkit-search-cancel-button,
				&::-webkit-search-results-button,
				&::-webkit-search-results-decoration {
					display: none;
				}
			}

			&[type="color"] {
				padding: 4px;
			}

			&[type="range"] {
				box-shadow: none;
			}

			&:disabled {
				color: var(--text-3);
				cursor: not-allowed;
			}
		}

		/* height: inherit; */

		border-radius: var(--br);
		background-color: var(--shade-01);
		border: 1px solid var(--border-color);
		box-shadow: var(--box-shadow);

		width: 100%;

		&:has(input:disabled) {
			background-color: var(--shade-02);
		}

		& > input[type="range"] {
			display: none;

			/* Show the custom slider thumb */
			&::-moz-range-thumb {
				display: block;
				width: 1em;
				height: 1em;
				border-radius: 50%;
				background-color: var(--primary);
				border: 1px solid var(--border-color);
				box-shadow: var(--box-shadow);
				cursor: pointer;
			}
		}
	}
</style>
