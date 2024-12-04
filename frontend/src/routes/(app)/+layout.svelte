<script lang="ts">
	import "carta-md/default.css";
	import "../../app.css";
	import Header from "$lib/components/header.svelte";
	import Footer from "$lib/components/footer.svelte";
	import { mode, ModeWatcher } from "mode-watcher";
	import { Toaster } from "svelte-sonner";
	import DialogProvider from "$lib/components/dialog/dialog-provider.svelte";
	let { children } = $props();
</script>

<ModeWatcher />
<DialogProvider />
<Toaster closeButton richColors theme={$mode} duration={8000}/>
<Header />
<main class="h-[calc(100dvh-var(--header-height))]">
	{@render children()}
</main>
<!-- <Footer /> -->
