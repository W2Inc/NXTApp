<script lang="ts">
	import { encodeID } from "$lib/utils";


interface Props {
	data: BackendTypes["MinimalUserDTO"];
}

const { data }: Props = $props();
</script>

<a href="/users/{encodeID(data?.id ?? "")}" class="flex items-center p-4 rounded-lg border border bg-card text-card-foreground shadow-sm hover:shadow transition-shadow duration-200 cursor-pointer">
	<div class="shrink-0 mr-3">
		{#if data?.avatarUrl}
			<img src={data.avatarUrl} alt="{data.displayName}" class="h-10 w-10 rounded-full object-cover" />
		{:else}
			<div class="h-10 w-10 rounded-full bg-muted flex items-center justify-center text-muted-foreground">
				{data?.login.charAt(0).toUpperCase()}
			</div>
		{/if}
	</div>
	<div class="flex-1 min-w-0">
		<p class="text-sm font-medium text-foreground truncate">{data?.login}</p>
		{#if data?.displayName}
			<p class="text-xs text-muted-foreground truncate">{data.displayName}</p>
		{/if}
	</div>
</a>
