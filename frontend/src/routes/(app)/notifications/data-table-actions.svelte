<script lang="ts">
	import {Button} from "$lib/components/ui/button/index.js";
	import Check from "lucide-svelte/icons/check";
	import X from "lucide-svelte/icons/x";
	import Inbox from "lucide-svelte/icons/inbox";
	import {Eye} from "lucide-svelte";
	import Separator from "$lib/components/ui/separator/separator.svelte";

	let { id, type, resource }: { id: GUID; type: number, resource: GUID | null } = $props();
</script>

{#snippet invite()}
	<Button
		variant="outline"
		size="sm"
		class="h-7 border-green-600 text-green-600 shadow-inner hover:bg-green-600 hover:text-white"
	>
		<Check class="size-4" />
		Accept
	</Button>
	<Button
		variant="outline"
		size="sm"
		class="h-7 border-red-600 text-red-600 hover:bg-red-600 hover:text-white ml-1 shadow-inner"
	>
		<X class="size-4" />
		Decline
	</Button>
{/snippet}

{#snippet actionButtons()}
	{#if type & (1 << 4)}
		<Button size="sm" href="projects/{resource}" variant="outline" class="h-7 px-3 text-xs">
			<Eye class="mr-1 h-3 w-3" />
			View Project
		</Button>
		<Separator orientation="vertical" />
	{/if}
	{#if type & (1 << 0)}
		{@render invite()}
	{/if}
{/snippet}

{#snippet dismiss()}
	<Button size="sm" variant="outline" class="shadow-inner">
		<Inbox class="size-4" />
		Mark as Read
	</Button>
{/snippet}

<div class="flex justify-end pr-4 gap-1 flex-wrap">
	{resource}
	{@render actionButtons()}
</div>
