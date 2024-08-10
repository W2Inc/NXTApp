<script lang="ts">
	import {
		Check,
		ChevronDown,
		Icon,
		type IconSource,
		Plus,
		BuildingOffice2,
	} from "svelte-hero-icons";
	import Card from "../cards/card.svelte";
	import Dropdown from "../input/dropdown.svelte";
	import Profileimg from "../links/profileimg.svelte";
	import type { components } from "$lib/api/types";

	export let user: components["schemas"]["User"];

	let contexts = [
		{
			name: "Dashboard",
			current: true,
			image: "https://avatars.githubusercontent.com/u/0?v=4",
			href: `/users/${user.id}`,
		},
		{
			name: "Codam Coding College",
			current: false,
			image: "https://avatars.githubusercontent.com/u/52664144?s=40&v=4",
			href: `/organizations/codam-coding-college`,
		},
	];

	let NullIcon: IconSource = {};
</script>

<Dropdown toggle role="menu">
	<Card aria-haspopup="menu" role="button" slot="summary" class="center">
		<Profileimg inert {user} width="20px" height="20px" />
		<span style="flex: 1;">Personal Dashboard</span>
		<Icon src={ChevronDown} solid size="18px" />
	</Card>

	<menu>
		{#each contexts as context}
			<li>
				<a href={context.href}>
					<Icon src={context.current ? Check : NullIcon} solid size="18px" />
					<img src={context.image} alt={context.name} width="20px" height="20px" />
					<span style="flex: 1;">{context.name}</span>
				</a>
			</li>
		{/each}

		<li>
			<a href="/organizations">
				<Icon src={Plus} solid size="18px" />
				<Icon src={BuildingOffice2} solid size="18px" />
				Join an Organization
			</a>
		</li>
	</menu>
</Dropdown>

<style>

	menu {
		box-shadow: var(--box-shadow);
		border-radius: var(--br);
		border: 1px solid var(--border-color);
		background-color: var(--shade-01);
	}

	li {
		list-style: none;
		border-top: 1px solid var(--border-color);
		padding: 8px 16px;

		&:hover {
			background-color: var(--shade-02);
		}

		&:first-child {
			border-top: none;
			border-top-right-radius: var(--br);
			border-top-left-radius: var(--br);
		}

		&:last-child {
			padding: 8px 16px;
			border-bottom-right-radius: var(--br);
			border-bottom-left-radius: var(--br);
		}
	}

	a {
		display: flex;
		align-items: center;
		gap: 8px;
		color: var(--text-color);
		text-decoration: none;
	}
</style>
