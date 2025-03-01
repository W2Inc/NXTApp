<script lang="ts">
	import Archive from "lucide-svelte/icons/archive";
	import User from "lucide-svelte/icons/user";
	import * as Avatar from "$lib/components/ui/avatar/";
	const { data }: { data: BackendTypes["FeedDO"] } = $props();
	const formatter = new Intl.RelativeTimeFormat("en", {
		numeric: "auto",
		style: "long",
	});

	function getRelativeTime(dateString: string) {
		const date = new Date(dateString);
		const now = new Date();
		const diffInDays = Math.round(
			(date.getTime() - now.getTime()) / (1000 * 60 * 60 * 24),
		);
		return formatter.format(diffInDays, "days");
	}
</script>

{#snippet newUser()}
	<div
		class="bg-background hover:bg-accent flex items-center gap-x-4 rounded-lg border p-4 shadow transition-colors mb-2"
	>
		<div class="flex-shrink-0">
			{#if data.$type === "NewUser"}
				<Avatar.Root>
					<Avatar.Image class="border" src={data.newUser.displayName ?? data.newUser.login} alt="@shadcn" />
					<Avatar.Fallback class="border">CN</Avatar.Fallback>
				</Avatar.Root>
			{:else if data.$type === "CompletedProject"}
				<Archive size={40} class="scale-75" />
			{/if}
		</div>
		<div class="flex-1">
			{#if data.$type === "NewUser"}
				<p class="font-medium">W2Wizard joined the platform</p>
			{:else if data.$type === "CompletedProject"}
				<p class="font-medium">
					{data.newProject.creator?.displayName} completed a project!
				</p>
			{/if}
			<p class="text-muted-foreground text-sm capitalize">
				{getRelativeTime(data.createdAt)}
			</p>
		</div>
	</div>
{/snippet}

{@render newUser()}
