<script>
	import { onMount } from "svelte";
	import toast from "sonner-svelte";
	import { page } from "$app/stores";
	import Error from "$assets/textures/error.gif";
	import Button from "$lib/components/buttons/button.svelte";
	import { ArrowLeftOnRectangle, Home, Icon } from "svelte-hero-icons";

	onMount(() => {
		toast.dismiss();
		toast.error(`Oops, something went wrong: ${$page.status}`);
	});
</script>

<div>
	<h1>Oops!</h1>
	<p>
		{$page.status}
		-
		{#if $page.error}
			{$page.error.message}
		{:else}
			Something went wrong, please try again later.
		{/if}
	</p>
	<img src={Error} alt="Error" />

	{#if $page.status == 401}
		<Button href="/auth/login">
			Login
			<Icon src={ArrowLeftOnRectangle} size="18px" />
		</Button>
	{:else}
		<Button href="/">
			Go back
			<Icon src={Home} size="18px" />
		</Button>
	{/if}
</div>

<style>
	div {
		position: absolute;
		top: 50%;
		left: 50%;
		transform: translate(-50%, -50%);

		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 1rem;

		& img {
			border-radius: var(--br);
		}
	}
</style>
