<script>
	//@ts-nocheck
	import "../app.postcss";
	import { onNavigate } from "$app/navigation";
	import { Toaster } from "sonner-svelte";

	onNavigate(() => {
		document.querySelectorAll("dialog").forEach((dialog) => dialog.close());
		document
		.querySelectorAll("details")
		.forEach((detail) => detail.removeAttribute("open"));
	});
</script>

<Toaster closeButton richColors expand theme="system"/>
<slot />
