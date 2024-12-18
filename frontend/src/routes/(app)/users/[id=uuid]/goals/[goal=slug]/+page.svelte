<script>
	import Base from "$lib/components/base.svelte";
	import Taskcard from "$lib/components/cards/task-card.svelte";
	import * as Accordion from "$lib/components/ui/accordion";
	import * as Avatar from "$lib/components/ui/avatar";
	import Button from "$lib/components/ui/button/button.svelte";
	import Separator from "$lib/components/ui/separator/separator.svelte";
	import Trophy from "lucide-svelte/icons/trophy";
	import ShieldCheck from "lucide-svelte/icons/shield-check";
</script>

<Base variant="center-navbar">
	{#snippet left()}
		<div class="flex flex-col rounded border p-4 h-min gap-2">
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
				Goal: Learn to code
			</h1>

			<Accordion.Root>
				<Accordion.Item value="item-1">
					<Accordion.Trigger>Goals</Accordion.Trigger>
					<Accordion.Content>
						<div class="flex max-h-96 flex-wrap gap-4 overflow-auto p-3">
							<Taskcard
								href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/projects/1"
								type="project"
								title="Project!"
							/>
							<Taskcard
								href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/projects/1"
								type="project"
								title="Project!"
							/>
							<Taskcard
								href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/projects/1"
								type="project"
								title="Project!"
							/>
						</div>
					</Accordion.Content>
				</Accordion.Item>
			</Accordion.Root>

			<div class="mt-4 overflow-auto">
				Lorem ipsum dolor sit amet consectetur adipisicing elit. Assumenda veritatis
				corporis, error laboriosam suscipit accusamus, magnam rem tempore quia laudantium
				ullam sequi sint maxime, nulla non officia? Placeat, consequatur autem. Lorem
				ipsum dolor sit amet consectetur adipisicing elit. Assumenda veritatis corporis,
				error laboriosam suscipit accusamus, magnam rem tempore quia laudantium ullam
				sequi sint maxime, nulla non officia? Placeat, consequatur autem. Lorem ipsum
				dolor sit amet consectetur adipisicing elit. Assumenda veritatis corporis, error
				laboriosam suscipit accusamus, magnam rem tempore quia laudantium ullam sequi sint
				maxime, nulla non officia? Placeat, consequatur autem. Lorem ipsum dolor sit amet
				consectetur adipisicing elit. Assumenda veritatis corporis, error laboriosam
				suscipit accusamus, magnam rem tempore quia laudantium ullam sequi sint maxime,
				nulla non officia? Placeat, consequatur autem. Lorem ipsum dolor sit amet
				consectetur adipisicing elit. Assumenda veritatis corporis, error laboriosam
				suscipit accusamus, magnam rem tempore quia laudantium ullam sequi sint maxime,
				nulla non officia? Placeat, consequatur autem. Lorem ipsum dolor sit amet
				consectetur adipisicing elit. Assumenda veritatis corporis, error laboriosam
				suscipit accusamus, magnam rem tempore quia laudantium ullam sequi sint maxime,
				nulla non officia? Placeat, consequatur autem. Lorem ipsum dolor sit amet
				consectetur adipisicing elit. Assumenda veritatis corporis, error laboriosam
				suscipit accusamus, magnam rem tempore quia laudantium ullam sequi sint maxime,
				nulla non officia? Placeat, consequatur autem.
			</div>
		</div>
	{/snippet}
</Base>
