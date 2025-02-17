<script lang="ts">
	import { page } from "$app/state";
	import Navgroup from "$lib/components/navgroup.svelte";
	import type { IconLink } from "$lib/types";
	import { decodeID, encodeID } from "$lib/utils.js";
	import Trophy from "lucide-svelte/icons/trophy";
	import Archive from "lucide-svelte/icons/archive";
	import GraduationCap from "lucide-svelte/icons/graduation-cap";
	import User from "lucide-svelte/icons/user";
	import Sparkles from "lucide-svelte/icons/sparkles";
	import * as Avatar from "$lib/components/ui/avatar";
	import Markdown from "$lib/components/markdown/markdown.svelte";

	const { data } = $props();
	const id = page.params.id;
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

	const socials = [
		{ label: "Website", url: data.user.details?.websiteUrl },
		{ label: "LinkedIn", url: data.user.details?.linkedinUrl },
		{ label: "Twitter", url: data.user.details?.twitterUrl },
		{ label: "GitHub", url: data.user.details?.githubUrl },
	].filter((s) => s.url);
</script>

<div class="m-auto max-w-5xl px-4 py-8">
	<div class="flex gap-4 rounded bg-[url('/img.png')] p-8">
		<Avatar.Root class="size-32 rounded-sm border border-gray-700">
			<Avatar.Image
				src={data.user.avatarUrl}
				class="border-gray-400"
				alt={data.user.login}
			/>
			<Avatar.Fallback class="rounded-sm">CN</Avatar.Fallback>
		</Avatar.Root>
		<div
			class="flex-1 rounded-lg bg-gray-700 bg-opacity-[0.2] px-4 py-2 backdrop-blur-sm"
		>
			<h1 class="font-bold sm:text-lg md:text-2xl">{data.user.displayName}</h1>
			<div class="flex gap-1">
				{#each socials as social}
					<a href={social.url} target="_blank" class="text-blue-500 hover:underline"
						>{social.label}</a
					>
				{/each}
			</div>
		</div>
	</div>
	<div class="grid grid-cols-1 gap-2 pt-4 md:grid-cols-[256px,1fr]">
		<Navgroup title="General" navs={links}></Navgroup>
		{#if data.bio}
			<Markdown variant="viewer" value={data.bio} />
		{/if}
	</div>
</div>
