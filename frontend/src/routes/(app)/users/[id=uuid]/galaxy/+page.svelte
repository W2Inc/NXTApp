<script lang="ts">
	import { T, Canvas } from "@threlte/core";
	import { OrbitControls } from "@threlte/extras";
	import * as THREE from "three";
	import Scene from "./Scene.svelte";

</script>

<svelte:head>
	<title>Learning Goals Galaxy</title>
</svelte:head>

<div class="canvas">
	<Canvas>
		<Scene />
	</Canvas>
</div>

<style>
	.canvas {
		width: 100%;
		height: 100vh;
		display: block;
	}
</style>
