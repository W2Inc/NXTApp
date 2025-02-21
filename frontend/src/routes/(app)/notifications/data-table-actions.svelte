<script lang="ts">
	import Ellipsis from "lucide-svelte/icons/ellipsis";
	import { Button } from "$lib/components/ui/button/index.js";
	import * as DropdownMenu from "$lib/components/ui/dropdown-menu/index.js";
	import Check from "lucide-svelte/icons/check";
	import X from "lucide-svelte/icons/x";
	import Inbox from "lucide-svelte/icons/inbox";
	import Tippy from "$lib/components/tippy.svelte";

	let { id, type }: { id: string; type: "notification" | "invite" } = $props();
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

{#snippet dismiss()}
	<Button size="sm" variant="outline" class="shadow-inner">
		<Inbox class="size-4" />
		Mark as Read
	</Button>
{/snippet}

<div class="pr-4 text-right">
	{#if type === "notification"}
		{@render dismiss()}
	{:else if type === "invite"}
		{@render invite()}
	{/if}
</div>
