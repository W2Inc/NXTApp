<script lang="ts">
	import { ChevronDown, Icon } from "svelte-hero-icons";
	import { afterUpdate, onMount } from "svelte";
	import type { HTMLBaseAttributes } from "svelte/elements";

	/** Offset the dropdown menu, offset is calcuated on mount, but it might be desired to offset it a bit more. */
	export let offset: number = 0;
	/** Hide the dropdown marker */
	export let nomarker = false;
	/** Toggle the dropdown instead of closing it on mouseleave */
	export let toggle = false;
	/** Is the dropdown open */
	export let open = false;


	interface $$Props extends HTMLBaseAttributes {
		open?: boolean;
		offset?: number;
		nomarker?: boolean;
		toggle?: boolean;
	}

	function onLeave() {
		open = toggle ? open : false;
	}

	function onOpen() {
		open = toggle ? open : true;
	}

	function windowClick({ target }: MouseEvent) {
		const node = target as Node;
		if (open && target && !dropdown.contains(node)) {
			open = false;
		}
	}

	function calcOffset() {
		const bodyRect = body.getBoundingClientRect();
		const viewportRect = document.documentElement.getBoundingClientRect();

		const offsetVal = Math.max(
			0,
			bodyRect.right - viewportRect.right,
			viewportRect.left - bodyRect.left
		);
		if (offsetVal > 0) body.style.right = `${0 + offset}px`;
		else if (offsetVal < 0) body.style.left = `${0 + offset}px`;
	}

	let body: HTMLDivElement;
	let dropdown: HTMLDetailsElement;

	onMount(calcOffset);
	afterUpdate(calcOffset);

</script>

<!-- We lose focus on clicking anywhere but this -->
<svelte:window on:click={windowClick} />

<details
	bind:open={open}
	bind:this={dropdown}
	on:mouseenter={onOpen}
	on:mouseleave={onLeave}
	{...$$restProps}
>
	<summary>
		<slot name="summary">
			<div class="content">
				<!-- Allow for inserting just text slot. e.g: Appheader -->
				<slot name="text" />
				{#if !nomarker}
					<span class="rotate" class:down={open}>
						<Icon src={ChevronDown} size="16px" />
					</span>
				{/if}
			</div>
		</slot>
	</summary>

	<!-- The body of the dropdown -->
	<div class="body" bind:this={body}>
		<slot>No Content!</slot>
	</div>
</details>

<style>
	details {
		z-index: 9999;
		border-radius: var(--br);
	}

	details summary::-webkit-details-marker {
		display: none;
	}

	div.body {
		position: absolute;
		padding: 8px 4px;
	}

	summary {
		cursor: pointer;

		&::marker {
			content: "";
		}

		&:hover {
			user-select: none;
		}

		& div.content {
			display: flex;
			align-items: center;
			gap: 4px;
		}

		& .content span.rotate {
			display: flex;
			transform: rotate(-90deg);
			transform-origin: center;
			transition: transform 0.1s ease-in-out;

			&.down {
				transform: rotate(0deg);
			}
		}
	}
</style>
