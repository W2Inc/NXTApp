<script lang="ts">
	import type { Snippet } from "svelte";
	import { Label } from "$lib/components/ui/label";
	import FileWarning from "lucide-svelte/icons/triangle-alert";

	interface Props {
		label?: string;
		name: string;
		description?: string;
		errors?: string[];
		children: Snippet;
	}

	const { label, name, errors, description, children }: Props = $props();
	const currentError = $derived.by(() => {
		if (!errors) return null;
		return errors.at(0) ?? null;
	});
</script>

<div class="flex-1">
	<Label class="cursor-pointer" for={name}>
		{#if label}
			{label}
		{/if}
	</Label>
	{#if description}
		<p class="text-muted-foreground mb-2 text-xs">
			{description}
		</p>
	{/if}
	{@render children()}
	{#if currentError}
		<p class="text-destructive flex gap-1 mt-1 text-sm motion-safe:animate-pulse">
			<FileWarning size={16} class="min-w-4" />
			{currentError}
		</p>
	{/if}
</div>
