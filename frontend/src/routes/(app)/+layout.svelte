<script lang="ts">
	import "carta-md/default.css";
	import "../../app.css";
	import Header from "$lib/components/header.svelte";
	import Footer from "$lib/components/footer.svelte";
	import { mode, ModeWatcher } from "mode-watcher";
	import { Toaster } from "svelte-sonner";
	import DialogProvider from "$lib/components/dialog/dialog-provider.svelte";
	import Search from "$lib/components/search/search.svelte";
	import * as Tooltip from "$lib/components/ui/tooltip";
	let { children } = $props();
</script>

<Tooltip.Provider>
	<ModeWatcher />
	<DialogProvider />
	<Toaster closeButton richColors theme={$mode} duration={8000} />
	<Header />
	<main class="h-[calc(100dvh-var(--header-height))]">
		{@render children()}
	</main>
</Tooltip.Provider>

<!-- <Footer /> -->
