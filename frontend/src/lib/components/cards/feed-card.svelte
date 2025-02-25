<script lang="ts">
	const { data }: { data: BackendTypes["FeedDO"] } = $props();
	const formatter = new Intl.RelativeTimeFormat('en', {
		numeric: 'auto',
		style: 'long'
	});

	function getRelativeTime(dateString: string) {
		const date = new Date(dateString);
		const now = new Date();
		const diffInDays = Math.round((date.getTime() - now.getTime()) / (1000 * 60 * 60 * 24));
		return formatter.format(diffInDays, 'days');
	}
</script>

{#snippet newUser()}
	<div class="rounded-lg shadow p-4 flex items-center space-x-4 border bg-background hover:bg-accent transition-colors">
		<div class="flex-shrink-0">
			<div class="w-12 h-12 rounded-full bg-blue-100 flex items-center justify-center">
				<span class="text-blue-500 text-xl font-bold">
					W2
				</span>
			</div>
		</div>
		<div class="flex-1">
			{#if data.$type === "NewUserFeedDO"}
				<p class="font-medium">
					W2Wizard joined the platform
				</p>
			{:else if data.$type === "CompletedProjectFeedDO"}
				<p class="font-medium">
					W2Wizard completed a project!
				</p>
			{/if}
			<p class="text-muted-foreground text-sm capitalize">
				{getRelativeTime(data.createdAt)}
			</p>
		</div>
	</div>
{/snippet}

{@render newUser()}
