<script lang="ts">
	import Flask from "lucide-svelte/icons/flask-conical";
	import { Separator } from "$lib/components/ui/separator";
	import * as Alert from "$lib/components/ui/alert";
</script>

<h1 class="text-2xl">API Applications</h1>
<p class="text-muted-foreground text-sm">
	You can create your own API Applications to interact with the API.
</p>
<Separator class="my-2" />
<div class="mb-2 flex gap-2">
	<Alert.Root variant="warning">
		<Flask class="size-4" />
		<Alert.Title>Not yet Supported</Alert.Title>
		<Alert.Description>Applications are not yet supported.</Alert.Description>
	</Alert.Root>
</div>
