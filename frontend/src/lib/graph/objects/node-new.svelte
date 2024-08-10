<script lang="ts">
	import { T } from "@threlte/core";
	import type { Action, Node } from "../builder";
	import { HTML } from "@threlte/extras";
	import Button from "$lib/components/buttons/button.svelte";
	import { Icon, MinusCircle, PlusCircle } from "svelte-hero-icons";
	import { goto } from "$app/navigation";
	import AwaitForm from "$lib/components/misc/await-form.svelte";
	import Select from "$lib/components/input/select.svelte";

	// Params
	export let node: Node<Action>;

	let showSub = false;
	let isLoading = false;
	function onHit() {
		showSub = !showSub;
	}
</script>

<T.Mesh position={[node.position.x, node.position.y, node.position.z]}>
	<HTML center transform sprite>
		<button title="Subscribe to a new goal" on:click={onHit}>
			<Icon src={showSub ? MinusCircle : PlusCircle} size="48px" />
		</button>

		{#if showSub}
			<div class="quick-subcribe">
				<p>Subscribe to a new goal</p>
				<!--<AwaitForm bind:isLoading method="POST" action="/new?/subscribe">-->
				<!--<AwaitForm bind:isLoading method="POST" >-->
					<input type="hidden" name="type" value="goal"/>
					<Select id="join-new-:goal:" name="name">
						<option value="1">C++ Algorithms</option>
						<option value="2">Data Structures</option>
						<option value="3">Object-Oriented Programming</option>
						<option value="4">Web Development</option>
						<option value="5">Machine Learning</option>
						<option value="6">Database Management</option>
						<option value="7">Network Security</option>
						<option value="8">Artificial Intelligence</option>
						<option value="9">Mobile App Development</option>
						<option value="10">Game Development</option>
						<option value="11">Cloud Computing</option>
						<option value="12">UI/UX Design</option>
						<option value="13">Software Testing</option>
						<option value="14">Blockchain Technology</option>
						<option value="15">Internet of Things (IoT)</option>
						<option value="16">Big Data Analytics</option>
						<option value="17">Cybersecurity</option>
						<option value="18">Embedded Systems</option>
						<option value="19">Robotics</option>
						<option value="20">Virtual Reality (VR)</option>
					</Select>
					<hr />
					<Button inert type="submit">
						Subscribe
					</Button>
				<!--</AwaitForm>-->
			</div>
		{/if}
	</HTML>
</T.Mesh>

<style>
	button {
		padding: 8px;
		background-color: var(--shade-01);
		cursor: pointer;
		border-radius: var(--br);
		transition: background-color var(--transition);
		box-shadow: var(--box-shadow);
		border: 1px solid var(--border-color);
		display: inline-flex;
		color: var(--foreground-color);
		user-select: none;
		z-index: -9999999;

		&:disabled {
			opacity: 0.5;
			cursor: not-allowed;
		}

		&:focus-visible {
			outline-offset: var(--outline-offset);
			outline: var(--outline);
		}

		&:hover:not(:disabled) {
			background-color: var(--shade-04);
		}

		&:active:not(:disabled) {
			background-color: var(--shade-03);
		}
	}

	div.quick-subcribe {
		position: absolute;
		top: 0;
		left: 72px;
		padding: 8px;
		width: 300px;
		place-items: center;
		background-color: var(--shade-01);
		border-radius: var(--br);
		transition: background-color var(--transition);
		box-shadow: var(--box-shadow);
		border: 1px solid var(--border-color);

		& p {
			margin: 0;
			padding: 0;
			font-size: 0.8rem;
			font-weight: 600;
			color: var(--text-secondary);
		}
	}
</style>
