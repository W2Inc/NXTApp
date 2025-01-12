<script lang="ts">
	import Github from "lucide-svelte/icons/github";
	import Linked from "lucide-svelte/icons/linkedin";
	import Link from "lucide-svelte/icons/link";
	import Check from "lucide-svelte/icons/check";
	import Button from "$lib/components/ui/button/button.svelte";
	import Flask from "lucide-svelte/icons/flask-conical";
	import { Separator } from "$lib/components/ui/separator";
	import * as Alert from "$lib/components/ui/alert";

	const { data } = $props();
</script>

{#snippet SPI(name: string, providerId: string, linkable: boolean)}
	<form method="post">
		<input hidden readonly value={providerId} name="providerId" />
		{#if linkable}
			<Button type="submit" variant="outline" formaction="?/link">
				<Github />
				<span class="capitalize">{name}</span>
				<Link />
			</Button>
		{:else}
			<Button variant="outline" formaction="?/unlink">
				<Github />
				<span class="capitalize">{name}</span>
				<Check />
			</Button>
		{/if}
	</form>
{/snippet}

<h1 class="text-2xl">Socials</h1>
<p class="text-muted-foreground text-sm">
	Linking your account lets us access your other accounts and manage repositories, etc...
</p>
<Separator class="my-2" />
<div class="mb-2 flex gap-2">
	{#if data.realmIdentities.length === 0}
		<Alert.Root>
			<Flask class="size-4" />
			<Alert.Title>No socials availble</Alert.Title>
			<Alert.Description>
				This app has no socials setup.
			</Alert.Description>
		</Alert.Root>
	{:else}
		{#each data.realmIdentities as identity}
			{@const hasThisIdentity = data.identities.some(
				(i) => i.identityProvider === identity.providerId,
			)}
			{@render SPI(
				identity.alias ?? identity.providerId!,
				identity.providerId!,
				!hasThisIdentity,
			)}
		{/each}
	{/if}
</div>
