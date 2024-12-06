<script>
	import Button from "$lib/components/ui/button/button.svelte";
	import { Separator } from "$lib/components/ui/separator";
	import * as Alert from "$lib/components/ui/alert";
	import Flask from "lucide-svelte/icons/flask-conical";
</script>

<h1 class="text-2xl">Features</h1>
<p class="text-muted-foreground text-sm">
	Get early access to features that will make into main.
</p>
<Separator class="my-2" />
<Alert.Root>
	<Flask class="size-4" />
	<Alert.Title>Nothing here yet</Alert.Title>
	<Alert.Description>
		At the moment there are no new preview features available!
	</Alert.Description>
</Alert.Root>
