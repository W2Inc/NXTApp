<script lang="ts">
	import { page } from "$app/stores";
	import Button from "$lib/components/ui/button/button.svelte";
	import Taskcard from "$lib/components/taskcard.svelte";
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
			<!-- <pre><code>{JSON.stringify(data.session?.user)}</code></pre> -->
		{/await}
	{:else}
	<p>Login lol</p>
	{/if}
	<div class="flex gap-6 p-5">
		<Taskcard title="Project A" type="goal" state="Active" href="/"></Taskcard>
		<Taskcard title="Project B" state="Awaiting" href="/"></Taskcard>
		<Taskcard title="Project C" type="goal" state="Completed" href="/"></Taskcard>
		<Taskcard title="Project D" state="Inactive" href="/"></Taskcard>
	</div>
</div>
