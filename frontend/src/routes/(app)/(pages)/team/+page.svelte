<script>
	import Content from "$assets/markdown/terms.md?raw";
	import CardNotice from "$lib/components/cards/card-notice.svelte";
	import Card from "$lib/components/cards/card.svelte";
	import Link from "$lib/components/links/link.svelte";
</script>

<!-- https://i.imgur.com/UeDNBNQ.jpeg -->

<div class="body">
	<h1>The Team</h1>

	<div class="container">
		<div class="card">
			<div class="image" style="background-image: url(https://avatars.githubusercontent.com/u/32016388?v=4?s=400)"/>
			<div class="content">
				<Link href="https://github.com/OscarMulder">OscarMulder</Link>
				<p>CEO</p>
			</div>
		</div>

		<div class="card">
			<div class="image" style="background-image: url(https://avatars.githubusercontent.com/u/7946761?v=4?s=400)"/>
			<div class="content">
				<Link href="https://github.com/Neph4lim">Neph4lim</Link>
				<p>CTO</p>
			</div>
		</div>

		<div class="card">
			<div class="image" style="background-image: url(https://avatars.githubusercontent.com/u/63303990?v=4?s=400)"/>
			<div class="content">
				<Link href="https://github.com/W2Wizard">W2Wizard</Link>
				<p>Co-Founder & Engineer</p>
			</div>
		</div>
	</div>
</div>

<style>
	div.body {
		margin: 0 auto;
		max-width: var(--max-content-width);
		padding: 3rem 1.5rem;
	}

	div.container {
		display: grid;
		justify-items: center;
		grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
		gap: 1rem;
	}

	div.card {
		min-width: 200px;
		max-width: 200px;
		display: flex;
		flex-direction: column;
		border: 1px solid var(--border-color);
		border-radius: var(--br);


		& div.content {
			flex: 1;
			padding: 1rem;
			border-bottom-left-radius: var(--br);
			border-bottom-right-radius: var(--br);
			background-color: var(--shade-01);
			color: var(--foreground-color);
			text-align: center;
			border-top: 1px solid var(--border-color);

			& a {
				font-weight: bold;
				color: var(--primary);
			}
		}

		& div.image {
			height: 200px;
			width: 100%;
			background-repeat: no-repeat;
			background-size: cover;
			background-color: black;
			border-top-left-radius: var(--br);
			border-top-right-radius: var(--br);
		}
	}

</style>
