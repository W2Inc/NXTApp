<script lang="ts">
	import "../../app.css";
	import Header from "$lib/components/header.svelte";
	import Footer from "$lib/components/footer.svelte";
	import { mode, ModeWatcher } from "mode-watcher";
	import { Toaster } from "svelte-sonner";
	import DialogProvider from "$lib/components/dialog/dialog-provider.svelte";
	import Search from "$lib/components/search/search.svelte";
	import * as Tooltip from "$lib/components/ui/tooltip";
	import { afterNavigate, onNavigate } from "$app/navigation";
	import { useStorage } from "$lib/utils/local.svelte";
	import type { NamedLink } from "$lib/types";
	import type { LayoutProps } from "./$types";

	const { data, children }: LayoutProps = $props();
</script>

<Tooltip.Provider>
	<ModeWatcher defaultMode="system" />
	<Toaster closeButton richColors duration={8000} />
	<DialogProvider />
	{#if data.session}
		<Header />
	{/if}
	<main>
		{@render children?.()}
	</main>
</Tooltip.Provider>
