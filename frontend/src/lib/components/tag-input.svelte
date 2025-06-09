<script lang="ts">
	import X from "lucide-svelte/icons/x";
	import Badge from "./ui/badge/badge.svelte";
	import Button from "./ui/button/button.svelte";
	import Input from "./ui/input/input.svelte";

	let items = $state.raw<string[]>(["1", "2", "3", "4", "5"]);
</script>

<div
	class="border-input bg-background flex min-h-10 flex-wrap items-center gap-2 rounded-md border p-2"
>
	{#each items as item, i}
		<Badge
			color="primary"
			class="shadow-text flex items-center justify-center gap-1 px-3 py-1 text-center"
		>
			<input type="hidden" name="tags" value={item} />
			<span>{item}</span>
			<button
				type="button"
				class="hover:bg-primary-foreground/20 ml-1 rounded-full transition-colors"
				onclick={() => (items = items.filter((_, j) => j !== i))}
				aria-label="Remove tag"
			>
				<X class="h-4 w-4" />
			</button>
		</Badge>
	{/each}
	<Input
		placeholder="Add tag..."
		maxlength={64}
		class="h-min w-auto min-w-32 border-none bg-transparent p-0 shadow-none focus-visible:ring-0"
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
