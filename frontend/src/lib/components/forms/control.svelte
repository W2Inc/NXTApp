<script lang="ts">
	import type { Snippet } from "svelte";
	import { Label } from "$lib/components/ui/label";
	import FileWarning from "lucide-svelte/icons/triangle-alert";

	interface Props {
		label: string;
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
		{label}
	</Label>
	{#if description}
		<p class="text-muted-foreground mb-2 text-xs">
			{description}
		</p>
	{/if}
	{@render children()}
	{#if currentError}
		<p class="text-destructive center-content mt-1 motion-safe:animate-pulse text-sm">
			<FileWarning size={16} />
			{currentError}
		</p>
	{/if}
</div>
