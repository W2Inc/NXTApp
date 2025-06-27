<script lang="ts">
	import X from "lucide-svelte/icons/x";
	import { Button } from "../ui/button";
	import Separator from "../ui/separator/separator.svelte";

	let parent: HTMLDivElement;

	const { data }: { data: BackendTypes["SpotlightEventDO"] } = $props();
</script>

<div bind:this={parent} class="text-white">
	<div
		class="flex flex-col gap-1 rounded bg-cover bg-center p-4"
		style="background-image: url('{data.backgroundUrl}');"
	>
		<input type="hidden" name="id" value={data.id} />

		<h1 class="center-content justify-between text-2xl">
			{data.title}
			<Button
				class="shadow-none size-6"
				variant="ghost"
				size="icon"
				onclick={() => parent.remove()}
				type="submit"
			>
				<X size={18} />
			</Button>
		</h1>
		<Separator class="bg-white" />
		<p>{data.description}</p>
		<Button class="w-full" href={data.href}>
			{data.actionText}
		</Button>
	</div>
</div>
