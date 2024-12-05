<script lang="ts">
	import * as AlertDialog from "$lib/components/ui/alert-dialog";
	import { buttonVariants } from "../ui/button";
	import { dialog } from "./state.svelte";

	function resolveIt(value?: unknown) {
		switch (dialog.current?.type) {
			case "confirm": {
				dialog.current?.resolve(Boolean(value));
				break;
			}
			case "alert": {
				dialog.current?.resolve();
				break;
			}
			default:
				break;
		}

		dialog.current = null;
	}
</script>

<AlertDialog.Root open={dialog.current !== null}>
	<AlertDialog.Content class="p-4">
		<AlertDialog.Header>
			<AlertDialog.Title>
				{dialog.current?.title}
			</AlertDialog.Title>
			<AlertDialog.Description>
				{dialog.current?.message}
			</AlertDialog.Description>
		</AlertDialog.Header>
		<AlertDialog.Footer>
			{@const isConfirm = dialog.current?.type === "confirm"}
			{#if isConfirm}
				<AlertDialog.Cancel onclick={() => resolveIt(false)}>
					Cancel
				</AlertDialog.Cancel>
			{/if}
			<AlertDialog.Action
				class={buttonVariants({ variant: "default" })}
				onclick={() => (isConfirm ? resolveIt(true) : resolveIt())}
			>
				{#if isConfirm}
					Confirm
				{:else}
					Ok
				{/if}
			</AlertDialog.Action>
		</AlertDialog.Footer>
	</AlertDialog.Content>
</AlertDialog.Root>
