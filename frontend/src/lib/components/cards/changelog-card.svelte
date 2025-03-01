<script lang="ts">
	import ArrowRight from "lucide-svelte/icons/arrow-right";
	export let changelog = [
		{
			title: "New Feature: Dashboard Analytics",
			date: "2023-10-15",
			description:
				"Added advanced analytics dashboard with real-time data visualization.",
			link: "/changelog/dashboard-analytics",
		},
		{
			title: "Bug Fixes and Performance Improvements",
			date: "2023-09-30",
			description: "Fixed several UI issues and improved loading times by 30%.",
			link: "/changelog/bug-fixes-september",
		},
		{
			title: "Mobile App Launch",
			date: "2023-09-01",
			description: "Launched our mobile app for iOS and Android platforms.",
			link: "/changelog/mobile-launch",
		},
	];
</script>

<div
	class="bg-background mb-2 w-full min-w-[346px] max-w-[400px] rounded-lg border p-4 shadow"
>
	<h2 class="mb-4 flex items-center justify-between text-xl font-bold">
		<span>Changelog</span>
		<a href="#" class="center-content text-muted-foreground text-sm hover:underline">
			View More
			<ArrowRight size={14} />
		</a>
	</h2>

	<div class="space-y-2">
		{#each changelog as item, i}
			<div class="flex">
				<!-- Timeline element with dot and connecting line -->
				<div class="relative mr-2 flex flex-col items-center">
					<div class="bg-primary z-10 h-3 w-3 rounded-full"></div>
					{#if i < Math.min(changelog.length - 1, 3)}
						<div class="bg-primary absolute bottom-0 top-3 h-full w-0.5"></div>
					{/if}
				</div>

				<!-- Content -->
				<div class="flex-1 pb-4">
					<a href={item.link} class="group block">
						<div
							class="bg-muted group-hover:border-primary rounded-lg border p-3 transition-all duration-200 group-hover:shadow"
						>
							<div
								class="mb-1 flex flex-col sm:flex-row sm:items-start sm:justify-between"
							>
								<h3 class="group-hover:text-primary text-sm font-semibold">
									{item.title}
								</h3>
							</div>
							<p class="text-muted-foreground text-xs">{item.description}</p>
							<div class="flex justify-between">
								<div class="text-primary mt-1 text-xs font-medium group-hover:underline">
									Read more
								</div>
								<span class="text-muted-foreground mt-1 text-xs sm:mt-0">{item.date}</span
								>
							</div>
						</div>
					</a>
				</div>
			</div>
		{/each}
	</div>
</div>
