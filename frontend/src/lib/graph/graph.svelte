<script lang="ts">
	import { selectedGoal } from "$lib/stores/node-store";
	import { HTML, interactivity } from "@threlte/extras";
	import type { ClientGraph } from "./builder";
	import { T, useThrelte } from "@threlte/core";
	import Line from "./objects/line.svelte";
	import NodeNew from "./objects/node-new.svelte";
	import NodeGoal from "./objects/node-goal.svelte";

	export let graphData: ClientGraph;
	const { camera, size } = useThrelte();

	interactivity({
		// Return the first object that is hit by the raycaster only
		filter: (hits, state) => hits.slice(0, 1),
		compute: (event, state) => {
			state.pointer.update((pointer) => {
				return pointer.set(
					(event.offsetX / $size.width) * 2 - 1,
					-(event.offsetY / $size.height) * 2 + 1
				);
			});
			state.raycaster.setFromCamera(state.pointer.current, camera.current);

			if (event.type !== "click") return;
			const objects = state.raycaster.intersectObjects(state.interactiveObjects);
			if (objects.length == 0) selectedGoal.set(null);
		},
	});
</script>

	{#each graphData.nodes as node}
		{#if node.type === "action"}
			<NodeNew {node} on:click={() => console.log("Subscribe me please")}/>
		{:else if node.type === "goal"}
			{@const hackNode = node} <!-- HACK: Since Svelte4 does not suppport TS (fixed in 5)-->
			<NodeGoal {node} on:click={() => selectedGoal.set(hackNode)} />
		{:else}
			<HTML>
				<b>Oops! The Junior pushed to production :D</b>
			</HTML>
		{/if}
	{/each}

<T.Group>
	{#each graphData.links as link}
		{@const dotted = link.state === "unavailable"}
		<Line {dotted} source={link.source.position} target={link.target.position} />
	{/each}
</T.Group>
