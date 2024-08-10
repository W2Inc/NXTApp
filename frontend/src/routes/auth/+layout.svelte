<main>
	<slot />
</main>

<style>
	:global(body) {
		height: 100dvh;
		width: 100%;
	}

	main {
		display: grid;
		place-items: center;
		height: 100%;
	}
</style>
