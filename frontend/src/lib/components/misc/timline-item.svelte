<script>
	import { Icon, Stop } from "svelte-hero-icons";
</script>

<li>
	<div class="body">
		<slot />
	</div>
</li>

<style>
	li {
		display: flex;
		position: relative;
		padding-bottom: 1rem;
		padding-left: 1rem;
		--timeline-color: var(--border-color);
		color: var(--timeline-color);

		@media screen and (max-width: 700px) {
			padding-left: 0;
			--timeline-color: transparent;
		}

		&::before {
			bottom: 0;
			content: "";
			display: block;
			left: 0;
			position: absolute;
			top: 0;
			width: 2px;
			background-color: var(--timeline-color);
		}

		&::after {
			--size: 12px;

			content: "";
			position: absolute;
			left: calc((var(--size) * -1 / 2) + 1px);
			width: var(--size);
			height: var(--size);
			border-radius: 50%;
			background-color: var(--timeline-color);
		}
	}

	.body {
		flex: 1;
		color: var(--foreground-color);
	}
</style>
