<li>
	<button>
		<slot/>
	</button>
</li>

<style>
	button {
		background-color: var(--shade-01);
		display: flex;
    align-items: center;
    gap: 4px;
    width: 100%;
    padding: 4px;
    border-radius: var(--br);
    color: var(--text-01);
    margin-bottom: 4px;
    text-decoration: none;
		border: none;
		cursor: pointer;

		animation: fadeIn 0.2s ease;
	}

	button:hover {
		background-color: var(--shade-02);
	}

	button:focus {
		background-color: var(--shade-03);
	}

	button:disabled {
		opacity: 0.5;
		cursor: not-allowed;
	}
</style>
