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
	import { afterNavigate, onNavigate } from "$app/navigation";
	import { useStorage } from "$lib/utils/local.svelte";
	import type { NamedLink } from "$lib/types";

	let { children } = $props();

	const MAX_HISTORY = 5;
	const storage = useStorage<NamedLink[]>();

	onNavigate(({ to, from }) => {
		if (!from) return;
		const link = { href: from.url.href, title: document.title };
		const history = storage.get("app:history") ?? [];
		storage.set("app:history", [
			...history.filter((nav) => nav.href !== from.url.href).slice(-MAX_HISTORY + 1),
			link,
		]);
	});
</script>

<Tooltip.Provider>
	<ModeWatcher />
	<DialogProvider />
	<Toaster closeButton richColors theme={$mode} duration={8000} />
	<Header />
	<main>
		{@render children()}
	</main>
</Tooltip.Provider>

<!-- <Footer /> -->
