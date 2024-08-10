<script lang="ts">
	import { goto, invalidate, invalidateAll, pushState } from "$app/navigation";
	import { Icon, MagnifyingGlass } from "svelte-hero-icons";
	import type { HTMLBaseAttributes } from "svelte/elements";

	type $$Props = HTMLBaseAttributes;

	let value = "";
	let searchKey = "s";
	let input: HTMLInputElement;

	function captureFocus(e: KeyboardEvent) {
		if (
			e.key === searchKey &&
			!e.ctrlKey &&
			!e.shiftKey &&
			!e.altKey &&
			!e.metaKey &&
			document.activeElement === document.body
		) {
			e.preventDefault();
			input.focus();
		}

		if (e.key === "Escape") {
			input.blur();
			value = "";
		}

		if (e.key === "Enter" && value.length > 0) {
			document.querySelectorAll("dialog").forEach((dialog) => dialog.close());
			goto(`/search?q=${value}`, { invalidateAll: true });
			//invalidate("search:results");
		}
	}
</script>

<svelte:window on:keydown={captureFocus} />

<search class="center" {...$$restProps}>
	<Icon src={MagnifyingGlass} size="16px" />
	<div class="placeholder__container">
		<input bind:this={input} bind:value title="Search the world..." name="search" autocomplete="off" />
		<span class:hidden={value.length > 0} class="placeholder">
			Press <kbd>{searchKey}</kbd> to search
		</span>
	</div>
</search>

<style>
	.hidden {
		opacity: 0 !important;
	}

	input {
		flex: 1;
		border: none;
		outline: none;
		background-color: var(--shade-01);
		width: 100%;
		height: 20px;
		color: var(--foreground-color);
	}

	div.placeholder__container {
		flex: 1;
		position: relative;

		& .placeholder {
			position: absolute;
			display: flex;
			align-items: center;
			gap: 4px;
			top: 1px;
			left: 6px;
			pointer-events: none;
			user-select: none;
			color: var(--text-4);
			opacity: 1;
			height: 100%;
			font-size: 0.8rem;
			transition: opacity 0.1s ease-in-out;

			&.hidden {
				opacity: 0;
			}
		}
	}

	search.center {
		border-radius: var(--br);
		border: 1px solid var(--border-color);
		padding: 4px 12px;
		min-width: 12rem;
		width: 15rem;
		flex: 1 1 auto;
		box-shadow: var(--box-shadow);
		background: var(--shade-01);

		&:focus-within {
			outline-offset: var(--outline-offset);
			outline: var(--outline);
		}

		@media only screen and (max-width: 768px) {
			& kbd {
				display: none;
			}
		}
	}
</style>
