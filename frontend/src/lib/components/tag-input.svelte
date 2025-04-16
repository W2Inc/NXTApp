<script lang="ts">
	import X from "lucide-svelte/icons/x";
	import Badge from "./ui/badge/badge.svelte";
	import Button from "./ui/button/button.svelte";
	import Input from "./ui/input/input.svelte";

	let items = $state.raw<string[]>(["1", "2", "3", "4", "5"]);
</script>

<div class="flex flex-wrap items-center gap-2 p-2 rounded-md border border-input bg-background min-h-10">
	{#each items as item, i}
		<Badge color="primary" class="shadow-text px-3 py-1 flex gap-1 text-center justify-center items-center">
			<input type="hidden" name="tags" value={item} />
			<span>{item}</span>
			<button
				type="button"
				class="ml-1 rounded-full hover:bg-primary-foreground/20 transition-colors"
				onclick={() => items = items.filter((_, j) => j !== i)}
				aria-label="Remove tag"
			>
			<X class="w-4 h-4" />
			</button>
		</Badge>
	{/each}
	<Input
		placeholder="Add tag..."
		maxlength={64}
		class="border-none bg-transparent shadow-none h-min focus-visible:ring-0 p-0 w-auto min-w-[8rem]"
		onkeydown={(e) => {
			if (e.key === "Enter") {
				e.preventDefault();
				const value = e.currentTarget.value.trim();
				if (value && !items.includes(value)) {
					items = [...items, value];
					e.currentTarget.value = "";
				}
			} else if (e.key === "Backspace" && !e.currentTarget.value) {
				items = items.slice(0, -1);
			}
		}}
	/>
</div>
