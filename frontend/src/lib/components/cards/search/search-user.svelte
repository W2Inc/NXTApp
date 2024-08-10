<script lang="ts">
	import type { components } from "$lib/api/types";
	import Link from "$lib/components/links/link.svelte";
	import Profileimg from "$lib/components/links/profileimg.svelte";
	import { DateTime } from "luxon";

	// HACK: No longer needed in Svelte5 (which is still in alpha)
	export let result: components["schemas"]["SearchResult"] | components["schemas"]["User"];
	let data = result as components["schemas"]["User"];
	let joinedAt = DateTime.fromISO(data.created_at);
</script>

<div class="card">
		<div class="center">
			<Profileimg user={data} height="42px" width="42px" />
			<div class="details">
				<Link href={`/users/${data.id}`}>{data.display_name}</Link>
				<p>Joined on: {joinedAt.toISODate()}</p>
			</div>
		</div>
		<div class="role">
			<strong>{data.role}</strong>
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
	}

	div.role {
		background-color: var(--color);
		padding-inline: 8px;
		border-radius: var(--br);
		text-transform: capitalize;
		height: min-content;
	}
</style>
