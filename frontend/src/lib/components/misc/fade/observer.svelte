<script lang="ts">
	import {onMount } from "svelte";

	onMount(() => {
		const observer = new IntersectionObserver((entries) => {
			entries.forEach((entry) => {
				if (entry.isIntersecting) {
					entry.target.classList.add("visible");
				}
			});
		});

		document.querySelectorAll(".hidden").forEach((element) => {
			observer.observe(element);
		});
	});
</script>
