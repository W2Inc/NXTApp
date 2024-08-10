<script lang="ts">
	import type { components } from "$lib/api/types";
	import type { HTMLBaseAttributes } from "svelte/elements";

	/** Do not actually direct the user, basically the href is disabled */
	export let inert: boolean = false;
	export let width: number | string | undefined;
	export let height: number | string | undefined;
	export let avatar_url: string = "https://avatars.githubusercontent.com/u/4510610?v=4";
	export let userID: string;
</script>

<a href="/users/{userID}" {inert}>
	<img src={avatar_url} width={width} height={height} alt="A profile" />
</a>

<style>
	a {
		flex-shrink: 0;
		/* display: flex;
		align-items: center; */

		& img {
			object-fit: cover;
			border-radius: var(--br);
		}
	}
</style>
