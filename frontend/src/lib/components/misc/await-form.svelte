<script lang="ts">
	import toast from "sonner-svelte";
	import { applyAction, enhance } from "$app/forms";
	import type { ActionResult, SubmitFunction } from "@sveltejs/kit";
	import type { HTMLFormAttributes } from "svelte/elements";
	import { createEventDispatcher } from "svelte";

	// A form that "awaits" the result of the action
	// including a toast message

	export let isLoading = false;
	interface $$Props extends HTMLFormAttributes {
		isLoading?: boolean;
	}

	const dispatch = createEventDispatcher<{
		"loading": boolean,
		"result": ActionResult
	}>();

	export const onSubmit: SubmitFunction = ({ cancel }) => {
		dispatch("loading", (isLoading = true));
		toast.loading("Loading...", {
			action: {
				label: "Cancel",
				onClick: () => cancel(),
			},
		});

		return async ({ result }) => {
			dispatch("loading", (isLoading = false));
			dispatch("result", result);

			toast.dismiss();
			if (result.type === "success" && result.data) {
				toast.success(result.data.message);
			} else if (result.type === "failure" && result.data) {
				toast.error(result.data.message);
			}

			// NOTE: For errors, we just apply the action
			await applyAction(result);
		};
	};
</script>

<form enctype="multipart/form-data" {...$$restProps} use:enhance={onSubmit}>
	<slot />
</form>

<style>
	form {
		display: flex;
		flex-direction: column;
		gap: 8px;
	}
</style>
