<script>
	import { T, useTask } from "@threlte/core";
	import { MeshLineGeometry, MeshLineMaterial } from "@threlte/extras";
	import { Color, Vector3 } from "three";

	export let width = 0.085;
	export let dotted = false;
	export let source = new Vector3(0, 0, 0);
	export let target = new Vector3(10, 10, 10);
	export let color = "white"

	let dashOffset = 0.8;
	if (dotted) {
		useTask((delta) => {
			dashOffset -= delta / 32;
		});
	}
</script>

<T.Mesh>
	<MeshLineGeometry points={[source, target]} />
	{#if dotted}
		<MeshLineMaterial
			{width}
			{color}
			dashArray={0.09}
			dashRatio={0.6}
			{dashOffset}
			opacity={0.25}
			transparent
			depthTest={true}
		/>
	{:else}
		<MeshLineMaterial {width} {color} opacity={0.25} transparent depthTest={true} />
	{/if}
</T.Mesh>
