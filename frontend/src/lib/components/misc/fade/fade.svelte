<script lang="ts">
	export let direction: "left" | "right" | "top" | "bottom" = "left";

</script>

<div class="hidden {direction}">
	<slot />
</div>

<style>
	div {
		overflow: hidden;
		position: relative;
		z-index: 9999999999999;
	}

	:global(.visible) {
		opacity: 1 !important;
		translate: 0% !important;
		filter: blur(0px) !important;
	}

	:global(.hidden) {
		opacity: 0;
		filter: blur(8px);
		--push-strength: 15%;

		&.right {
			translate: var(--push-strength);
		}

		&.left {
			translate: calc(var(--push-strength) * -1);
		}

		&.bottom {
			translate: 0% var(--push-strength);
		}

		&.top {
			translate: 0% calc(var(--push-strength) * -1);
		}

		@media (prefers-reduced-motion: no-preference) {
			transition: all 1s;
		}
	}
</style>
