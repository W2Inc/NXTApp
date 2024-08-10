<script lang="ts">
	import { T, forwardEventHandlers } from "@threlte/core";
	import type { Goal, Node } from "../builder";
	import { HTML, useCursor } from "@threlte/extras";
	import { selectedGoal } from "$lib/stores/node-store";
	import { spring } from "svelte/motion";
	import { MeshStandardMaterial } from "three";
	import { goalParams, highlighted } from "$lib/types";
	import { onDestroy } from "svelte";

	// Params

	export let node: Node<Goal>;

	// Variables

	let isSelected = false;
	let dispatchingComponent = forwardEventHandlers();
	const scale = spring(1, { stiffness: 0.1, damping: 0.5 });
	const { onPointerEnter, onPointerLeave, hovering } = useCursor();
	let material = new MeshStandardMaterial(goalParams[node.meta.state]);
	const origMaterial = material.clone();

	// Functions

	hovering.subscribe((hovering) => {
		if (isSelected) return;
		highlight(hovering);
	});

	selectedGoal.subscribe((goal) => {
		highlight((isSelected = goal === node));
	});

	function highlight(toggle: boolean) {
		material = toggle ? highlighted : origMaterial;
		scale.set(toggle ? 1.3 : 1);
	}

	onDestroy(() => material.dispose());
</script>

<T.Mesh
	{material}
	scale={$scale}
	name={node.meta.name}
	bind:this={$dispatchingComponent}
	on:pointerenter={onPointerEnter}
	on:pointerleave={onPointerLeave}
	position={[node.position.x, node.position.y, node.position.z]}
>
	<T.IcosahedronGeometry />

	<HTML pointerEvents="none" center transform sprite>
		<b>{node.meta.name}</b>
	</HTML>
</T.Mesh>

<style>
	b {
		color: white;
		border: solid 1px var(--border-color);

		position: absolute;
		left: 16px;
		top: 16px;

		font-size: 12px;
		pointer-events: none;
		border-radius: var(--br);
		background-color: hsla(0, 0%, 9%, 0.5);
		padding: 4px 8px;
		word-break: keep-all;
		white-space: nowrap;
	}
</style>
