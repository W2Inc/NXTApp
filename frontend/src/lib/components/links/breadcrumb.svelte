<script lang="ts">
	import toast from "sonner-svelte";
	import { afterNavigate } from "$app/navigation";
	import { ChevronRight, Home, Icon } from "svelte-hero-icons";

	const isSlugRoute = /^\[.*\]$/;
	const isGroupRoute = /^\(.*\)$/;
	let crumbs: Array<{
		label: string;
		href: string;
	} | null> = [];

	// TODO: Avoid this shit when not displayed!
	afterNavigate((nav) => {
		const target = nav.to;
		if (!target || !target.route.id) {
			crumbs = [{ label: "Unknown", href: "" }];
			return;
		}

		let path = "";
		crumbs = target.route.id
			.split("/")
			.filter((t) => !isGroupRoute.test(t) && t !== "")
			.map((t) => {
				if (target.params && isSlugRoute.test(t)) {
					const slug = t.replaceAll(/[\[\]]/g, "").slice(0, t.indexOf("=") - 1);
					const value = target.params[slug] ?? "BUG!";
					path += `/${value}`;

					const tag = document.querySelector(`meta[name="${t}"]`) as HTMLMetaElement;
					if (!tag) {
						toast.error(`[BUG]: No meta tag found for ${t}, please report!`);
					}

					return {
						label: tag?.content ?? value,
						href: path,
					};
				}
				path += `/${t}`;
				return {
					label: `${t.charAt(0).toUpperCase()}${t.slice(1)}`,
					href: path,
				};
			});
		crumbs.unshift({ label: "Home", href: "/" });
	});
</script>

<div class="breadcrumb">
	{#each crumbs as c, i}
		{#if i !== crumbs.length && i !== 0}
			<Icon src={ChevronRight} size="18" solid />
		{/if}

		{#if c !== null}
			{#if c.label === "Home"}
				<Icon src={Home} size="18" solid />
			{/if}
			{#if i === crumbs.length - 1}
				<span class="label">{c.label}</span>
			{:else}
				<a class="label" href={c.href}>{c.label}</a>
			{/if}
		{:else}
			<span class="label">Unknown</span>
		{/if}
	{/each}
</div>

<style>
	.breadcrumb {
		display: flex;
		align-items: center;
		gap: 4px;

		& a {
			display: inline-block;
			color: var(--primary) !important;
		}

		& .label {
			color: var(--text-2);
			max-width: 96px;
			overflow-x: hidden;
			text-wrap: nowrap;
			text-overflow: ellipsis;
		}

		@media only screen and (max-width: 768px) {
			display: none;
		}
	}
</style>
