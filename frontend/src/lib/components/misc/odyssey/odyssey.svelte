<script lang="ts">
	// ============================================================================
	// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
	// See README in the root project for more information.
	// ============================================================================
	// Modified by: Nextdemy B.V @ 20/11/2023
	// Source: https://github.com/threlte/threlte/tree/main/apps/docs/src/components/hackathon
	// ============================================================================

	import { NoToneMapping, type WebGLRendererParameters } from "three";
	import { Canvas } from "@threlte/core";
	import { dev } from "$app/environment";
	import Scene from "./scene.svelte";

	let debug = false;

	const rendererParameters: WebGLRendererParameters = {
		powerPreference: "high-performance",
		antialias: false,
		stencil: false,
		depth: false,
		premultipliedAlpha: false,
		failIfMajorPerformanceCaveat: true,
	};
</script>

<svelte:window
	on:keypress={(e) => {
		if (e.key === "d" && dev) {
			debug = !debug;
		}
	}}
/>

<div class="theatre">
	<div class="child">
		<Canvas toneMapping={NoToneMapping} {rendererParameters} >
			<Scene {debug} />
		</Canvas>
	</div>
</div>

<style>
	div.theatre {
		position: absolute;
		top: 0;
		left: 0;
		z-index: -1;
		height: 100vh;
		width: 100vw;
		opacity: 0.95;

		& div.child {
			position: absolute;
			inset: 0;
		}

		@media (prefers-reduced-motion: reduce) {
			display: none;
		}
	}
</style>
