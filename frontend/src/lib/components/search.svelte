<script lang="ts">
	// import Logo from "$assets/logo.png";
	import { page } from "$app/stores";
	import { Button } from "./ui/button";
	import X from "lucide-svelte/icons/x";
	import User from "lucide-svelte/icons/user";
	import Users from "lucide-svelte/icons/users";
	import ArchiveBox from "lucide-svelte/icons/archive";
	import Trophy from "lucide-svelte/icons/trophy";
	import AcademiaCap from "lucide-svelte/icons/graduation-cap";
	import Sparkles from "lucide-svelte/icons/sparkles";
	import Home from "lucide-svelte/icons/home";
	import Cog from "lucide-svelte/icons/cog";
	import Feedback from "lucide-svelte/icons/message-circle-heart";
	import Navgroup from "./navgroup.svelte";
	import * as Dialog from "$lib/components/ui/dialog";
	import Separator from "./ui/separator/separator.svelte";

	const id = $page.data.session?.user?.id;

	let profileLinks = [
		{
			icon: User,
			href: `/user/${id}/`,
			title: "Profile",
		},
		{
			icon: ArchiveBox,
			href: `/user/${id}/projects`,
			title: "Projects",
		},
		{
			icon: Trophy,
			href: `/user/${id}/projects`,
			title: "Goals",
		},
		{
			icon: AcademiaCap,
			href: `/user/${id}/cursus`,
			title: "Cursus",
		},
		{
			icon: Sparkles,
			href: `/user/${id}/galaxy`,
			title: "Galaxy",
		},
	];

	let generalLinks = [
		{
			icon: Home,
			href: `/`,
			title: "Home",
		},
		{
			icon: Cog,
			href: `/settings`,
			title: "Settings",
		},
		{
			icon: Users,
			href: `/users`,
			title: "Users",
		},
		{
			icon: Feedback,
			href: `/user/${id}/feedback`,
			title: "Feedback",
		},
	];

	let open = $state(false);
	function handleKeydown(e: KeyboardEvent) {
		if (e.key === "k" && (e.metaKey || e.ctrlKey)) {
			e.preventDefault();
			open = !open;
		}
	}
</script>

<svelte:document onkeydown={handleKeydown} />

<Button
	variant="outline"
	class="text-muted-foreground relative w-full justify-start text-sm sm:pr-12 md:w-40 lg:w-64"
	onclick={() => (open = true)}
>
	<span class="hidden items-center lg:inline-flex"> Search website... </span>
	<span class="inline-flex lg:hidden">Search...</span>
	<kbd
		class="bg-muted pointer-events-none absolute right-1.5 top-1.5 hidden select-none items-center gap-1 rounded border px-1.5 font-mono text-[10px] font-medium opacity-100 sm:flex"
	>
		<span class="text-xs">⌘</span>K
	</kbd>
</Button>

<Dialog.Root bind:open>
	<Dialog.Content
		class="data-[state=closed]:slide-out-to-top-[unset] data-[state=open]:slide-in-from-left-[unset] left-0 top-0 h-full translate-x-[unset] translate-y-[unset] sm:max-w-[425px]"
	>
		<div class="flex flex-col gap-2">
			<div class="flex-1">
				<Dialog.Header class="flex-shrink-1 text-left">
					<Dialog.Title class="pb-2">NXTApp</Dialog.Title>
				</Dialog.Header>
				<Navgroup tabindex="0" title="Profile" navs={profileLinks}></Navgroup>
				<Navgroup title="General" navs={generalLinks}></Navgroup>
			</div>

			<div class="text-muted-foreground">
				<p>Made with ❤️ by © 2023 Nextdemy, B.V.</p>
				<ul class="flex gap-2 text-primary underline text-sm">
					<li><a href="/terms">Terms</a></li>
					<li><a href="/privacy">Privacy</a></li>
					<li><a href="/pricing">Pricing</a></li>
				</ul>
			</div>
		</div>
	</Dialog.Content>
</Dialog.Root>
