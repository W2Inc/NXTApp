<script lang="ts">
	import { preview } from "$lib/utils/image.svelte.js";
	import Upload from "lucide-svelte/icons/upload";

	interface Props {
		src: string;
		name?: string;
	}

	const { src, name = "image" }: Props = $props();
	let fileUpload: HTMLInputElement = $state(null!);
</script>

<div class="group relative max-w-52">
	<input
		bind:this={fileUpload}
		type="file"
		{name}
		value=""
		class="absolute inset-0 z-10 cursor-pointer opacity-0"
	/>
	<div class="relative">
		<!-- NOTE(W2): avatarUrl, will always be a string here. -->
		<img
			use:preview={{ input: fileUpload }}
			alt="logo"
			class="max-h-52 w-full rounded border object-cover"
			{src}
		/>
		<div
			class="absolute inset-0 rounded bg-black/50 opacity-0 transition-opacity group-hover:opacity-100"
		>
			<Upload class="absolute inset-0 z-1 m-auto size-8 text-white" />
		</div>
	</div>
</div>
