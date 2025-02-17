<script lang="ts">
	// import Logo from "$assets/logo.png";
	import { page } from "$app/state";
	import { Button } from "../ui/button";
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
	import Navgroup from "../navgroup.svelte";
	import * as Dialog from "$lib/components/ui/dialog";
	import Separator from "../ui/separator/separator.svelte";
	import Input from "../ui/input/input.svelte";
	import { redirect } from "@sveltejs/kit";
	import { goto, onNavigate } from "$app/navigation";
	import type { Component } from "svelte";
	import Heart from "lucide-svelte/icons/heart";
	import type { IconLink } from "$lib/types";
	import { encodeID } from "$lib/utils";


	const id = encodeID(page.data.session?.user_id ?? "null");
	const links = $state.raw<IconLink[]>([
		{
			icon: User,
			href: id ? `/users/${id}/` : `/users/`,
			title: "Profile",
		},
		{
			icon: ArchiveBox,
			href: id ? `/users/${id}/projects` : `/projects`,
			title: "Projects",
		},
		{
			icon: Trophy,
			href: id ? `/users/${id}/goals` : `/goals`,
			title: "Goals",
		},
		{
			icon: AcademiaCap,
			href: id ? `/users/${id}/cursus` : `/cursus`,
			title: "Cursus",
		},
		{
			icon: Sparkles,
			href: id ? `/users/${id}/galaxy` : `/galaxy`,
			title: "Galaxy",
		},
	]);

	const general = $state.raw<IconLink[]>([
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
			href: id ? `/users/${id}/feedback` : `/users/feedback`,
			title: "Feedback",
		},
	]);

	let query = $state("");
	let open = $state(false);
	function handleKeydown(e: KeyboardEvent) {
		if (e.key === "k" && (e.metaKey || e.ctrlKey)) {
			e.preventDefault();
			open = !open;
		}
	}

	onNavigate(() => {
		open = false;
	});
</script>

<svelte:document onkeydown={handleKeydown} />

<Button
	variant="outline"
	class="relative w-full justify-start text-sm text-muted-foreground sm:pr-12 md:w-64 lg:w-64"
	onclick={() => (open = true)}
>
	<span class="items-center lg:inline-flex"> Search website... </span>
	<kbd
		class="pointer-events-none absolute right-1.5 top-1.5 hidden select-none items-center gap-1 rounded border bg-muted px-1.5 font-mono text-[10px] font-medium opacity-100 sm:flex"
	>
		<span class="text-xs">⌘</span>K
	</kbd>
</Button>

<Dialog.Root bind:open>
	<Dialog.Content
		class="left-0 top-0 h-full translate-x-[unset] translate-y-[unset] overflow-auto data-[state=closed]:slide-out-to-top-[unset] data-[state=open]:slide-in-from-left-[unset] sm:max-w-[425px]"
	>
		<div class="flex flex-col gap-2">
			<div class="flex-1">
				<Dialog.Header class="flex-shrink-1 text-left">
					<Dialog.Title class="pb-2">NXTApp</Dialog.Title>
					<Dialog.Description>
						<form
							role="search"
							onsubmit={(e) => {
								open = false;
								e.preventDefault();
								goto(`search?q=${query}`);
							}}
						>
							<Input
								type="search"
								class="mb-2"
								placeholder="Search..."
								name="q"
								bind:value={query}
							/>
						</form>
					</Dialog.Description>
				</Dialog.Header>
				<Navgroup title={id ? "Profile" : "Explore"} navs={links}></Navgroup>
				<Navgroup title="General" navs={general}></Navgroup>
			</div>

			<div class="text-muted-foreground">
				<p>
					Made with <Heart size={16} fill="currentColor" class="inline" /> by © 2023-2024
					W2Inc, B.V.
				</p>
				<!-- <p>Made with ❤️ by © 2023-2024 W2Inc, B.V.</p> -->
				<ul class="flex gap-2 text-sm text-primary underline">
					<li><a href="/terms">Terms</a></li>
					<li><a href="/privacy">Privacy</a></li>
					<li><a href="/pricing">Pricing</a></li>
				</ul>
			</div>
		</div>
	</Dialog.Content>
</Dialog.Root>
