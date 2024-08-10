<script lang="ts">
	import Item from './item.svelte';
	import { files } from "./state";
	import { ExplorerContext } from './context';
	import { get_depth } from './utils';
	import File from './file.svelte';
	import type { DirectoryStub, FileStub, Stub } from '$lib/types';
	import { onMount } from 'svelte';
	import { Folder, FolderOpen } from 'svelte-hero-icons';


	export let directory: DirectoryStub;

	export let prefix: string;

	export let depth: number;

	export let contents: Stub[];


	const { collapsed } = ExplorerContext.get();

	$: segments = get_depth(prefix);

	$: children = contents
		.filter((file) => file.name.startsWith(prefix))
		.sort((a, b) => (a.name < b.name ? -1 : 1));

	$: child_directories = children.filter(
		(child) => get_depth(child.name) === segments && child.type === 'directory'
	) as DirectoryStub[];

	$: child_files = (
		children.filter((child) => get_depth(child.name) === segments && child.type === 'file')
	) as FileStub[];
</script>

<Item
	{depth}
	basename={directory.basename}
	icon={$collapsed[directory.name] ? Folder : FolderOpen}
	on:click={() => $collapsed[directory.name] = !$collapsed[directory.name]}
	on:keydown={(e) => {
		if (e.key === 'ArrowLeft' || e.key === 'ArrowRight') {
			$collapsed[directory.name] = e.key === 'ArrowLeft';
		}
	}}
/>

{#if !$collapsed[directory.name]}
	{#each child_directories as directory}
		<svelte:self {directory} prefix={directory.name + '/'} depth={depth + 1} contents={children} />
	{/each}

	{#each child_files as file}
		<File {file} depth={depth + 1} />
	{/each}
{/if}
