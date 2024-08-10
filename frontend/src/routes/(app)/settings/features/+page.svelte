<script lang="ts">
	import Featurecard from "$lib/components/cards/card-feature.svelte";
	import Notice from "$lib/components/cards/card-notice.svelte";
	import Loading from "$lib/components/misc/loading.svelte";
	import { onMount } from "svelte";
	import type { PageServerData } from "./$types";

	export let data: PageServerData;
</script>

<svelte:head>
	<title>Features</title>
	<meta name="description" content="Feature settings" />
</svelte:head>

<h1>Feature Page</h1>
<p>Get access to the latest features and updates for testing and feedback.</p>

<hr />

{#if data.features.length <= 0}
	<Notice type="info">
		<p>There are no features available at the moment</p>
	</Notice>
{:else}
	{#each data.features as feature}
		<Featurecard enabled={false} {feature} />
	{/each}
{/if}
