<script>
	import Taskcard from "$lib/components/cards/task-card.svelte";
	import * as Accordion from "$lib/components/ui/accordion";
	import * as Avatar from "$lib/components/ui/avatar";
	import Separator from "$lib/components/ui/separator/separator.svelte";
	import Base from "$lib/components/base.svelte";
	import Button from "$lib/components/ui/button/button.svelte";
	import Trophy from "lucide-svelte/icons/trophy";
	import ShieldCheck from "lucide-svelte/icons/shield-check";
</script>

<Base variant="center-navbar">
	{#snippet left()}
		<div class="flex h-min flex-col gap-2 rounded border p-4">
			<img
				src="https://github.com/w2wizard.png"
				alt="logo"
				class="max-h-64 rounded border object-cover"
			/>
			<Button class="w-full">
				Register
				<ShieldCheck />
			</Button>
		</div>
		<Separator class="my-2 md:hidden" />
	{/snippet}

	{#snippet right()}
		<div class="rounded border p-4">
			<h1 class="center-content text-3xl">
				<Trophy />
				Cursus: C
			</h1>

			<Accordion.Root>
				<Accordion.Item value="item-1">
					<Accordion.Trigger>Goals</Accordion.Trigger>
					<Accordion.Content>
						<div class="flex max-h-96 flex-wrap gap-4 overflow-auto p-3">
							<Taskcard
								href="./goals/1"
								type="goal"
								title="Goal!"
							/>
							<Taskcard
								href="./goals/1"
								type="goal"
								title="Goal!"
							/>
							<Taskcard
								href="./goals/1"
								type="goal"
								title="Goal!"
							/>
						</div>
					</Accordion.Content>
				</Accordion.Item>
			</Accordion.Root>

			<div class="mt-4 overflow-auto">
				Markdown here
			</div>
		</div>
	{/snippet}
</Base>
