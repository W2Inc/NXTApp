<script lang="ts">
	import "../../app.css";
	import Header from "$lib/components/header.svelte";
	import Footer from "$lib/components/footer.svelte";
	import { mode, ModeWatcher } from "mode-watcher";
	import { Toaster } from "svelte-sonner";
	let { children } = $props();
</script>

<ModeWatcher />
<Toaster closeButton richColors theme={$mode} duration={8000}/>
<Header />
<main class="h-[calc(100dvh-var(--header-height))]">
	{@render children()}
</main>
<!-- <Footer /> -->
