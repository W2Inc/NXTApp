<script lang="ts">
	import Ambience from "$lib/components/galaxy/audio/ambience.svelte";
	import Bloom from "$lib/components/galaxy/effects/bloom.svelte";
	import Galaxy from "$lib/components/galaxy/galaxy.svelte";
	import { Canvas, T } from "@threlte/core";
	import {
		AudioListener,
		MeshLineGeometry,
		MeshLineMaterial,
		OrbitControls,
	} from "@threlte/extras";
	import { getContext } from "svelte";

	// Define types for goals and connections
	type Goal = {
		id: string;
		position: [number, number, number];
		name: string;
	};

	type ConnectionState = "unavailable" | "available" | "complete";
	type Connection = {
		from: string;
		to: string;
		state: ConnectionState;
	};

	// Sample data
	const goals = $state<Goal[]>([
		{ id: "1", position: [0, 0, 0], name: "Start" },
		{ id: "2", position: [10, 5, 3], name: "Goal 1" },
		{ id: "3", position: [15, -5, 8], name: "Goal 2" },
		{ id: "4", position: [20, 10, -5], name: "Goal 3" },
		{ id: "5", position: [5, -10, -10], name: "Goal 4" },
		{ id: "6", position: [-10, -5, 15], name: "Goal 5" },
	]);

	const connections = $state<Connection[]>([
		{ from: "1", to: "2", state: "complete" },
		{ from: "2", to: "3", state: "available" },
		{ from: "3", to: "4", state: "unavailable" },
		{ from: "1", to: "5", state: "available" },
		{ from: "5", to: "6", state: "unavailable" },
	]);

	// Colors for connection states
	const connectionColors = {
		unavailable: "#555555",
		available: "#ffffff",
		complete: "#22cc22",
	};

	// Function to get positions for a line
	function getConnectionPositions(connection: Connection) {
		const fromGoal = goals.find((g) => g.id === connection.from);
		const toGoal = goals.find((g) => g.id === connection.to);

		if (!fromGoal || !toGoal) return [];

		return [
			fromGoal.position[0],
			fromGoal.position[1],
			fromGoal.position[2],
			toGoal.position[0],
			toGoal.position[1],
			toGoal.position[2],
		];
	}

	// Generate background galaxy spiral points
	const count = 5000;
	const positions = new Float32Array(count * 3);

	for (let i = 0; i < count; i++) {
		// Spiral parameters
		const radius = 40 * Math.sqrt(Math.random());
		const angle = radius * 0.3 + Math.random() * 0.2;
		const height = (Math.random() - 0.5) * 3;

		// Create spiral effect
		const x = radius * Math.cos(angle);
		const y = height;
		const z = radius * Math.sin(angle);

		positions[i * 3] = x;
		positions[i * 3 + 1] = y;
		positions[i * 3 + 2] = z;
	}

	const ctx = getContext<GalaxyCTX>("audio");
	$effect(() => {
		return () => (ctx.audioPlaying = false);
	});
</script>

<Canvas>
	<T.PerspectiveCamera makeDefault position={ctx.cameraPosition} fov={25}>
		<OrbitControls autoRotate autoRotateSpeed={0.5} />
		<AudioListener />
	</T.PerspectiveCamera>

	<T.AmbientLight intensity={0.2} />
	<T.DirectionalLight position={[10, 10, 10]} intensity={0.8} />

	{#each goals as goal}
		<T.Mesh position={goal.position}>
			<T.SphereGeometry args={[0.8, 16, 16]} />
			<T.MeshStandardMaterial color="#4488ff" />
		</T.Mesh>
	{/each}

	<!-- Connections between goals -->
	{#each connections as connection}
		{@const positions = getConnectionPositions(connection)}
		{@const color = connectionColors[connection.state]}
		{@const from = { x: positions[0], y: positions[1], z: positions[2] }}
		{@const to = { x: positions[3], y: positions[4], z: positions[5] }}
		{@const points = [from, to]}

		<T.Mesh>
			<MeshLineGeometry {points} />
			<MeshLineMaterial
				width={connection.state === "available" ? 0.3 : 0.5}
				{color}
				dashArray={connection.state === "available" ? 0.1 : 0}
				dashRatio={connection.state === "available" ? 0.6 : 0}
			/>
		</T.Mesh>
	{/each}

	<Galaxy />
	<Bloom />
	<Ambience playing={ctx.audioPlaying} />
</Canvas>
