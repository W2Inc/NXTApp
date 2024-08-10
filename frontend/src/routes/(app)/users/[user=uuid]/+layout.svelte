<script lang="ts">
	import { page } from "$app/stores";
	import type { LayoutData } from "./$types";
	export let data: LayoutData;
</script>

<svelte:head>
	<!-- To let the breadcrumb component know what to translate the ID into -->
	<meta name="[user=uuid]" content={data.user.data.display_name} />
</svelte:head>

<slot />
