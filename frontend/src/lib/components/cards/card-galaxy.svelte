<script lang="ts">
	import Galaxy from "$lib/galaxy/galaxy.svelte";
	import { Canvas, T } from "@threlte/core";
	import Card from "./card.svelte";
	import { Icon, ArrowsPointingOut } from "svelte-hero-icons";
	import { onMount } from "svelte";
	import Button from "../buttons/button.svelte";
	import Loading from "../misc/loading.svelte";
	import { OrbitControls } from "@threlte/extras";
	import Bloom from "$lib/galaxy/effects/bloom.svelte";
	import { page } from "$app/stores";

	let width = 0;
	let height = 0;
	let isReady = false;
	let parent: HTMLDivElement;
	let header: HTMLMenuElement;

	function onResize() {
		width = parent.clientWidth;
		height = parent.clientHeight;
	}

	onMount(() => {
		isReady = true;
		onResize();
	});
</script>

<svelte:window on:resize={() => onResize()} />

<Card>
	<div bind:this={parent} class="card">
		<menu bind:this={header}>
			<h1>Your Galaxy</h1>
			<hr />
			<li>
				<Button title="Open your galaxy" type="button" href="/users/{$page.data.me?.data.id}/graph">
					<Icon src={ArrowsPointingOut} size="24" />
				</Button>
			</li>
		</menu>
		{#if isReady}
			<div class="blur">
				<Canvas size={{ width, height }}>
					<T.PerspectiveCamera
						makeDefault
						position={[0, 750, -1000]}
						fov={65}
						on:create={({ ref }) => ref.lookAt(0, 0, 0)}
					>
						<OrbitControls
							enableZoom
							autoRotate
							enableDamping
							dampingFactor={0.025}
							maxDistance={1000}
							autoRotateSpeed={Math.PI / 128}
						/>
					</T.PerspectiveCamera>
					<Galaxy />
					<Bloom />
				</Canvas>
			</div>
		{:else}
			<Loading />
		{/if}
	</div>
</Card>

<style>
	menu {
		display: flex;
		position: absolute;
		top: 0;
		left: 0;
		justify-content: space-between;
		width: 100%;
		align-items: center;
		z-index: 2;
		padding: 8px;

		& h1 {
			padding: 0;
			user-select: none;
			color: aliceblue;
		}

		& hr {
			flex: 1;
			margin: 0 32px;
		}

		& li {
			list-style: none;
			padding: 0;
			margin: 0;
			font-size: 0.8rem;
			color: var(--text-secondary);
		}
	}

	div.card {
		position: relative;
		display: flex;
		justify-content: space-evenly;
		align-items: center;
		height: 360px;
		transition: filter 0.2s ease-in-out;
		border-radius: 32px;

		& .blur {
			position: absolute;
			top: 0;
			left: 0;
			filter: blur(2px);
			clip-path: ellipse(100% 100% at 50% 50%);
			background-color: rgba(0, 0, 0, 0.9);
		}

		& a {
			cursor: pointer;
			position: absolute;
			border-radius: var(--br);
			display: grid;
			place-items: center;
			color: white;
			transition: color var(--transition);

			&:hover {
				color: var(--primary);
			}
		}
	}
</style>
