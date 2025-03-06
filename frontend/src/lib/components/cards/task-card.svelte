<script lang="ts">
	import Trophy from "lucide-svelte/icons/trophy";
	import Archive from "lucide-svelte/icons/archive";

	interface Props {
		href: string;
		title: string;
		type?: "project" | "goal";
		state?: BackendTypes["TaskState"];
	}

	const { href, title, state, type = "project" }: Props = $props();
</script>

<a
	{href}
	class="flex-1 bg-muted h-48 max-w-96 transform rounded p-3 no-underline shadow motion-safe:transition-transform hover:scale-[1.025]"
>
	<div class="grid h-full place-content-center justify-items-center gap-2">
		{#if type === "project"}
			<Archive size={32} />
		{:else}
			<Trophy size={32} />
		{/if}
		<span>{title}</span>
	</div>
	{#if state}
		<div class="status-tag inline" data-status={state}></div>
	{/if}
</a>

<style>
	.status-tag::before {
		@apply relative top-0 inline-block h-6 rounded px-4 font-bold text-white shadow;
	}

	.status-tag[data-status="Active"]::before {
		content: "Active";
		@apply bg-green-500;
	}

	.status-tag[data-status="Inactive"]::before {
		content: "Inactive";
		@apply bg-gray-700;
	}

	.status-tag[data-status="Awaiting"]::before {
		content: "Awaiting";
		@apply bg-yellow-500;
	}

	.status-tag[data-status="Completed"]::before {
		content: "Completed";
		@apply bg-blue-600;
	}
</style>
