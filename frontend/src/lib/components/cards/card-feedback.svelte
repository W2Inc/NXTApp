<script lang="ts">
	import Profileimg from "../links/profileimg.svelte";
	import type { components } from "$lib/api/types";
	import { DateTime } from "luxon";
	import { ArrowRight, Icon } from "svelte-hero-icons";

	export let review: components["schemas"]["Review"];

	let reportedAt = DateTime.fromISO(review.created_at);
	let diff = DateTime.now().diff(reportedAt, ["days", "hours"]).toHuman({
		maximumFractionDigits: 0,
	});
</script>

<a href="/feedback/{review.id}" class="header center">
	<Profileimg userID={review.reviewer_id} width="40" height="40" />
	<div class="center-content">
		<div class="details">
			<!-- TODO: Have this in the schema -->
			<h4>{review.reviewer.display_name}</h4>
			<span class="medium">commented</span>
		</div>
		<span class="small">{diff} ago</span>
	</div>
	<div class="center icon">
		<Icon src={ArrowRight} size="16px" solid />
	</div>
</a>

<style>
	a.header.center {
		color: unset;
		text-decoration: none;
		border: 1px solid var(--border-color);
		padding: var(--padding);
		border-radius: var(--br);
		background-color: var(--shade-02);
		display: flex;

		& .icon {
			translate: 0;
			transition: translate var(--transition);
		}

		&:hover {
			& .icon {
				translate: 4px;
			}
		}
	}

	.header .center-content {
		flex: 1 1 auto;
		line-height: 1.15em;

		& h4 {
			padding: unset;
		}

		& span {
			color: var(--text-3);

			&.medium {
				font-size: 0.85em;
			}

			&.small {
				font-size: 0.7em;
			}
		}

		& .details {
			display: flex;
			align-items: center;
			gap: 4px;
		}
	}
</style>
