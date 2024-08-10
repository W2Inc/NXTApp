<script lang="ts">
	import Card from "./card.svelte";
	import type { components } from "$lib/api/types";
	import Profileimg from "../links/profileimg.svelte";
	import { PencilSquare, Icon, EllipsisHorizontal, Trash } from "svelte-hero-icons";
	import { DateTime } from "luxon";
	import Button from "../buttons/button.svelte";
	import { enhance } from "$app/forms";
	import Dropdown from "../input/dropdown.svelte";
	import MarkdownViewer from "../markdown/markdown-viewer.svelte";
	import AwaitForm from "../misc/await-form.svelte";

	interface CommentProp {
		review_id: string;
		created_at: string;
		updated_at: string;
		comment: string;
		user: {
			id: string;
			avatar_url: string;
			display_name: string;
		};
	}

	export let editable: boolean = false;
	export let data: CommentProp;

	let reportedAt = DateTime.fromISO(data.created_at);
	let diff = DateTime.now().diff(reportedAt, ["days", "hours"]).toHuman({
		maximumFractionDigits: 0,
	});
</script>

<Card>
	<div class="header center">
		<Profileimg userID={data.user.id} width="40" height="40" />
		<div class="center-content">
			<div class="details">
				<h4>{data.user.display_name}</h4>
				<span class="medium">commented</span>
			</div>
			<span class="small">{diff} ago</span>
		</div>

		<!-- Settings -->
		{#if editable}
			<Dropdown toggle>
				<svelte:fragment slot="summary">
					<Icon src={EllipsisHorizontal} size="16px" solid />
				</svelte:fragment>

				<Card style="padding: 4px;">
					<AwaitForm method="post" action="">
						<Button style="border: none; box-shadow: none;" title="Edit your feedback">
							<Icon src={PencilSquare} size="16px" solid />
						</Button>

						<Button style="border: none; box-shadow: none;" title="Report this feedback">
							<Icon src={Trash} size="16px" solid />
						</Button>
					</AwaitForm>
				</Card>
			</Dropdown>
		{/if}
	</div>
	<hr />
	<div class="body">
		<MarkdownViewer md={data.comment} />
	</div>
</Card>

<style>
	form {
		display: flex;
		gap: 2px;
		flex-wrap: wrap;
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
