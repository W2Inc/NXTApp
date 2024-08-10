<script lang="ts">
	import type { NumericRange } from "@sveltejs/kit";
	import { BuildingOffice2, CodeBracket, ComputerDesktop, GlobeEuropeAfrica, HandThumbDown, HandThumbUp, Icon } from "svelte-hero-icons";
	import type { HTMLAttributes } from "svelte/elements";

	export let likes: number;
	export let dislikes: number;
	interface $$Props extends HTMLAttributes<HTMLDivElement> {
		likes: number;
		dislikes: number;
	}

	$: total = likes + dislikes;
	$: likePercentage = Math.max((likes / total) * 100, 1);
	$: dislikePercentage = Math.max((dislikes / total) * 100, 1);
	$: dislikeWidth = 100 - likePercentage;
</script>

<div class="meter">
	<ul>
		{#if likes > 0}
			<li title="{likes} Likes" style="min-width: {likePercentage}%">
				<Icon solid src={HandThumbUp} size="14px" />
				<strong>{likes}</strong>
			</li>
		{/if}
		{#if dislikes > 0}
			<li title="{dislikes} Dislikes" style="width: {dislikeWidth}%">
				<Icon solid src={HandThumbDown} size="14px" />
				<strong>{dislikes}</strong>
			</li>
		{/if}
	</ul>
	<div
		role="status"
		class="swatch"
		style="
			--likes: {likePercentage}%;
			--dislikes: {dislikePercentage}%;
		"
		{...$$restProps}
	/>
</div>

<style>
	div.meter {
		margin-bottom: 8px;
	}

	ul {
		margin: 0;
		padding: 0;
		display: flex;
		list-style: none;

		& li {
			cursor: pointer;
			font-size: 0.75em;
			text-align: center;
			display: flex;
			align-items: center;
			justify-content: center;
			gap: 1px;
		}
	}

	div.swatch {
		--height: 6px;
		--like-color: #009432;
		--dislike-color: #ad171c;
		--background-color: var(--shade-04);

		position: relative;
		height: var(--height);
		box-shadow: var(--box-shadow);
		border-radius: var(--br);

		&:hover {
			cursor: pointer;
		}

		& div.tooltip {
			top: calc(var(--height) + 8px);
			position: absolute;
			z-index: 100;
			background-color: var(--shade-02);
			padding: 8px;
			border-radius: var(--br);
			box-shadow: var(--box-shadow);
			border: 1px solid var(--border-color);

			&::before {
				content: "";
				position: absolute;
				top: -16px;
				left: 50%;
				transform: translateX(-50%);
				border: 8px solid transparent;
				border-bottom-color: var(--shade-02);
			}
		}

		&::before {
			content: "";
			width: 100%;
			height: inherit;
			outline: none;
			position: absolute;
			border-radius: inherit;
			background-image: linear-gradient(
				90deg,
				var(--like-color) var(--likes),
				var(--dislike-color) var(--likes)
			);
		}
	}
</style>
