<script lang="ts">
	import type { Icon } from "lucide-svelte";
	import type { Component } from "svelte";
	import Separator from "./ui/separator/separator.svelte";
	import type { IconLink } from "$lib/types";

	interface Props {
		title?: string;
		navs: IconLink[];
	}

	const { title, navs, ...rest }: Props = $props();
</script>

<div>
	{#if title}
		<p class="mb-1 text-sm font-semibold">{title}</p>
		<Separator class="mb-1" />
	{/if}

	<menu>
		{#each navs as nav}
			{@const Icon = nav.icon}
			<li
				class="gap-2-y hover:bg-muted before:bg-primary relative flex cursor-pointer list-none items-center justify-around rounded-md py-0.5 before:absolute before:left-[-2px] before:h-[20px] before:w-[6px] before:rounded-md before:opacity-0 hover:before:opacity-100"
			>
				<a
					{...rest}
					href={nav.href}
					class="focus-visible:ring-ring flex flex-1 items-center gap-2 rounded-md p-[0.35rem] focus-visible:outline-none focus-visible:ring-2"
				>
					<Icon size={20} class="ml-1" />
					{nav.title}
				</a>
				<span class=""></span>
			</li>
		{/each}
	</menu>
</div>
