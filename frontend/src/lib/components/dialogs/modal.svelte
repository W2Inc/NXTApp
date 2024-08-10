<script lang="ts">
	import { createEventDispatcher } from "svelte";
	import { Icon, XMark } from "svelte-hero-icons";
	import type { HTMLFormAttributes } from "svelte/elements";

	let modal: HTMLDialogElement;
	type $$Props = HTMLFormAttributes;
	const dispatch = createEventDispatcher<{
		close: void;
	}>();

	/** Show the modal */
	export function show() {
		modal.showModal();
	}

	/** Close the modal */
	export function close() {
		modal.close();
		dispatch("close");
	}

	/** Toggle the modal */
	export function toggle() {
		modal.open ? close() : show();
	}
</script>

<dialog bind:this={modal}>
	<form {...$$restProps}>
		<div>
			<h1>{$$props.title}</h1>
			<!-- NOTE: Using formmethod="dialog" is fucked here -->
			<!-- Reset here actually helps us yeet all the data -->
			<button title="Close this modal" class="center" type="reset" on:click={close}>
				<Icon src={XMark} solid size="24" />
			</button>
		</div>
		<hr />
		<slot />
	</form>
</dialog>

<style>
	div {
		display: flex;
		justify-content: space-between;
		align-items: center;
		gap: 2rem;

		& h1 {
			padding: 0;
			font-size: 1.5rem;
			font-weight: 400;
			color: var(--foreground-color);
			text-align: center;
		}

		& button {
			background: transparent;
			border: none;
			cursor: pointer;
			color: var(--foreground-color);
			padding: 4px;
			border-radius: var(--br);

			&:hover {
				background-color: var(--shade-03);
			}
		}
	}

	dialog {
		top: 50%;
		left: 50%;
		transform: translate(-50%, -50%);
		opacity: 1;
		color: var(--foreground-color);
		background: var(--shade-01);
		border: 1px solid var(--border-color);
		box-shadow: var(--box-shadow);
		border-radius: var(--br);
		padding: 15px;
		/*width: 50%;*/

		&[open] {
			animation: slidein 0.35s ease-in-out;
		}

		&::backdrop {
			background: rgba(0, 0, 0, 0.35);
			animation: fadein 0.35s ease-in-out;
		}
	}

	@keyframes fadein {
		from {
			background: rgba(0, 0, 0, 0);
		}
		to {
			background: rgba(0, 0, 0, 0.35);
		}
	}

	@keyframes slidein {
		from {
			transform: translate(-50%, -200%);
			opacity: 0;
		}
		to {
			transform: translate(-50%, -50%);
			opacity: 1;
		}
	}
</style>
