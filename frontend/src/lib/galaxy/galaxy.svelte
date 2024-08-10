<script lang="ts">
	import * as THREE from "three";
	import { onDestroy } from "svelte";
	import { galaxy, star } from "./config.json";
	import { useThrelte } from "@threlte/core";
	import Bloom from "./effects/bloom.svelte";
	import { gaussianRandom, spiral } from "./utils";
	import vertexShader from "./shaders/galaxy.vertex.glsl?raw";
	import fragmentShader from "./shaders/galaxy.fragment.glsl?raw";

	//===< Functions >===//

	function hexToRgbArray(hex: string) {
		// Remove the '0x' prefix if present
		hex = hex.replace(/^0x/, "");

		// Parse the hex value to an integer
		const intValue = parseInt(hex, 16);

		// Extract individual RGB components
		const red = (intValue >> 16) & 255;
		const green = (intValue >> 8) & 255;
		const blue = intValue & 255;

		// Normalize the RGB components to values between 0 and 1
		const normalizedRed = red / 255;
		const normalizedGreen = green / 255;
		const normalizedBlue = blue / 255;

		// Return the normalized RGB components as an array
		return [normalizedRed, normalizedGreen, normalizedBlue];
	}

	/**
	 * Calculate the point size multiplier based on the camera's field of view
	 * @param camera The camera to calculate the multiplier for
	 * @param basePointSize The base point size
	 */
	function calculatePointMultiplier(
		camera: THREE.PerspectiveCamera,
		basePointSize: number
	) {
		// Example calculation: adjust point size based on camera's field of view
		// 'basePointSize' is a constant that represents the base size of your points
		const vFOV = camera.fov; // convert vertical fov to radians
		const height = 2 * Math.tan(vFOV / 2) * camera.position.z; // visible height
		// console.log(height);
		// console.log("Fov: " + vFOV);
		const pointMultiplier = 1.0; //basePointSize / height;
		// console.log("Point Multiplier: " + pointMultiplier);
		return pointMultiplier;
	}

	/** Calculate the coordinates of the stars. */
	function createCoords() {
		const positions: THREE.Vector3[] = [];

		for (let i = 0; i < galaxy.NUM_STARS; i++) {
			positions.push(
				new THREE.Vector3(
					gaussianRandom(0, galaxy.CORE_X_DIST),
					gaussianRandom(0, galaxy.THICKNESS),
					gaussianRandom(0, galaxy.CORE_Y_DIST)
				)
			);
		}

		for (let j = 0; j < galaxy.NUM_ARMS; j++) {
			for (let i = 0; i < galaxy.NUM_STARS / 4; i++) {
				positions.push(
					spiral(
						gaussianRandom(galaxy.ARM_X_MEAN, galaxy.ARM_X_DIST),
						gaussianRandom(galaxy.ARM_Y_MEAN, galaxy.ARM_Y_DIST),
						gaussianRandom(0, galaxy.THICKNESS),
						(j * 2 * Math.PI) / galaxy.ARMS
					)
				);
			}
		}

		return positions;
	}

	/**
	 * Create a galaxy
	 * @param scene
	 * @param camera
	 */
	function createGalaxy(scene: THREE.Scene, camera: THREE.PerspectiveCamera) {
		const coords = createCoords();
		const geo = new THREE.BufferGeometry();

		const shaderMaterial = new THREE.ShaderMaterial({
			uniforms: {
				opacity: { value: 1.0 },
				pointMultiplier: { value: calculatePointMultiplier(camera, 1.0) },
			},
			vertexShader: vertexShader,
			fragmentShader: fragmentShader,
			transparent: true,
		});

		geo.setAttribute(
			"size",
			new THREE.Float32BufferAttribute(
				coords.flatMap((v) => star.size[Math.floor(Math.random() * star.size.length)]!),
				1
			)
		);

		geo.setAttribute(
			"position",
			new THREE.Float32BufferAttribute(
				coords.flatMap((v) => [v.x, v.y, v.z]),
				3
			)
		);

		geo.setAttribute(
			"color",
			new THREE.Float32BufferAttribute(
				// coords.flatMap((v) => [v.x, v.y, v.z]), // DEBUG PATTERN

				coords.flatMap((v) => {
					const color = star.color[Math.floor(Math.random() * star.color.length)]!;
					return hexToRgbArray(color);
				}),
				3
			)
		);

		const points = new THREE.Points(geo, shaderMaterial);
		scene.add(points);
		return { points, geo };
	}

	//===< Galaxy >===//

	const { scene, camera, renderer } = useThrelte();
	if (!renderer.capabilities.isWebGL2) {
		alert("Your browser does not support WebGL2");
		throw new Error("Your browser does not support WebGL2");
	}

	const { geo, points } = createGalaxy(scene, $camera as THREE.PerspectiveCamera);

	onDestroy(() => {
		scene.remove(points);
		geo.dispose();
	});
</script>
