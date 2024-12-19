<script lang="ts">
	import "../../../app.css";
	import { mode, ModeWatcher } from "mode-watcher";
	import { Toaster } from "svelte-sonner";
	import DialogProvider from "$lib/components/dialog/dialog-provider.svelte";
	import * as Tooltip from "$lib/components/ui/tooltip";

	let { children } = $props();
</script>

<Tooltip.Provider>
	<ModeWatcher />
	<DialogProvider />
	<Toaster closeButton richColors theme={$mode} duration={8000} />
	<main>
		{@render children()}
	</main>
</Tooltip.Provider>
