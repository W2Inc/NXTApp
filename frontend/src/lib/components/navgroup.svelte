<script lang="ts">
	import type { Icon } from "lucide-svelte";
	import type { Component } from "svelte";
	import Separator from "./ui/separator/separator.svelte";

	interface Props {
		title?: string;
		navs: {
			href: string;
			title: string;
			icon: Component<Icon>;
		}[];
	}

	const { title, navs }: Props = $props();
</script>

<div>
	{#if title}
		<p class="mb-1 text-sm font-semibold">{title}</p>
		<Separator class="mb-1" />
	{/if}

	<menu>
		{#each navs as nav}
			<li
				class="gap-2-y py-0.5 hover:bg-muted before:bg-primary relative flex cursor-pointer list-none items-center justify-around rounded-md before:absolute before:left-[-2px] before:h-[20px] before:w-[6px] before:rounded-md before:opacity-0 hover:before:opacity-100"
			>
				<a href={nav.href} class="flex flex-1 focus-visible:ring-ring focus-visible:outline-none focus-visible:ring-2 items-center gap-2 rounded-md p-2">
					<svelte:component this={nav.icon} size={20} />
					{nav.title}
				</a>
				<span class=""></span>
			</li>
		{/each}
	</menu>
</div>
