<script lang="ts">
	import Github from "lucide-svelte/icons/github";
	import Linkedin from "lucide-svelte/icons/linkedin";
	import Rss from "lucide-svelte/icons/rss";

	const today = new Date();
	const otherLinks = {
		Team: "/team",
		Jobs: "https://www.homerun.co/",
		Press: "/press",
		FAQ: "/faq",
		Contact: "/contact",
	};

	const links = {
		"Privacy Statement": "#",
		"Cookie Statement": "#",
		"Policy Plan": "#",
		"Complaint procedure": "#",
		"Financial Statement": "#",
		"FS - Standard form": "#",
		"NRTO Code of Conduct Consumers": "#",
		"NRTO General Terms and Conditions for Consumers": "#",
	};

	const colors = [
		"hover:text-blue-500",
		"hover:text-indigo-500",
		"hover:text-orange-500",
		"hover:text-purple-500",
		"hover:text-red-500",
		"hover:text-yellow-500",
		"hover:text-pink-500",
		"hover:text-green-500",
		"hover:text-teal-500",
		"hover:text-cyan-500",
	];
</script>

<footer class="bg-muted flex flex-col gap-4 p-8">
	<div class="mb-3 flex items-center justify-between text-sm">
		<span>&copy; {today.getFullYear()} W2 B.V. All rights reserved.</span>
		<nav class="flex items-center gap-2">
			<a href="#" class="hover:text-gray-400">
				<Github size={24} />
			</a>
			<a href="#" class="hover:text-gray-400">
				<Linkedin size={24} />
			</a>
			<a href="/rss.xml" class="hover:text-gray-400">
				<Rss size={24} />
			</a>
		</nav>
	</div>
	<div class="flex gap-x-2 align-middle">
		{#each Object.entries(otherLinks) as [name, url], i}
			{@const color = colors[i % colors.length]}
			{@const isLast = i === Object.entries(otherLinks).length - 1}
			<a href={url} class="font-bold transition-colors hover:underline {color}">
				{name}
			</a>
			{#if !isLast}
				<span aria-hidden="true">/</span>
			{/if}
		{/each}
	</div>
	<div class="mb-4 flex justify-between">
		<ul class="list-none space-y-1 text-sm">
			<li class="font-bold">W2 B.V</li>
			<li>0000GG Amsterdam</li>
			<li>Laan van Nieuwmarkt 14</li>
			<li>KvK 123457382</li>
			<li>NL872288990011</li>
		</ul>
		<div class="flex items-center">
			<a href="##">
				<!-- TODO: Replace with our image -->
				<img src="/favicon.png" alt="" class="h-24 w-24" />
			</a>
		</div>
	</div>
	<ul class="flex flex-wrap items-center gap-x-1 text-sm">
		{#each Object.entries(links) as [name, url], i}
			{@const color = colors[i % colors.length]}
			{@const isLast = i === Object.entries(links).length - 1}
			<li>
				<a href={url} class="transition-colors hover:underline {color}">
					{name}
				</a>
			</li>
			{#if !isLast}
				<span aria-hidden="true">/</span>
			{/if}
		{/each}
	</ul>
</footer>
