<script lang="ts">
  import { T } from '@threlte/core'
  import { Float, useGltf, useSuspense } from '@threlte/extras'
	import { Vector3 } from 'three';

	export let position: Vector3 = new Vector3(0, 0, 0);

  const suspend = useSuspense();
  const gltf = suspend(useGltf("https://static.oolp.dev/mesh/ship.gltf"))
</script>

{#await gltf then { scene }}
  <T.Group {...$$restProps}>
    <Float
      rotationIntensity={0.5}
      speed={3}
    >
      <T is={scene} scale={1} />
    </Float>
  </T.Group>
{/await}
