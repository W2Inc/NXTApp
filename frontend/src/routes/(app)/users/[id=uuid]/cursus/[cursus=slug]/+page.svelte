<script>
	import Taskcard from "$lib/components/cards/task-card.svelte";
	import * as Accordion from "$lib/components/ui/accordion";
	import * as Avatar from "$lib/components/ui/avatar";
	import Separator from "$lib/components/ui/separator/separator.svelte";
</script>

<div class="m-auto max-w-6xl px-4 py-2">
	<div class="grid grid-cols-1 gap-x-8 pt-4 md:grid-cols-[256px,1fr]">
		<div class="flex flex-col">
			<img src="https://github.com/w2wizard.png" alt="logo" class="rounded border max-h-64 object-cover" />
			<div class="rounded border p-4 mt-2">Bottom</div>
			<Separator class="my-2 md:hidden"/>
		</div>

		<Accordion.Root class="w-full rounded border p-4">
			<Accordion.Item value="item-1">
				<Accordion.Trigger>Cursus Goals</Accordion.Trigger>
				<Accordion.Content>
					<div class="flex gap-4 flex-wrap p-3 max-h-96 overflow-auto">
						<Taskcard href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/goals/1" type="goal" title="Cursus!" />
						<Taskcard href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/goals/1" type="goal" title="Cursus!" />
						<Taskcard href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/goals/1" type="goal" title="Cursus!" />
						<Taskcard href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/goals/1" type="goal" title="Cursus!" />
						<Taskcard href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/goals/1" type="goal" title="Cursus!" />
						<Taskcard href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/goals/1" type="goal" title="Cursus!" />
						<Taskcard href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/goals/1" type="goal" title="Cursus!" />
						<Taskcard href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/goals/1" type="goal" title="Cursus!" />
						<Taskcard href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/goals/1" type="goal" title="Cursus!" />
						<Taskcard href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/goals/1" type="goal" title="Cursus!" />
						<Taskcard href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/goals/1" type="goal" title="Cursus!" />
						<Taskcard href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/goals/1" type="goal" title="Cursus!" />
						<Taskcard href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/goals/1" type="goal" title="Cursus!" />
						<Taskcard href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/goals/1" type="goal" title="Cursus!" />
						<Taskcard href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/goals/1" type="goal" title="Cursus!" />
						<Taskcard href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/goals/1" type="goal" title="Cursus!" />
						<Taskcard href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/goals/1" type="goal" title="Cursus!" />
						<Taskcard href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/goals/1" type="goal" title="Cursus!" />
						<Taskcard href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/goals/1" type="goal" title="Cursus!" />
						<Taskcard href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/goals/1" type="goal" title="Cursus!" />
						<Taskcard href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/goals/1" type="goal" title="Cursus!" />
						<Taskcard href="/users/Lmwfa1HEQt6jhU0BbzW5zQ/goals/1" type="goal" title="Cursus!" />
					</div>
				</Accordion.Content>
			</Accordion.Item>

			<div class="overflow-auto pt-6">
				Lorem ipsum dolor sit amet consectetur adipisicing elit. Assumenda veritatis
				corporis, error laboriosam suscipit accusamus, magnam rem tempore quia laudantium
				ullam sequi sint maxime, nulla non officia? Placeat, consequatur autem. Lorem ipsum
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
				nulla non officia? Placeat, consequatur autem. Lorem ipsum dolor sit amet
				consectetur adipisicing elit. Assumenda veritatis corporis, error laboriosam
				suscipit accusamus, magnam rem tempore quia laudantium ullam sequi sint maxime,
				nulla non officia? Placeat, consequatur autem.
			</div>
		</Accordion.Root>

	</div>
</div>
