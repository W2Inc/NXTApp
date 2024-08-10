<script lang="ts">
	import type { NumericRange } from "@sveltejs/kit";
	import { BuildingOffice2, CodeBracket, ComputerDesktop, GlobeEuropeAfrica, Icon } from "svelte-hero-icons";
	import type { HTMLAttributes } from "svelte/elements";

	export let self: number = 0;
	export let tester: number = 0;
	export let remote: number = 0;
	export let onsite: number = 0;
	interface $$Props extends HTMLAttributes<HTMLDivElement> {
		self?: number;
		tester?: number;
		remote?: number;
		onsite?: number;
	}

	$: total = self + tester + remote + onsite;
	$: selfPercentage = (self / total) * 100;
	$: testerPercentage = (tester / total) * 100;
	$: remotePercentage = (remote / total) * 100;
	$: onsitePercentage = (onsite / total) * 100;
	$: console.log(selfPercentage);
</script>

<div class="meter">
	<ul>
		{#if self > 0}
			<li title="Self Test" style="width: {selfPercentage}%">
				<!-- <Icon solid src={ComputerDesktop} size="16px" /> -->
				<strong>{selfPercentage}%</strong>
			</li>
		{/if}
		{#if tester > 0}
			<li title="Automated Tests" style="width: {testerPercentage}%">
				<!-- <Icon solid src={CodeBracket} size="16px" /> -->
				<strong>{testerPercentage}%</strong>
			</li>
		{/if}
		{#if remote > 0}
			<li title="Remote Async Tests" style="width: {remotePercentage}%">
				<!-- <Icon solid src={GlobeEuropeAfrica} size="16px" /> -->
				<strong>{remotePercentage}%</strong>
			</li>
		{/if}
		{#if onsite > 0}
			<li title="Onsite Tests" style="width: {onsitePercentage}%">
				<!-- <Icon solid src={BuildingOffice2} size="14px" /> -->
				<strong>{onsitePercentage}%</strong>
			</li>
		{/if}
	</ul>
	<div
		role="status"
		class="swatch"
		style="
			--self: {selfPercentage}%;
			--tester: {testerPercentage}%;
			--remote: {remotePercentage}%;
			--onsite: {onsitePercentage}%;
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
		}
	}

	div.swatch {
		--height: 6px;
		--self-color: #006266;
		--tester-color: #ffc312;
		--remote-color: #ee5a24;
		--onsite-color: #b53471;
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
			--start: calc(var(--self) + var(--tester));
			--end: calc(var(--start) + var(--remote));
			background-image: linear-gradient(
				90deg,
				var(--self-color) 0%,
				var(--self-color) var(--self),
				var(--tester-color) var(--self),
				var(--tester-color) var(--start),
				var(--remote-color) var(--start),
				var(--remote-color) var(--end),
				var(--onsite-color) var(--end),
				var(--onsite-color) calc(var(--end) + var(--onsite)),
				var(--background-color) calc(var(--end) + var(--onsite)),
				var(--background-color) 100%
			);
		}
	}
</style>
