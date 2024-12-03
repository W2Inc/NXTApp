<script lang="ts">
	import { page } from "$app/stores";
	import Button from "$lib/components/ui/button/button.svelte";
	import Skeleton from "$lib/components/ui/skeleton/skeleton.svelte";
	import { SignIn, SignOut } from "@auth/sveltekit/components";

	const { data } = $props();
</script>

<div>
	<!-- TODO: Don't use their components... -->

	{#if $page.data.session}
		{#await data.data}
			<div class="flex items-center space-x-4">
				<Skeleton class="size-12 rounded-full" />
				<div class="space-y-2">
					<Skeleton class="h-4 w-[250px]" />
					<Skeleton class="h-4 w-[200px]" />
				</div>
			</div>
		{:then user}
			<pre><code>{JSON.stringify(user)}</code></pre>
		{/await}
	{:else}
		<p>Fuck off</p>
	{/if}
</div>
