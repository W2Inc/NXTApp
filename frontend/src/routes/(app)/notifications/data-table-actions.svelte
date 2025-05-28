<script lang="ts">
	import Ellipsis from "lucide-svelte/icons/ellipsis";
	import { Button } from "$lib/components/ui/button/index.js";
	import * as DropdownMenu from "$lib/components/ui/dropdown-menu/index.js";
	import Check from "lucide-svelte/icons/check";
	import X from "lucide-svelte/icons/x";
	import Inbox from "lucide-svelte/icons/inbox";
	import Tippy from "$lib/components/tippy.svelte";
	import { Eye } from "lucide-svelte";

	let { id, type }: { id: string; type: number } = $props();
</script>

{#snippet invite()}
	<Button
		variant="outline"
		size="sm"
		class="border-green-700 text-green-700 shadow-inner hover:bg-green-700 hover:text-white"
	>
		<Check class="size-4" />
		Accept
	</Button>
	<Button
		variant="outline"
		size="sm"
		class="border-destructive text-destructive hover:bg-destructive hover:text-destructive-foreground ml-2 shadow-inner"
	>
		<X class="size-4" />
		Decline
	</Button>
{/snippet}

{#snippet actionButtons()}
	{#if type & (1 << 0)}
		<Button
			size="sm"
			class="h-7 border-0 bg-green-600 px-3 text-xs text-white hover:bg-green-700"
		>
			<Check class="mr-1 h-3 w-3" />
			Accept
		</Button>
		<Button
			size="sm"
			variant="outline"
			class="h-7 border-red-200 px-3 text-xs text-red-600 hover:bg-red-50"
		>
			<X class="mr-1 h-3 w-3" />
			Decline
		</Button>
	{:else if type & (1 << 4)}
		<Button size="sm" variant="outline" class="h-7 px-3 text-xs">
			<Eye class="mr-1 h-3 w-3" />
			View Details
		</Button>
	{/if}
{/snippet}

{#snippet dismiss()}
	<Button size="sm" variant="outline" class="shadow-inner">
		<Inbox class="size-4" />
		Mark as Read
	</Button>
{/snippet}

<div class="pr-4 text-right">
	{@render actionButtons()}
</div>
