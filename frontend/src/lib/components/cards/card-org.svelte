<script lang="ts">
	import Card from "./card.svelte";
	import Link from "../links/link.svelte";
	import type { HTMLBaseAttributes } from "svelte/elements";
	import {
	CodeBracket,
		Envelope,
		GlobeAlt,
		Icon,
		MapPin,
	} from "svelte-hero-icons";

	export let organization = {
		name: "Codam Coding College",
		email: "contact@codam.nl",
		web: "https://www.codam.nl/",
		map: "https://www.openstreetmap.org/#map=18/52.37453/4.91556",
		image: "https://avatars.githubusercontent.com/u/52664144?s=400&v=4",
		git: "https://github.com/codam-coding-college",
		slug: `codam-coding-college`,
		description: "Codam offers a full-time, 3.5 year Computer Science curriculum for free.",
	};

	interface $$Props extends HTMLBaseAttributes {
		organization?: typeof organization;
	}
</script>

<Card {...$$restProps}>
	<div>
		<div class="center">
			<div class="image" style="background-image: url({organization.image})" />
			<Link href="/organizations/{organization.slug}">{organization.name}</Link>
		</div>
		<ul class="center">
			<li class="center">
				<Link href={organization.map} target="_blank">
					<Icon src={MapPin} solid size="18px" />
				</Link>
			</li>
			<li class="center">
				<Link href={organization.web} target="_blank">
					<Icon src={GlobeAlt} solid size="18px" />
				</Link>
			</li>
			<li class="center">
				<Link href={`mailto:${organization.email}`} target="_blank">
					<Icon src={Envelope} solid size="18px" />
				</Link>
			</li>
			<li class="center">
				<Link href={organization.git} target="_blank">
					<Icon src={CodeBracket} solid size="18px" />
				</Link>
			</li>
		</ul>
	</div>
	<hr />
	<p>{organization.description}</p>
</Card>

<style>
	ul {
		margin-top: 4px;
		align-items: center;
		list-style: none;
		padding: 0;
	}

	div.image {
		height: 32px;
		width: 32px;
		border-radius: var(--br);
		background-repeat: no-repeat;
		background-size: cover;
		background-color: var(--shade-01);
		background-position: center;
		border-top-left-radius: var(--br);
		border-top-right-radius: var(--br);
	}
</style>
