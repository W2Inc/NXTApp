<!-----------------------------------------------------------------------------
 NextDemy , Amsterdam @ 2022.
 See README in the root project for more information.
------------------------------------------------------------------------------>

<!-- Scripting -->
<script lang="ts">
	import {
		Icon,
		ExclamationCircle,
		CheckCircle,
		Cog,
		XCircle,
		ArchiveBox,
		LightBulb,
		type IconSource,
	} from "svelte-hero-icons";

	export let name: string;
	export let status: string | undefined = undefined;
	export let href: string;

	const icons: { [key: string]: IconSource } = {
		available: LightBulb,
		done: CheckCircle,
		fail: XCircle,
		in_progress: Cog,
		not_recommended: ExclamationCircle,
		unavailable: ArchiveBox,
	};
</script>

<a {href}>
	<div class="project-content">
		{#if status}
			{@const icon = icons[status]}
			<Icon src={icon} size="4rem" />
		{:else}
			<Icon src={ArchiveBox} size="4rem" />
		{/if}

		<p>{name}</p>
	</div>

	{#if status}
		<div class="status-tag" data-project-status={status} />
	{/if}
</a>

<style>
	a {
		text-decoration: none;

		color: var(--text-1);
		padding: 10px;

		width: 20rem;
		height: 12rem;

		border-radius: var(--br);
		background-color: var(--shade-03);
		box-shadow: var(--box-shadow);

		transition: transform 0.2s;

		&:hover {
			transform: scale(1.05);
		}
	}

	.project-content {
		height: 93%;
		display: flex;
		justify-content: center;
		flex-direction: column;
		align-items: center;
		/*gap: 1em;*/

		& p {
			display: inline;
			text-overflow: ellipsis;
			overflow: hidden;
			max-width: 75%;

			height: 2rem;
			font-size: 1.42rem;
		}
	}

	.status-tag {
		&::before {
			display: inline-block;
			position: relative;
			border-radius: 0.6em;
			color: white;
			border: 5px var(--shade-03) solid;
			top: 0;
			padding: 1px 10px;
			box-shadow: inherit;
			font-weight: bold;
		}

		&[data-project-status="done"]::before {
			content: "Done";
			color: white;
			background-color: #4caf50;
		}

		&[data-project-status="fail"]::before {
			content: "Fail";
			color: white;
			background-color: #e42b2f;
		}

		&[data-project-status="available"]::before {
			content: "Available";
			color: white;
			background-color: #1c1c1c;
		}

		&[data-project-status="unavailable"]::before {
			content: "Unavailable";
			color: white;
			background-color: #1c1c1c;
		}

		&[data-project-status="in_progress"]::before {
			content: "In Progress";
			color: white;
			background-color: #ff9800;
		}

		&[data-project-status="not_recommended"]::before {
			content: "Not Recommended";
			color: white;
			background-color: #e42b2f;
		}
	}
</style>
