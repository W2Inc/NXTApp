<div>
	<slot />
</div>

<style>
	div {
		padding: 16px;
		gap: 1rem;
		margin-inline: auto;
		padding-inline: 24px;
		max-width: var(--max-content-width);
		display: flex;
		flex-direction: column;

		& div.submit {
			width: 100%;
			display: flex;

			& > * {
				flex: 1;
				width: 100%;
			}
		}
	}
</style>
