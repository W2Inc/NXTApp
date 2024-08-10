<script lang="ts">
	import Logo from "$assets/logo.png";
	import Imglink from "../links/imglink.svelte";
	import Dropdown from "../input/dropdown.svelte";
	import Button from "../buttons/button.svelte";
	import Search from "../input/search.svelte";
	import CardNavigation from "../cards/card-navigation.svelte";
	import {
		ArchiveBox,
		Bars3,
		ChatBubbleLeft,
		CodeBracket,
		CurrencyDollar,
		GlobeAlt,
		Icon,
		LightBulb,
		Sparkles,
		Users,
	} from "svelte-hero-icons";
	import Modal from "../dialogs/modal.svelte";
	import Accordion from "../misc/accordion.svelte";
	import { dev } from "$app/environment";

	let navmodal: Modal;
</script>

<!-- Mobile -->
<Modal title="Navigation" bind:this={navmodal}>
	<div class="search mobile">
		<Search />
	</div>
	<nav>
		<Accordion title="Explore">
			<div style="margin-top: 8px;" class="explore-navigation">
				<CardNavigation
					title="Recent Projects"
					icon={ArchiveBox}
					description="Discover the latest projects"
					href="/recent/projects"
				/>
				<CardNavigation
					title="Recent Reviews"
					icon={ChatBubbleLeft}
					description="Read and find the latest reviews"
					href="/recent/reviews"
				/>
				<CardNavigation
					title="The Graph"
					icon={Sparkles}
					description="Discover what's trending"
					href="/"
				/>
				<CardNavigation
					title="Github"
					icon={CodeBracket}
					description="Follow us on Github and contribute"
					href="https://github.com/Nextdemy"
				/>
			</div>
		</Accordion>
		<hr />
		<Accordion title="About Us">
			<div style="margin-top: 8px;" class="explore-navigation">
				<CardNavigation
					title="The Methodology"
					icon={LightBulb}
					description="Learn about our peer-to-peer method"
					href="/explore"
				/>
				<!--<CardNavigation
					title="Our Network"
					icon={GlobeAlt}
					description="Connect, communicate and collaborate"
					href="/network"
				/>-->
				<CardNavigation
					title="The Team"
					icon={Users}
					description="Discover the team, values and our mission"
					href="/team"
				/>
			</div>
		</Accordion>
		<hr />
		<Accordion title="Pricing">
			<div style="margin-top: 8px;" class="explore-navigation">
				<CardNavigation
					title="Pricing"
					icon={CurrencyDollar}
					description="Interested? Check out our pricing"
					href="/pricing"
				/>
			</div>
		</Accordion>
	</nav>
</Modal>

<!-- Desktop -->
<header>
	<div class="start">
		<Imglink href="/" src={Logo} alt="Codam logo" width={64} />
		<ul dir="ltr">
			<!-- Explore -->
			<li>
				<Dropdown>
					<span slot="text">Explore</span>
					<div class="explore-navigation">
						<CardNavigation
							title="Recent Projects"
							icon={ArchiveBox}
							description="Discover the latest projects"
							href="/recent/projects"
						/>
						<CardNavigation
							title="Recent Reviews"
							icon={ChatBubbleLeft}
							description="Read and find the latest reviews"
							href="/recent/reviews"
						/>
						<CardNavigation
							title="The Graph"
							icon={Sparkles}
							description="Discover what's trending"
							href="/"
						/>
						<CardNavigation
							title="Github"
							icon={CodeBracket}
							description="Follow us on Github and contribute"
							href="https://github.com/Nextdemy"
						/>
					</div>
				</Dropdown>
			</li>
			<!-- Company -->
			<li>
				<Dropdown>
					<span slot="text">Company</span>
					<div class="explore-navigation">
						<CardNavigation
							title="The Methodology"
							icon={LightBulb}
							description="Learn about our peer-to-peer method"
							href="/explore"
						/>
						<!--<CardNavigation
							title="Our Network"
							icon={GlobeAlt}
							description="Connect, communicate and collaborate"
							href="/network"
						/>-->
						<CardNavigation
							title="The Team"
							icon={Users}
							description="Discover the team, values and our mission"
							href="/team"
						/>
					</div>
				</Dropdown>
			</li>
			<!-- Pricing -->
			<li>
				<a href="/pricing">Pricing</a>
			</li>
		</ul>
		<div class="mobile-nav">
			<Button on:click={navmodal.toggle}>
				<Icon src={Bars3} solid size="18px" />
			</Button>
		</div>
	</div>

	<div class="end">
		<div class="search">
			<Search />
		</div>
		<Button href="/auth/login">Sign in</Button>
	</div>
</header>

<style>
	header {
		position: relative;
		backdrop-filter: blur(10px);
		background-color: var(--nav-bg-color);
		width: 100%;
		display: flex;
		align-items: center;
		justify-content: space-between;
		padding: var(--padding);
		border-bottom: 1px solid var(--border-color);
		box-shadow: var(--box-shadow);
		z-index: 9999;
	}

	a {
		color: var(--text-color);
		text-decoration: none;
	}

	ul {
		display: flex;
		align-items: center;
		gap: 1rem;
		padding-inline-start: 0;
		list-style: none;
	}

	div.explore-navigation {
		border-radius: var(--br);
		box-shadow: var(--box-shadow);
		background-color: var(--shade-01);
		padding: 4px;
		border: 1px solid var(--border-color);
		border-radius: var(--br);

		display: grid;
		gap: 4px;
		grid-template-columns: 1fr 1fr;

		@media screen and (max-width: 929px) {
			display: flex;
			flex-direction: column;
		}
	}

	.start {
		display: flex;
		align-items: center;
		gap: 1rem;

		& .mobile-nav {
			display: none;
		}

		@media screen and (max-width: 929px) {
			& ul {
				display: none;
			}

			& .mobile-nav {
				display: flex;
			}
		}
	}

	.search {
		display: flex;

		&.mobile {
			display: none;
			margin-bottom: 8px;
		}

		@media only screen and (max-width: 495px) {
			display: none;

			&.mobile {
				display: flex;
			}
		}
	}

	.end {
		display: flex;
		align-items: center;
		gap: 10px;
	}
</style>
