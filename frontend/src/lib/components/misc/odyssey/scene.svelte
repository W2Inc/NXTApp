<script lang="ts">
	// ============================================================================
	// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
	// See README in the root project for more information.
	// ============================================================================
	// Modified by: Nextdemy B.V @ 20/11/2023
	// Source: https://github.com/threlte/threlte/tree/main/apps/docs/src/components/hackathon
	// ============================================================================

	import { T, useFrame, useThrelte } from "@threlte/core";
	import { Align, OrbitControls, useTexture } from "@threlte/extras";
	import { onMount, tick } from "svelte";
	import { DEG2RAD } from "three/src/math/MathUtils.js";
	import PostProcessing from "./post-processing.svelte";
	import { tweened } from "svelte/motion";
	import { cubicIn } from "svelte/easing";
	import warp0 from "$assets/textures/odessey0.jpg";
	import warp1 from "$assets/textures/odessey1.jpg";
	import {
		MeshBasicMaterial,
		MirroredRepeatWrapping,
		PlaneGeometry,
		Texture,
	} from "three";

	export let debug: boolean;


	let opacity = tweened(0, {
		duration: 750,
		easing: cubicIn,
	});

	const warpTexture = useTexture(warp1);
	$: if ($warpTexture) {
		$warpTexture.colorSpace = "srgb";
		$warpTexture.wrapS = MirroredRepeatWrapping;
		$warpTexture.wrapT = MirroredRepeatWrapping;
	}

	let clonedTexture: Texture | undefined;
	onMount(async () => {
		clonedTexture = (await warpTexture).clone();
		clonedTexture.wrapS = MirroredRepeatWrapping;
		clonedTexture.wrapT = MirroredRepeatWrapping;
		clonedTexture.colorSpace = "srgb";

		await tick();
		opacity.set(1);
	});
	const getWarpTexture = () => $warpTexture;
	const getClonedTexture = () => clonedTexture;

	useFrame((_, delta) => {
		const speed = 0.1;
		const wt = getWarpTexture();
		if (wt) wt.offset.x += speed * delta;
		const ct = getClonedTexture();
		if (ct) ct.offset.x += speed * delta;
	});

	const heightIterations = 5;
	const height = 40;
	const offset = 0.2;
	const gap = 4.55;
	const rotationInDegrees = 85;

	const geometry = new PlaneGeometry(100, height);
	if (geometry.attributes.uv) {
		geometry.attributes.uv.array[2] = 0.125;
		geometry.attributes.uv.array[6] = 0.125;
		geometry.attributes.uv.needsUpdate = true;
	}
</script>

<PostProcessing />
<T.PerspectiveCamera
	makeDefault={!debug}
	position={[0, 0, 50]}
	fov={90}
	on:create={({ ref }) => {
		ref.lookAt(0, 0, 0);
	}}
/>

<T.PerspectiveCamera makeDefault={debug} position={[8, 25, 100]}>
	<OrbitControls
		enableZoom
		enablePan={true}
		dampingFactor={0.025}
	/>
</T.PerspectiveCamera>

{#if $warpTexture && clonedTexture}
	<Align x={false} z={false}>
		{@const leftMaterial = new MeshBasicMaterial({
			map: clonedTexture,
			transparent: true,
		})}
		{@const rightMaterial = new MeshBasicMaterial({
			map: $warpTexture,
			transparent: true,
		})}

		{#each Array(heightIterations) as _, i}
			{@const isOdd = i % 2 === 0}
			<T.Mesh
				position={[-gap + offset, height * i, 0]}
				rotation.y={rotationInDegrees * DEG2RAD}
				scale.y={isOdd ? -1 : 1}
			>
				<T is={geometry} />
				<T is={leftMaterial} opacity={$opacity} />
			</T.Mesh>
		{/each}

		{#each Array(heightIterations) as _, i}
			{@const isOdd = i % 2 === 0}
			<T.Mesh
				position={[gap - offset, height * i, 0]}
				rotation.y={-rotationInDegrees * DEG2RAD}
				scale.y={isOdd ? -1 : 1}
				scale.x={-1}
			>
				<T is={geometry} />
				<T is={rightMaterial} opacity={$opacity} />
			</T.Mesh>
		{/each}
	</Align>
{/if}
