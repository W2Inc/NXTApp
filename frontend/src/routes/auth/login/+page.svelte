<script lang="ts">
	import Logo from "$assets/logo.png";
	import GH from "$assets/github//github-mark-white.svg";
	import Button from "$lib/components/buttons/button.svelte";
	import Odyssey from "$lib/components/misc/odyssey/odyssey.svelte";
	import Link from "$lib/components/links/link.svelte";
	import HardwareCheck from "$lib/components/misc/hardware-check.svelte";

	let rotation = 3.14;
	let target: HTMLDivElement;

	function setRotation(clientX: number, clientY: number) {
		const { top, right, bottom, left } = target.getBoundingClientRect();
		const y = (left + right) / 2 - clientX;
		const x = clientY - (top + bottom) / 2;
		rotation = Math.atan2(y, x);
	}
</script>

<svelte:window on:mousemove={(e) => setRotation(e.clientX, e.clientY)} />

<HardwareCheck>
	<Odyssey />
</HardwareCheck>

<img class="logo" src={Logo} width="96" alt="Company logo" />
<div bind:this={target} class="container" style="--rotation: {rotation}rad">
	<div class="mask">
		<h1 class="signin">Sign in</h1>
		<p>
			By signing in, you agree to our <Link href="/terms">terms</Link> and
			<Link href="/privacy">privacy policy</Link>
		</p>
		<form method="POST" action="/auth/login">
			<Button type="submit" style="width: 100%;">
				Sign in with Github
				<img src={GH} width="24" alt="Github logo" />
			</Button>
		</form>
	</div>
</div>

<style>
	h1.signin {
		padding: none;
		text-align: center;
		color: white;
	}

	p {
		font-size: 0.8rem;
		text-align: center;
		color: white;
	}

	img.logo {
		border-radius: 8px;
		position: absolute;
		top: 0;
		left: 0;
	}

	div.mask {
		border-radius: 15px;
		padding: 32px;
		background-color: rgba(0, 0, 0, 0.8);
		box-shadow: var(--box-shadow);
	}

	div.container {
		max-width: 360px;
		border-radius: 16px;

		padding: 2px;
		background-image: linear-gradient(
			var(--rotation),
			#f81ce5 0,
			#7928ca 20%,
			rgba(121, 40, 202, 0) 80%
		);

		@media (--md) {
			width: 256px;
		}
	}
</style>
