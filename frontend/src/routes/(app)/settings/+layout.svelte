<script lang="ts">
	import Navgroup from "$lib/components/navgroup.svelte";
	import User from "lucide-svelte/icons/user";
	import Socials from "lucide-svelte/icons/book-user";
	import FlaskConical from "lucide-svelte/icons/flask-conical";
	import Computer from "lucide-svelte/icons/computer";

	let { children } = $props();
	let recentNavigations = [
		{
			icon: User,
			href: "./user",
			title: "Profile",
		},
		{
			icon: Computer,
			href: "./sessions",
			title: "Sessions",
		},
		{
			icon: FlaskConical,
			href: "./features",
			title: "Features",
		},
		{
			icon: Socials,
			href: "./socials",
			title: "Socials",
		},
	];
</script>

<div class="mx-auto flex max-w-screen-lg flex-col gap-4 px-8 py-16 md:flex-row">
	<aside class="md:sticky md:top-0 min-w-64 h-fit pt-4">
		<Navgroup title="Profile Settings" navs={recentNavigations} />
	</aside>
	<div class="flex-1">
		{@render children()}
	</div>
</div>

<style>
</style>
