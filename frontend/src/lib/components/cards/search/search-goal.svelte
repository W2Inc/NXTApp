<script lang="ts">
	import type { components } from "$lib/api/types";
	import Link from "$lib/components/links/link.svelte";
	import { DateTime } from "luxon";
	import { Icon, Trophy } from "svelte-hero-icons";

	// HACK: No longer needed in Svelte5 (which is still in alpha)
	export let result: components["schemas"]["SearchResult"];
	let data = result as components["schemas"]["LearningGoals"];
	let createdAt = DateTime.fromISO(data.created_at).toISODate();
	let href = `/projects/${data.id}`;
</script>

<div class="card">
	<div class="center">
		<Icon src={Trophy} size="42px" solid />
		<div class="details">
			<Link {href}>{data.name}</Link>
			<p>Created at: {createdAt}</p>
		</div>
	</div>
</div>

<style>
	div.card {
		& div.details p {
			font-size: 0.8rem;
			margin: 0;
			color: var(--text-2);
		}
	}
</style>

