<script lang="ts">
	import Card from "./card.svelte";
	import Button from "../buttons/button.svelte";
	import Toggle from "../buttons/toggle.svelte";
	import { Icon, Megaphone } from "svelte-hero-icons";
	import AwaitForm from "../misc/await-form.svelte";
	import type { components } from "$lib/api/types";
	import MarkdownViewer from "../markdown/markdown-viewer.svelte";

	export let enabled: boolean = false;
	export let isLoading: boolean = false;
	export let feature: components["schemas"]["Features"];
</script>

<Card style="margin-bottom: 16px;">
	<div class="header">
		<h2>{feature.name}</h2>
		<div class="options">
			<AwaitForm bind:isLoading method="post" action="?/toggleFeature">
				<input type="hidden" readonly name="id" value={feature.id} />
				<input type="hidden" readonly name="enabled" value={enabled} />
				<Toggle
					type="submit"
					disabled={isLoading}
					title="Enable or disable this feature"
					bind:toggled={enabled}
				/>
			</AwaitForm>

			<Button href="https://github.com/orgs/Nextdemy/discussions">
				<span>Feedback</span>
				<Icon src={Megaphone} size="18px" />
			</Button>
		</div>
	</div>
	<hr />
	<div class="body">
		<MarkdownViewer md={feature.description} />
	</div>
</Card>

<style>
	.header {
		display: flex;
		justify-content: space-between;
		align-items: center;

		& .options {
			display: flex;
			align-items: center;
			gap: 16px;
		}
	}

	.body {
		& img {
			max-width: 10px;
		}
	}
</style>
