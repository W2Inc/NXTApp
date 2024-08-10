<script lang="ts">
	import { Icon, XMark } from "svelte-hero-icons";
	import Card from "./card.svelte";
	import Button from "../buttons/button.svelte";
	import { enhance } from "$app/forms";

	/** A URL for a background image to be used in the card*/
	export let BGUrl: string = "";
	export let title: string | undefined = undefined;

	let parent: HTMLDivElement;
</script>

<div bind:this={parent}>
	<Card style="background-image: url({BGUrl});">
		<form
			use:enhance
			method="post"
			action="?/noNotification"
			on:submit={() => parent.remove()}
		>
			<input type="hidden" name="id" value={123} />
			<div class="close">
				<button type="submit">
					<Icon src={XMark} size="24px" solid />
				</button>
			</div>

			<!-- TODO: Check API on how to make this convenient -->
			<!-- Use markdown perhaps for content ? -->
			<h1>{title}</h1>
			<hr />
			<p>Are you ready to learn?</p>
			<p>Join our hackathon on the 20th of June!</p>
			<Button
				style="width: 100%;"
				href="https://www.youtube.com/watch?v=xvFZjo5PgG0"
			>
				Join our Waitlist!
			</Button>
		</form>
	</Card>
</div>

<style>
	form {
		position: relative;
	}

	button {
		display: flex;
		align-items: center;
		justify-content: center;
		position: absolute;
		width: 24px;
		height: 24px;
		top: 4px;
		right: 4px;
		background: transparent;
		border: none;
		cursor: pointer;
		border-radius: var(--br);
		padding: 2px;
		transition: background var(--transition);

		&:hover {
			background: var(--shade-01);
		}
	}
</style>
