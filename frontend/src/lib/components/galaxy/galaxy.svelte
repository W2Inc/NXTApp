<script lang="ts">
	import * as THREE from "three";
	import { galaxy, star } from "./config.json";
	import { useThrelte } from "@threlte/core";
	import vertexShader from "./shaders/galaxy.vertex.glsl?raw";
	import fragmentShader from "./shaders/galaxy.fragment.glsl?raw";

	const {
		NUM_STARS,
		NUM_ARMS,
		CORE_X_DIST,
		CORE_Y_DIST,
		THICKNESS,
		ARM_X_MEAN,
		ARM_X_DIST,
		ARM_Y_MEAN,
		ARM_Y_DIST,
	} = galaxy;

	function gaussianRandom(mean = 0, stdev = 1) {
		let u = 1 - Math.random();
		let v = Math.random();
		let z = Math.sqrt(-2.0 * Math.log(u)) * Math.cos(2.0 * Math.PI * v);

		return z * stdev + mean;
	}

	function spiral(x: number, y: number, z: number, offset: number) {
		let r = Math.sqrt(x ** 2 + y ** 2);
		let theta = offset;
		theta += x > 0 ? Math.atan(y / x) : Math.atan(y / x) + Math.PI;
		theta += (r / galaxy.ARM_X_DIST) * galaxy.SPIRAL;
		return new THREE.Vector3(r * Math.cos(theta), z, r * Math.sin(theta));
	}

	/**
	 * Convert hexadecimal color to RGB array with normalized values
	 */
	function hexToRgbArray(hex: string): [number, number, number] {
		hex = hex.replace(/^0x/, "");
		const intValue = parseInt(hex, 16);
		const red = (intValue >> 16) & 255;
		const green = (intValue >> 8) & 255;
		const blue = intValue & 255;
		return [red / 255, green / 255, blue / 255];
	}

	/**
	 * Calculate the point size multiplier based on the camera's field of view
	 */
	function calculatePointMultiplier(
		camera: THREE.PerspectiveCamera,
		basePointSize: number,
	): number {
		const vFOV = camera.fov * (Math.PI / 180);
		const height = 2 * Math.tan(vFOV / 2) * camera.position.z;
		return basePointSize / height || 1.0;
	}

	/**
	 * Calculate the coordinates of the stars
	 */
	function createCoords(): Float32Array {
		let index = 0;
		const totalStars = NUM_STARS + (NUM_STARS / 4) * NUM_ARMS;
		const positions = new Float32Array(totalStars * 3);

		// Core stars
		for (let i = 0; i < NUM_STARS; i++) {
			positions[index++] = gaussianRandom(0, CORE_X_DIST);
			positions[index++] = gaussianRandom(0, THICKNESS);
			positions[index++] = gaussianRandom(0, CORE_Y_DIST);
		}

		// Arm stars
		for (let j = 0; j < NUM_ARMS; j++) {
			const armAngle = (j * 2 * Math.PI) / NUM_ARMS;

			for (let i = 0; i < NUM_STARS / 4; i++) {
				const spiralPos = spiral(
					gaussianRandom(ARM_X_MEAN, ARM_X_DIST),
					gaussianRandom(ARM_Y_MEAN, ARM_Y_DIST),
					gaussianRandom(0, THICKNESS),
					armAngle,
				);

				positions[index++] = spiralPos.x;
				positions[index++] = spiralPos.y;
				positions[index++] = spiralPos.z;
			}
		}

		return positions;
	}

	/**
	 * Create a galaxy
	 */
	function createGalaxy(scene: THREE.Scene, camera: THREE.PerspectiveCamera) {
		const positions = createCoords();
		const geo = new THREE.BufferGeometry();
		const totalStars = positions.length / 3;

		// Create size and color arrays
		const sizes = new Float32Array(totalStars);
		const colors = new Float32Array(totalStars * 3);

		for (let i = 0; i < totalStars; i++) {
			sizes[i] = star.size[Math.floor(Math.random() * star.size.length)] || 1.0;

			const colorHex =
				star.color[Math.floor(Math.random() * star.color.length)] || "FFFFFF";
			const [r, g, b] = hexToRgbArray(colorHex);
			colors[i * 3] = r;
			colors[i * 3 + 1] = g;
			colors[i * 3 + 2] = b;
		}

		const shaderMaterial = new THREE.ShaderMaterial({
			uniforms: {
				opacity: { value: 1.0 },
				pointMultiplier: { value: calculatePointMultiplier(camera, 1.0) },
			},
			vertexShader,
			fragmentShader,
			transparent: true,
		});

		geo.setAttribute("position", new THREE.BufferAttribute(positions, 3));
		geo.setAttribute("size", new THREE.BufferAttribute(sizes, 1));
		geo.setAttribute("color", new THREE.BufferAttribute(colors, 3));

		const points = new THREE.Points(geo, shaderMaterial);
		scene.add(points);
		return { points, geo };
	}

	//===< Galaxy >===//
	const { scene, camera, renderer } = useThrelte();

	if (!renderer.capabilities.isWebGL2) {
		throw new Error("Your browser does not support WebGL2");
	}

	const { geo, points } = createGalaxy(scene, $camera as THREE.PerspectiveCamera);
	$effect(() => {
		return () => {
			if (scene && points) scene.remove(points);
			if (geo) geo.dispose();
		};
	});
</script>
