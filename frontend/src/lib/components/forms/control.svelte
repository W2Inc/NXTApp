<script lang="ts">
	import type { Snippet } from "svelte";
	import { Label } from "$lib/components/ui/label";

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
	<p class="mb-2 text-muted-foreground text-sm">
		{description}
	</p>
	{@render children()}
	<p class="text-destructive mb-2 text-sm">
		{currentError}
	</p>
</div>
