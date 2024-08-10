<script lang="ts">
	import Button from "$lib/components/buttons/button.svelte";
	import Card from "$lib/components/cards/card.svelte";
	import AwaitForm from "$lib/components/misc/await-form.svelte";
	import { Icon, UserGroup, User, ShieldCheck } from "svelte-hero-icons";
	import type { PageData } from "../review/$types";
	import IconTerminal from "$lib/icons/icon-terminal.svelte";

	export let data: PageData;
</script>

<h1>Review Selection Page</h1>
<AwaitForm method="post">
	{#if data.isMyProject}
		<Card>
			<h2 class="center">
				<Icon src={User} size="24px" solid />
				Self Review
			</h2>
			<p>
				Review your own project, the purpose of this is mainly self reflection.
				Did you do what you set out to do? What could you have done better? What did you learn?
			</p>
			<Button type="submit">
				Request
				<Icon src={ShieldCheck} size="18px" solid />
			</Button>
		</Card>
	{/if}

	<Card>
		<h2 class="center">
			<Icon src={UserGroup} size="24px" solid />
			Peer Review
		</h2>
		{#if data.isMyProject}
			<p>
				Request that someone else review your project. This will try to set your project on a queue for receiving a review from anywhere in the world.
			</p>
		{:else}
			<p>This project can be reviewed by you asynchrously, meaning you can review it at your own pace and time.</p>
		{/if}
		<Button type="submit">
			Request
			<Icon src={ShieldCheck} size="18px" solid />
		</Button>
	</Card>

	{#if data.isMyProject}
		<Card>
			<h2 class="center">
				<IconTerminal size={24} />
				Auto Review
			</h2>
			<p>
				Automatically review your project, it is merely to test the project functionality and not judge the in the same way a human would.
				Basically compare the project with a series of provided test cases.
			</p>
			<Button type="submit">
				Request
				<Icon src={ShieldCheck} size="18px" solid />
			</Button>
		</Card>
	{/if}
</AwaitForm>

<style>
	h2 {
		padding: 0;
	}

	p {
		color: var(--text-3);
	}
</style>
