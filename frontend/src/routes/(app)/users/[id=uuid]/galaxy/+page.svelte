<script lang="ts">
	// import { Canvas } from "@threlte/core";
	import { T, Canvas  } from '@threlte/core'
</script>

<svelte:head>
	<title>Users Galaxy</title>
</svelte:head>

{#snippet scene()}
	<T.PerspectiveCamera
		position={[10, 10, 10]}
		on:create={({ ref }) => {
			ref.lookAt(0, 0, 0);
		}}
	/>

	<T.Mesh>
		<T.BoxGeometry args={[1, 1, 1]} />
		<T.MeshBasicMaterial color="red" />
	</T.Mesh>
{/snippet}

<Canvas>
  {@render scene()}
</Canvas>
