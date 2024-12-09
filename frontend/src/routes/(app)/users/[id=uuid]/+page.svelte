<script lang="ts">
	import { page } from "$app/stores";
	import Navgroup from "$lib/components/navgroup.svelte";
	import type { IconLink } from "$lib/types";
	import { decodeUUID64, encodeUUID64 } from "$lib/utils.svelte.js";
	import Trophy from "lucide-svelte/icons/trophy";
	import Archive from "lucide-svelte/icons/archive";
	import GraduationCap from "lucide-svelte/icons/graduation-cap";
	import User from "lucide-svelte/icons/user";
	import Sparkles from "lucide-svelte/icons/sparkles";
	import { Markdown } from "carta-md";
	import { cartaContext } from "$lib/components/markdown/context.svelte";
	import * as Avatar from "$lib/components/ui/avatar";

	const { data } = $props();
	const id = $page.params.id;
	const links = $state.raw<IconLink[]>([
		{
			icon: Archive,
			href: `./${id}/projects`,
			title: "Projects",
		},
		{
			icon: Trophy,
			href: `./${id}/goals`,
			title: "Goals",
		},
		{
			icon: GraduationCap,
			href: `./${id}/cursus`,
			title: "Cursus",
		},
		{
			icon: Sparkles,
			href: `./${id}/galaxy`,
			title: "Galaxy",
		},
	]);
</script>

<div class="m-auto max-w-5xl px-4 py-8">
	<div
		class="flex p-8 rounded gap-4 bg-[url('/img.png')]"
	>
		<Avatar.Root class="size-32 rounded-sm border border-gray-700">
			<Avatar.Image
				src="https://github.com/w2wizard.png"
				class="border-gray-400"
				alt="@w2wizard"
			/>
			<Avatar.Fallback class="rounded-sm">CN</Avatar.Fallback>
		</Avatar.Root>
		<div class="backdrop-blur-sm flex-1 rounded-lg px-4 py-2 bg-gray-700 bg-opacity-[0.2]">
			<h1 class="md:text-2xl sm:text-lg font-bold">Display Name</h1>
			<div class="flex gap-1">
				<a
					href="https://github.com/w2wizard"
					target="_blank"
					class="text-blue-500 hover:underline">GitHub</a
				>
				<a
					href="https://linkedin.com/in/w2wizard"
					target="_blank"
					class="text-blue-500 hover:underline">LinkedIn</a
				>
			</div>
		</div>
	</div>
	<div class="grid grid-cols-1 md:grid-cols-[256px,1fr] gap-2 pt-4">
		<Navgroup title="General" navs={links}></Navgroup>
		<div class="max-h-60 overflow-auto rounded border p-4">
			<Markdown value="~~Markdown~~" carta={cartaContext} theme="github" />
		</div>
	</div>
</div>
