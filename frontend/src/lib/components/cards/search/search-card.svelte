<script lang="ts">
	import type { components } from "$lib/api/types";
	import GoalSearch from "./search-goal.svelte";
	import ProjectSearch from "./search-project.svelte";
	import UserSearch from "./search-user.svelte";
	// import OrgSearch from "./search-organization.svelte";

	export let type: components["schemas"]["SearchResult"]["type"] | "Unknown";
	export let data: components["schemas"]["SearchResult"];
</script>

<div data-search-type={type.toLowerCase()}>
	{#if type == "LearningGoals"}
		<GoalSearch result={data} />
	<!-- {:else if type == "Organization"}
		<OrgSearch result={data} /> -->
	{:else if type == "Project"}
		<ProjectSearch result={data} />
	{:else if type == "User"}
		<UserSearch	result={data} />
	{:else if type == "Unknown"}
		<p>Error: Unknown type</p>
	{/if}
</div>

<style>
	div {
		--user-color: hsla(200, 80%, 50%, 1);
		--project-color: hsla(40, 80%, 50%, 1);
		--org-color: hsla(0, 80%, 50%, 1);
		--goal-color: hsla(120, 80%, 50%, 1);

		padding: 16px;
		border-radius: var(--br);
		box-shadow: var(--box-shadow);
		border: 1px solid var(--border-color);
		background-color: var(--shade-01);
		font-size: 14px;
		word-wrap: break-word;

		&[data-search-type="user"] {
			--color: var(--user-color);
			border-left: 4px solid var(--color);
		}

		&[data-search-type="project"] {
			--color: var(--project-color);
			border-left: 4px solid var(--color);
		}

		&[data-search-type="organization"] {
			--color: var(--org-color);
			border-left: 4px solid var(--color);
		}

		&[data-search-type="learninggoals"] {
			--color: var(--goal-color);
			border-left: 4px solid var(--color);
		}
	}
</style>
