<script>
	import Github from "lucide-svelte/icons/github";
	import Linked from "lucide-svelte/icons/linkedin";
	import Twitter from "lucide-svelte/icons/twitter";
	import Link from "lucide-svelte/icons/link";
	import Button from "$lib/components/ui/button/button.svelte";
	import { Separator } from "$lib/components/ui/separator";
</script>

<h1 class="text-2xl">Socials</h1>
<p class="text-muted-foreground text-sm">
	Link your account to another Service Provider Interface (SPI).
	This lets you to login with the configured provider.
</p>
<Separator class="my-2" />
<div class="flex gap-2">
	<Button variant="outline">
		<Github />
		Github
		<Link />
	</Button>
	<Button variant="outline">
		<Twitter />
		Twitter
		<Link />
	</Button>
	<Button variant="outline">
		<Linked />
		LinkedIn
		<Link />
	</Button>
</div>
