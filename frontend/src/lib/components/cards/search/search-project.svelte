<script lang="ts">
	import { page } from "$app/stores";
	import type { components } from "$lib/api/types";
	import Link from "$lib/components/links/link.svelte";
	import { DateTime } from "luxon";
	import { ArchiveBox, Icon, RocketLaunch, ShieldExclamation } from "svelte-hero-icons";

	// HACK: No longer needed in Svelte5 (which is still in alpha)
	export let result: components["schemas"]["SearchResult"];

	let data = result as components["schemas"]["Project"];
	let createdAt = DateTime.fromISO(data.created_at).toISODate();
	let href = `/users/${$page.data.me?.data.id}/projects/${data.slug}`;
</script>

<div class="card">
	<div class="center">
		<Icon src={ArchiveBox} size="42px" solid />
		<div class="details">
			<Link {href}>{data.name}</Link>
			<p>Created at: {createdAt} by <Link href={`/users/${data.owner_id}`}>User</Link></p>
			<span>{data.description}</span>
		</div>
	</div>
	<div class="role">
		<strong>Project</strong>
	</div>
</div>

<style>
	div.card {
		display: flex;
		justify-content: space-between;
		gap: 8px;

		& div.details p {
			font-size: 0.8rem;
			margin: 0;
			color: var(--text-2);
		}

		& div.details span {
			font-size: 0.8rem;
			margin: 0;
			color: var(--text-3);
		}
	}

	div.role {
		background-color: var(--color);
		padding-inline: 8px;
		border-radius: var(--br);
		text-transform: capitalize;
		height: min-content;
	}
</style>
