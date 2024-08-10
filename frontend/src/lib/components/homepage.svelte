<script lang="ts">
	import Galaxy from "$lib/galaxy/galaxy.svelte";
	import { Canvas, T } from "@threlte/core";
	import { Environment, OrbitControls } from "@threlte/extras";
	import Fallback from "$assets/fallback-homepage.png?enhanced";
	import { afterUpdate, getContext, onMount } from "svelte";
	import HardwareCheck from "./misc/hardware-check.svelte";
	import Bloom from "$lib/galaxy/effects/bloom.svelte";
	import Footer from "./footer.svelte";
	import { ChevronDown, Icon } from "svelte-hero-icons";
	import Fade from "./misc/fade/fade.svelte";
	import Observer from "./misc/fade/observer.svelte";
	import Promo from "./promo.svelte";
	import { cdnURL } from "$lib/api/api";
	import Graph from "$lib/graph/graph.svelte";
	import type { ClientGraph } from "$lib/graph/builder";
	import toast from "sonner-svelte";

	afterUpdate(() => {
		const observer = new IntersectionObserver((entries) => {
			entries.forEach((entry) => {
				if (entry.isIntersecting) {
					entry.target.classList.add("visible");
				}
			});
		});

		document.querySelectorAll(".hidden").forEach((element) => {
			observer.observe(element);
		});
	});

	let width = 400;
	let height = 200;
	let parent: HTMLElement;
	let goodGPU = true;
	export let graphData: ClientGraph;

	function onResize() {
		width = parent.offsetWidth;
		height = parent.offsetHeight;
	}

	onMount(() => {
		onResize();
	});
</script>

<svelte:window on:resize={() => onResize()} />

<Observer />
<div class="main-page">
	<section bind:this={parent} class="introduction" class:fallback={true}>
			<HardwareCheck bind:goodGPU>
				<Canvas size={{ width, height }}>
					<T.PerspectiveCamera
						makeDefault
						position={[0, 25, -50]}
						fov={45}
						on:create={({ ref }) => ref.lookAt(0, 0, 0)}
					>
						<OrbitControls
							enabled={true}
							autoRotate
							enableDamping
							enableZoom={false}
							dampingFactor={0.025}
							maxDistance={1000}
							autoRotateSpeed={Math.PI / 64}
						/>
					</T.PerspectiveCamera>
					<Graph {graphData}/>
					<Environment files={`${cdnURL}/blob/envmap/hdr.png`} isBackground={true} />
					<Galaxy />
					<Bloom />
				</Canvas>

				<svelte:fragment slot="fallback">
					<!--<enhanced:img src={Fallback} alt="A bright yet blurred galaxy" />-->
				</svelte:fragment>
			</HardwareCheck>

		<div class="hero">
			<Fade direction="left">
				<h1>25x0</h1>
				<h2>Take back control of your learning!</h2>
				<hr />
				<p>
					25x0 ("25 Zeroes") is an open online learning platform. We
					provide you with the tools, resources and community so you can learn what you want, when you want.
				</p>
			</Fade>
		</div>

		<a href="#first" class="hidden scroll-down center v">
			Scroll down
			<Icon src={ChevronDown} size="64px" solid />
		</a>
	</section>

	<!-- SEE: https://v0.dev/t/gSk0pJkC37I -->

	<div class="gradient up" />
	<section id="first" class="first-section">
		<Promo />
	</section>
	<div class="gradient down" />

	<section id="#projects-section" class="last-section">
		<Footer />
	</section>
</div>

<style>
	h1 {
		font-size: 2rem;
	}

	@keyframes buttonDown {
		0% {
			transform: translateY(-16px);
		}
		50% {
			transform: translateY(0px);
		}
		100% {
			transform: translateY(-16px);
		}
	}

	.main-page {
		--page-bg: rgb(4, 4, 4);
		--section-bg: hsl(221.54, 37.14%, 6.86%);

		background-color: var(--page-bg);
	}

	a.scroll-down {
		background-color: transparent;
		position: absolute;
		bottom: 64px;
		gap: 0;
		animation: buttonDown 2s ease-in-out infinite;
		color: white;
		text-decoration: none;
		transition: color 0.3s ease-in-out;

		&:hover {
			color: var(--primary);
		}
	}

	section {
		position: relative;
		padding: 64px 3rem 0px;
		overflow: hidden;
		/*height: 100lvh;*/

		&.introduction {
			height: 100lvh;
			padding: unset;
			z-index: 0;

			&.fallback {
				display: grid;
				place-items: center;
			}
		}

		&.first-section {
			background: var(--section-bg);
			overflow-x: hidden;

			@media screen and (max-width: 768px) {
				padding: 8px;
				margin: 0;
			}
		}

		&.last-section {
			padding: 16px;
			height: unset;
			background-color: transparent;
		}
	}

	.hero {
		top: 0;
		left: 0;
		position: absolute;
		padding: 64px 8rem 0px;
		color: #fff;

		& h1 {
			padding: 0;
			font-size: 4em;
			color: transparent;
			background: linear-gradient(347deg, var(--primary), rgb(246, 255, 0));
			background-clip: text;

			@media screen and (max-width: 420px) {
				font-size: 2em;
			}
		}

		& p {
			max-width: 576px;
		}

		@media screen and (max-width: 768px) {
			padding: 64px 1.5rem 0px;
		}

		@media screen and (max-width: 480px) {
			padding: 64px 1rem 0px;
			& h1 {
				font-size: 2.5rem;
			}
		}
	}

	div.gradient {
		height: 10lvh;

		&.up {
			background: linear-gradient(var(--page-bg) 5.79%, var(--section-bg));
		}

		&.down {
			background: linear-gradient(var(--section-bg), var(--page-bg) 94.21%);
		}
	}
</style>
