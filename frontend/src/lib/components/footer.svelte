<script lang="ts">
	import Card from "./cards/card.svelte";
	import Link from "./links/link.svelte";
</script>

<Card>
	<footer>
		<div class="content">
			<p>Copyright © 2024 Nextdemy B.V. All rights reserved.</p>
			<ul>
				<li><Link href="mailto:oscar@nextdemy.com">Contact</Link></li>
			</ul>
		</div>
	</footer>
</Card>

<style>
	div.content {
		display: flex;
		justify-content: space-between;
		align-items: center;
	}

	ul {
		display: flex;
		list-style: none;
		padding: 0;
		margin: 0;
		gap: 0.5rem;
		flex-wrap: wrap;

		& li {
			padding-right: 0.5rem;
			border-right: 1px solid var(--border-color);

			&:last-child {
				border-right: none;
			}
		}
	}
</style>
