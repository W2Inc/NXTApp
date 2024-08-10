<script lang="ts">
	import Item from './item.svelte';
	import { selectedFile, selectedName } from './state';
	import { ExplorerContext} from './context';
	import type { FileStub } from '$lib/types';
	import { Document } from 'svelte-hero-icons';

	export let file: FileStub;
	export let depth: number;

	const { select } = ExplorerContext.get();
</script>

<Item
	{depth}
	icon={Document}
	basename={file.basename}
	selected={file.name === $selectedName}
	on:click={() => {
		selectedName.set(file.name);
		select(file.name);
	}}
/>
