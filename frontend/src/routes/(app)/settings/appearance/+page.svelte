<script lang="ts">
	import { theme } from "$stores/theme";
	import { Icon, Moon, Sun } from "svelte-hero-icons";
	import Toggle from "$lib/components/buttons/toggle.svelte";
	import CardNotice from "$lib/components/cards/card-notice.svelte";

	let toggled: boolean = $theme.scheme === "dark";
</script>

<svelte:head>
	<title>Appearance</title>
	<meta name="description" content="Appearance settings" />
</svelte:head>

<h1>Appearance Page</h1>
<p>Change the appearance of your profile and theme your experience!</p>
<CardNotice>
	<div>
		<p>Currently light-mode is sort of a WIP feature, it's good enough but the colors need to be refined.</p>
		<p>Dark-mode is the main focus and is the most polished.</p>
	</div>
</CardNotice>
<hr />


<label for="dark-mode-toggle" class="center" style="margin-top: 8px;">
	<Icon src={toggled ? Moon : Sun} size="24px" />
	<Toggle
		id="dark-mode-toggle"
		bind:toggled
		on:click={(e) => {
			$theme.scheme = toggled ? "dark" : "light";
			document.documentElement.setAttribute("data-theme", $theme.scheme);
		}}
	/>
</label>
