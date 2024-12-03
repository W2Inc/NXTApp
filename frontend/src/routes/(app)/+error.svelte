<script>
	import { page } from "$app/stores";
	import { Home, LogIn } from "lucide-svelte";
	import { Button } from "$lib/components/ui/button";
</script>

<div>
	<img class="rounded" src="https://http.cat/{$page.status}" alt="Something went wrong..." />

	{#if $page.status == 401}
		<Button href="/auth/login">
			Login
			<LogIn />
		</Button>
	{:else}
		<Button href="..">
			Go back
			<Home />
		</Button>
	{/if}
</div>

<style>
	div {
		position: absolute;
		top: 50%;
		left: 50%;
		transform: translate(-50%, -50%);

		display: grid;
		place-items: center;
		gap: 1rem;
	}
</style>
