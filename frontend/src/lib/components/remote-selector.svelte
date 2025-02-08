<script lang="ts">
	import { BookMarked } from "lucide-svelte";
	import * as Tabs from "$lib/components/ui/tabs";
	import { Button } from "./ui/button";
	import Terminal from "lucide-svelte/icons/terminal";
  import * as Alert from "$lib/components/ui/alert/index.js";

	interface Props {
		remotes: BackendTypes["GitDO"][];
		selectedRemote?: BackendTypes["GitDO"];
	}

	const { remotes = [] }: Props = $props();
</script>

<Tabs.Root value="create">
	<Tabs.List class="grid w-full grid-cols-2">
		<Tabs.Trigger value="select">Select Repository</Tabs.Trigger>
		<Tabs.Trigger value="create">Create Repository</Tabs.Trigger>
	</Tabs.List>
	<Tabs.Content value="select">
		<ul class="grid w-full gap-2">
			{#each remotes as remote}
				<li>
					<Button
						variant="ghost"
						class="flex h-full w-full items-center justify-start gap-3 text-left"
					>
						<BookMarked class="text-muted-foreground" />
						<div class="flex flex-col">
							<span class="font-medium">{remote?.name}</span>
							<span class="text-muted-foreground text-sm">{remote?.url}</span>
						</div>
					</Button>
				</li>
			{/each}
		</ul>
	</Tabs.Content>
	<Tabs.Content value="create">
		<Alert.Root>
			<Terminal class="size-4" />
			<Alert.Title>Managed</Alert.Title>
			<Alert.Description>A repository will be made for you</Alert.Description>
		</Alert.Root>
	</Tabs.Content>
</Tabs.Root>
