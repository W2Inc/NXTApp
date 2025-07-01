<script>
	import { enhance } from "$app/forms";
	import Button from "$lib/components/ui/button/button.svelte";
	import {
		Accessibility,
		ArrowRight,
		FlaskConical,
		Github,
		LogIn,
		Rss,
		Twitter,
		Users,
		Code,
		GitBranch,
		GanttChart,
		Orbit,
	} from "lucide-svelte";
	import { fade, fly, scale } from "svelte/transition";

	// State management with Svelte 5's $state
	let isLoaded = $state(false);
	let visibleSections = $state({
		hero: false,
		features: false,
		stats: false,
		community: false,
	});

	// Intersection observer for scroll animations
	$effect(() => {
		// Show hero section immediately after a brief delay
		setTimeout(() => {
			visibleSections.hero = true;
			isLoaded = true;
		}, 300);

		// Setup intersection observers for scroll animations
		const sections = ["features", "stats", "community"];
		const observer = new IntersectionObserver(
			(entries) => {
				entries.forEach((entry) => {
					if (entry.isIntersecting) {
						const sectionId = entry.target.id;
						visibleSections[sectionId] = true;
						observer.unobserve(entry.target);
					}
				});
			},
			{ threshold: 0.2 },
		);

		sections.forEach((section) => {
			const element = document.getElementById(section);
			if (element) observer.observe(element);
		});
	});

	// Features data
	const features = [
		{
			icon: "🚀",
			title: "Project-Based Learning",
			description:
				"Engage with hands-on projects that build real-world skills and deepen your understanding through practical application.",
		},
		{
			icon: "👥",
			title: "Peer-to-Peer Reviews",
			description:
				"Get your projects reviewed by peers remotely, in-person, or asynchronously to receive valuable feedback and diverse perspectives.",
		},
		{
			icon: "🌌",
			title: "3D Learning Paths",
			description:
				"Visualize your educational journey through an interactive 3D galaxy that represents your unique learning path and progress.",
		},
		{
			icon: "🔄",
			title: "Git Integration",
			description:
				"Track project progress with built-in Git source control, making version management and collaboration seamless.",
		},
	];

	// Stats data
	const stats = [
		{ value: "500+", label: "Learning Projects" },
		{ value: "50K+", label: "Active Learners" },
		{ value: "100+", label: "Learning Paths" },
		{ value: "12K+", label: "Community Reviews" },
	];
</script>

<div class="overflow-hidden" style="filter: url(#pixelate-20); will-change: filter;">
	<section
		class="bg-background dark:bg-background relative flex items-center overflow-hidden pb-24 pt-32 md:min-h-[calc(100vh-80px)]"
	>
		<div class="absolute inset-0 overflow-hidden">
			<div
				class="bg-primary/10 dark:bg-primary/5 absolute -right-40 -top-40 h-96 w-96 rounded-full blur-3xl"
			></div>
			<div
				class="bg-accent/10 dark:bg-accent/5 absolute -left-20 top-1/3 h-72 w-72 rounded-full blur-3xl"
			></div>
			<div
				class="bg-secondary/10 dark:bg-secondary/5 absolute bottom-20 right-10 h-60 w-60 rounded-full blur-3xl"
			></div>
		</div>

		<!-- Animated decorative elements -->
		<div class="animate-float-slow absolute right-10 top-20 hidden md:block">
			<div
				class="from-primary/20 to-primary/10 dark:from-primary/10 dark:to-primary/5 h-24 w-24 rotate-12 rounded-xl bg-gradient-to-br backdrop-blur-sm"
			></div>
		</div>
		<div class="animate-float-slow absolute bottom-20 left-10 hidden md:block">
			<div
				class="from-primary/20 to-primary/10 dark:from-primary/10 dark:to-primary/5 h-16 w-16 -rotate-12 rounded-lg bg-gradient-to-br backdrop-blur-sm"
			></div>
		</div>

		<div class="container relative z-10 mx-auto px-4 md:px-6">
			<div class="grid items-center gap-12 md:grid-cols-2 md:gap-8">
				{#if visibleSections.hero}
					<div class="flex flex-col space-y-8">
						<div in:fly={{ y: 50, delay: 200, duration: 700 }}>
							<span
								class="bg-primary/10 text-primary dark:bg-primary/20 dark:text-primary-foreground inline-block rounded-full px-4 py-1.5 text-sm font-medium"
							>
								Self-hostable & Open Source
							</span>
						</div>

						<h1
							class="text-foreground text-4xl font-bold leading-tight md:text-5xl lg:text-6xl"
							in:fly={{ y: 50, delay: 400, duration: 700 }}
						>
							<span class="block">Chart Your Own Path with</span>
							<span
								class="from-primary to-accent bg-gradient-to-r bg-clip-text text-transparent"
							>
								Project-Based Learning
							</span>
						</h1>

						<p
							class="text-muted-foreground max-w-xl text-lg md:text-xl"
							in:fly={{ y: 50, delay: 600, duration: 700 }}
						>
							Join our peer-to-peer learning community where you control your educational
							journey. Complete projects, get meaningful feedback, and visualize your
							progress in a 3D galaxy of knowledge.
						</p>

						<div
							class="flex flex-wrap gap-4"
							in:fly={{ y: 50, delay: 800, duration: 700 }}
						>
							<a
								href="#get-started"
								class="bg-primary text-primary-foreground hover:bg-primary/90 flex items-center rounded-lg px-6 py-3 font-medium shadow-md transition-all hover:shadow-lg"
							>
								Get Started
								<ArrowRight class="ml-2 h-5 w-5" />
							</a>
							<a
								href="#features"
								class="bg-secondary text-secondary-foreground hover:bg-secondary/90 rounded-lg px-6 py-3 font-medium shadow-sm transition-all hover:shadow"
							>
								Explore Features
							</a>
						</div>
					</div>

					<div class="relative" in:fade={{ delay: 1200, duration: 1000 }}>
						<div
							class="from-primary to-accent absolute -inset-1 rounded-2xl bg-gradient-to-r opacity-30 blur-md motion-safe:animate-pulse dark:opacity-20"
						></div>
						<div
							class="bg-card dark:bg-card relative z-10 rounded-2xl border p-1 shadow-xl"
						>
							<div
								class="from-muted/30 to-background/30 dark:from-background/10 dark:to-muted/10 flex aspect-[4/3] items-center justify-center overflow-hidden rounded-xl bg-gradient-to-br"
							>
								<!-- Platform UI mockup -->
								<div class="flex h-full w-full flex-col">
									<!-- Mock navigation -->
									<div
										class="bg-background dark:bg-card flex items-center justify-between border-b p-4"
									>
										<div class="flex items-center space-x-3">
											<div class="bg-destructive h-3 w-3 rounded-full"></div>
											<div
												class="h-3 w-3 rounded-full bg-amber-400 dark:bg-amber-500"
											></div>
											<div
												class="h-3 w-3 rounded-full bg-emerald-400 dark:bg-emerald-500"
											></div>
										</div>
										<div class="bg-muted h-4 w-32 rounded-full"></div>
										<div class="bg-muted h-4 w-4 rounded-full"></div>
									</div>

									<!-- Mock content -->
									<div class="grid flex-1 grid-cols-4 gap-4 p-6">
										<div class="col-span-1 space-y-4">
											<div
												class="bg-primary/20 dark:bg-primary/10 h-10 w-full rounded-lg"
											></div>
											<div class="bg-muted h-6 w-3/4 rounded-lg"></div>
											<div class="bg-muted h-6 w-2/3 rounded-lg"></div>
											<div class="bg-muted h-6 w-5/6 rounded-lg"></div>
											<div class="bg-muted h-6 w-4/5 rounded-lg"></div>
										</div>
										<div
											class="bg-card col-span-3 flex flex-col rounded-xl p-4 shadow-sm"
										>
											<div class="bg-primary mb-4 h-8 w-1/3 rounded-lg"></div>
											<div class="mb-4 grid grid-cols-2 gap-3">
												<div
													class="bg-muted/50 dark:bg-muted/20 flex h-32 items-center justify-center rounded-lg"
												>
													<div
														class="bg-primary/20 dark:bg-primary/10 h-10 w-10 rounded-full"
													></div>
												</div>
												<div
													class="bg-muted/50 dark:bg-muted/20 flex h-32 items-center justify-center rounded-lg"
												>
													<div
														class="bg-accent/20 dark:bg-accent/10 h-10 w-10 rounded-full"
													></div>
												</div>
											</div>
											<div class="bg-muted mb-2 h-4 w-full rounded-full"></div>
											<div class="bg-muted h-4 w-5/6 rounded-full"></div>
										</div>
									</div>
								</div>
							</div>
						</div>
					</div>
				{/if}
			</div>
		</div>
	</section>

	<!-- FEATURES SECTION -->
	<section id="features" class="bg-card dark:bg-card relative overflow-hidden py-20">
		<div
			class="from-background dark:from-background absolute left-0 top-0 h-24 w-full bg-gradient-to-b to-transparent dark:to-transparent"
		></div>

		<div
			class="bg-primary/10 dark:bg-primary/5 absolute right-0 top-40 h-64 w-64 rounded-full opacity-30 blur-3xl"
		></div>
		<div
			class="bg-accent/10 dark:bg-accent/5 absolute bottom-20 left-20 h-72 w-72 rounded-full opacity-30 blur-3xl"
		></div>

		<div class="container relative z-10 mx-auto px-4 md:px-6">
			<div class="mx-auto mb-16 max-w-3xl text-center">
				{#if visibleSections.features}
					<div in:fly={{ y: 30, duration: 700, delay: 200 }}>
						<span class="text-primary font-semibold">Core Platform Features</span>
						<h2 class="text-foreground mb-4 mt-2 text-3xl font-bold md:text-4xl">
							Take Control of Your Education
						</h2>
						<p class="text-muted-foreground text-lg">
							Our platform empowers you to learn through practical projects, meaningful
							feedback, and a customizable experience that adapts to your unique
							educational goals.
						</p>
					</div>
				{/if}
			</div>

			<div class="grid gap-8 md:grid-cols-2 lg:grid-cols-4">
				{#if visibleSections.features}
					{#each features as feature, i}
						<div
							class="bg-card dark:bg-card shadow-muted/20 border rounded-xl p-6 shadow-lg transition-all duration-300 hover:-translate-y-1 hover:shadow-xl dark:shadow-black/20"
							in:fly={{ y: 30, duration: 700, delay: 300 + i * 100 }}
						>
							<div
								class="from-primary/20 to-accent/10 dark:from-primary/10 dark:to-accent/5 mb-5 flex h-14 w-14 items-center justify-center rounded-xl bg-gradient-to-br"
							>
								<span class="text-3xl">{feature.icon}</span>
							</div>
							<h3 class="text-foreground mb-3 text-xl font-semibold">{feature.title}</h3>
							<p class="text-muted-foreground">{feature.description}</p>
						</div>
					{/each}
				{/if}
			</div>
		</div>
	</section>

	<!-- STATS SECTION -->
	<section
		id="stats"
		class="from-primary to-accent text-primary-foreground relative overflow-hidden bg-gradient-to-br py-20"
	>
		<!-- Decorative elements -->
		<div class="absolute bottom-0 left-0 right-0 top-0 opacity-10">
			<div
				class="border-primary-foreground absolute left-10 top-10 h-40 w-40 rounded-full border-4"
			></div>
			<div
				class="border-primary-foreground absolute bottom-10 right-10 h-60 w-60 rounded-full border-4"
			></div>
			<div
				class="border-primary-foreground absolute left-1/2 top-1/2 h-80 w-80 -translate-x-1/2 -translate-y-1/2 transform rounded-full border-4"
			></div>
		</div>

		<div class="container relative z-10 mx-auto px-4 md:px-6">
			{#if visibleSections.stats}
				<div class="mb-16 text-center" in:fly={{ y: 30, duration: 700 }}>
					<h2 class="text-3xl font-bold md:text-4xl">Building a Learning Universe</h2>
				</div>

				<div class="grid grid-cols-2 gap-8 lg:grid-cols-4">
					{#each stats as stat, i}
						<div
							class="text-center"
							in:fly={{ y: 30, duration: 700, delay: 200 + i * 100 }}
						>
							<div class="mb-2 text-4xl font-bold md:text-5xl">{stat.value}</div>
							<div class="text-lg opacity-90 md:text-xl">{stat.label}</div>
						</div>
					{/each}
				</div>
			{/if}
		</div>
	</section>

	<!-- COMMUNITY SECTION -->
	<section
		id="community"
		class="from-muted/50 to-background dark:from-background dark:to-muted/10 relative overflow-hidden bg-gradient-to-br py-24"
	>
		<div class="container relative z-10 mx-auto px-4 md:px-6">
			<div class="grid items-center gap-12 md:grid-cols-2">
				{#if visibleSections.community}
					<div in:fly={{ x: -50, duration: 700 }}>
						<span class="text-primary font-semibold">Join Our Community</span>
						<h2 class="text-foreground mb-6 mt-2 text-3xl font-bold md:text-4xl">
							Shape the Future of Learning
						</h2>
						<p class="text-muted-foreground mb-8 text-xl">
							Contribute to our open-source platform by creating projects, defining
							learning goals, and crafting educational paths. Share your expertise or keep
							your progress private—it's all under your control.
						</p>

						<div class="flex flex-wrap gap-4">
							<Button size="lg" href="#github">
								<svg role="img" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><title>GitHub</title><path d="M12 .297c-6.63 0-12 5.373-12 12 0 5.303 3.438 9.8 8.205 11.385.6.113.82-.258.82-.577 0-.285-.01-1.04-.015-2.04-3.338.724-4.042-1.61-4.042-1.61C4.422 18.07 3.633 17.7 3.633 17.7c-1.087-.744.084-.729.084-.729 1.205.084 1.838 1.236 1.838 1.236 1.07 1.835 2.809 1.305 3.495.998.108-.776.417-1.305.76-1.605-2.665-.3-5.466-1.332-5.466-5.93 0-1.31.465-2.38 1.235-3.22-.135-.303-.54-1.523.105-3.176 0 0 1.005-.322 3.3 1.23.96-.267 1.98-.399 3-.405 1.02.006 2.04.138 3 .405 2.28-1.552 3.285-1.23 3.285-1.23.645 1.653.24 2.873.12 3.176.765.84 1.23 1.91 1.23 3.22 0 4.61-2.805 5.625-5.475 5.92.42.36.81 1.096.81 2.22 0 1.606-.015 2.896-.015 3.286 0 .315.21.69.825.57C20.565 22.092 24 17.592 24 12.297c0-6.627-5.373-12-12-12"/></svg>
								GitHub
							</Button>
						</div>

						<div class="mt-10 grid grid-cols-3 gap-4">
							<div
								class="bg-card/50 dark:bg-card/50 rounded-lg p-4 text-center backdrop-blur-sm"
							>
								<div class="text-foreground text-2xl font-bold">150+</div>
								<div class="text-muted-foreground">Contributors</div>
							</div>
							<div
								class="bg-card/50 dark:bg-card/50 rounded-lg p-4 text-center backdrop-blur-sm"
							>
								<div class="text-foreground text-2xl font-bold">25+</div>
								<div class="text-muted-foreground">Languages</div>
							</div>
							<div
								class="bg-card/50 dark:bg-card/50 rounded-lg p-4 text-center backdrop-blur-sm"
							>
								<div class="text-foreground text-2xl font-bold">1000+</div>
								<div class="text-muted-foreground">Projects Created</div>
							</div>
						</div>
					</div>

					<div in:fly={{ x: 50, duration: 700 }}>
						<div
							class="bg-card/50 dark:bg-card/50 rounded-xl border p-8 backdrop-blur-sm"
						>
							<h3 class="text-foreground mb-6 text-2xl font-bold">Our Values</h3>

							<div class="space-y-6">
								<div class="flex">
									<div
										class="bg-primary/20 dark:bg-primary/10 mr-4 flex h-12 w-12 flex-shrink-0 items-center justify-center rounded-full"
									>
										<Accessibility class="h-6 w-6" />
									</div>
									<div>
										<h4 class="text-foreground text-lg font-semibold">
											Learning For Everyone
										</h4>
										<p class="text-muted-foreground">
											Educational resources should be accessible regardless of financial
											situation or background.
										</p>
									</div>
								</div>

								<div class="flex">
									<div
										class="bg-accent/20 dark:bg-accent/10 mr-4 flex h-12 w-12 flex-shrink-0 items-center justify-center rounded-full"
									>
										<Users class="h-6 w-6" />
									</div>
									<div>
										<h4 class="text-foreground text-lg font-semibold">
											Peer-Driven Knowledge
										</h4>
										<p class="text-muted-foreground">
											We believe in the collective power of community feedback and diverse
											perspectives to enhance learning.
										</p>
									</div>
								</div>

								<div class="flex">
									<div
										class="bg-secondary/20 dark:bg-secondary/10 mr-4 flex h-12 w-12 flex-shrink-0 items-center justify-center rounded-full"
									>
										<Orbit class="h-6 w-6" />
									</div>
									<div>
										<h4 class="text-foreground text-lg font-semibold">
											Educational Freedom
										</h4>
										<p class="text-muted-foreground">
											Your learning path should be under your control, with flexibility to
											explore and adapt as your interests evolve.
										</p>
									</div>
								</div>
							</div>
						</div>
					</div>
				{/if}
			</div>
		</div>
	</section>

	<!-- GET STARTED CTA SECTION -->
	<section id="get-started" class="bg-card dark:bg-card py-20">
		<div class="container mx-auto px-4 md:px-6">
			<div
				class="from-primary to-accent relative overflow-hidden rounded-xl bg-gradient-to-r p-8 shadow-xl md:p-12"
			>
				<!-- Decorative elements -->
				<div
					class="absolute right-0 top-0 h-64 w-64 -translate-y-1/2 translate-x-1/2 rounded-full bg-white/10"
				></div>
				<div
					class="absolute bottom-0 left-0 h-64 w-64 -translate-x-1/2 translate-y-1/2 rounded-full bg-white/10"
				></div>

				<div class="relative z-10 mx-auto max-w-3xl text-center">
					<h2 class="text-primary-foreground mb-6 text-3xl font-bold md:text-4xl">
						Begin Your Project-Based Journey
					</h2>
					<p class="text-primary-foreground/80 mb-8 text-xl">
						Join our community to discover projects, get meaningful feedback, and chart
						your unique learning path in a 3D galaxy. Self-host for free or choose our
						subscription for hassle-free access.
					</p>

					<form method="post" action="/auth/keycloak?/signin" use:enhance>
						<Button size="lg" type="submit">
							<LogIn class="mr-2 h-5 w-5" />
							Create Free Account
						</Button>
					</form>
				</div>
			</div>
		</div>
	</section>

	<!-- FOOTER -->
	<footer class="bg-muted/20 dark:bg-muted/10 text-foreground pb-8 pt-16">
		<div class="container mx-auto px-4 md:px-6">
			<div class="mb-12 grid gap-12 md:grid-cols-2 lg:grid-cols-4">
				<div>
					<div
						class="from-primary to-accent mb-4 bg-gradient-to-r bg-clip-text text-2xl font-bold text-transparent"
					>
						W2Edu
					</div>
					<p class="text-muted-foreground mb-6">
						Self-hostable, open-source education platform empowering peer-to-peer learning
						through project-based experiences and community reviews.
					</p>
					<div class="flex space-x-4">
						<!-- Social media icons would go here -->
						<a
							href="#twitter"
							class="bg-muted dark:bg-muted/30 hover:bg-primary/20 dark:hover:bg-primary/20 flex h-10 w-10 items-center justify-center rounded-full transition-colors"
						>
							<Twitter class="h-5 w-5" />
						</a>
						<a
							href="#github"
							class="bg-muted dark:bg-muted/30 hover:bg-primary/20 dark:hover:bg-primary/20 flex h-10 w-10 items-center justify-center rounded-full transition-colors"
						>
							<Github class="h-5 w-5" />
						</a>
						<a
							href="#discord"
							class="bg-muted dark:bg-muted/30 hover:bg-primary/20 dark:hover:bg-primary/20 flex h-10 w-10 items-center justify-center rounded-full transition-colors"
						>
							<Rss class="h-5 w-5" />
						</a>
					</div>
				</div>

				<div>
					<h3 class="text-foreground mb-4 text-lg font-semibold">Platform</h3>
					<ul class="space-y-3">
						<li>
							<a
								href="#projects"
								class="text-muted-foreground hover:text-foreground transition-colors"
								>Projects</a
							>
						</li>
						<li>
							<a
								href="#goals"
								class="text-muted-foreground hover:text-foreground transition-colors"
								>Learning Goals</a
							>
						</li>
						<li>
							<a
								href="#cursi"
								class="text-muted-foreground hover:text-foreground transition-colors"
								>Cursi Paths</a
							>
						</li>
						<li>
							<a
								href="#reviews"
								class="text-muted-foreground hover:text-foreground transition-colors"
								>Peer Reviews</a
							>
						</li>
					</ul>
				</div>

				<div>
					<h3 class="text-foreground mb-4 text-lg font-semibold">Community</h3>
					<ul class="space-y-3">
						<li>
							<a
								href="#forums"
								class="text-muted-foreground hover:text-foreground transition-colors"
								>Forums</a
							>
						</li>
						<li>
							<a
								href="#discord"
								class="text-muted-foreground hover:text-foreground transition-colors"
								>Discord</a
							>
						</li>
						<li>
							<a
								href="#contribute"
								class="text-muted-foreground hover:text-foreground transition-colors"
								>Contribute</a
							>
						</li>
						<li>
							<a
								href="#self-host"
								class="text-muted-foreground hover:text-foreground transition-colors"
								>Self-Hosting Guide</a
							>
						</li>
					</ul>
				</div>

				<div>
					<h3 class="text-foreground mb-4 text-lg font-semibold">Legal</h3>
					<ul class="space-y-3">
						<li>
							<a
								href="#privacy"
								class="text-muted-foreground hover:text-foreground transition-colors"
								>Privacy Policy</a
							>
						</li>
						<li>
							<a
								href="#terms"
								class="text-muted-foreground hover:text-foreground transition-colors"
								>Terms of Service</a
							>
						</li>
						<li>
							<a
								href="#cookies"
								class="text-muted-foreground hover:text-foreground transition-colors"
								>Cookie Policy</a
							>
						</li>
						<li>
							<a
								href="#license"
								class="text-muted-foreground hover:text-foreground transition-colors"
								>License</a
							>
						</li>
					</ul>
				</div>
			</div>

			<div
				class="flex flex-col items-center justify-between border-t pt-8 md:flex-row"
			>
				<p class="text-muted-foreground mb-4 text-sm md:mb-0">
					© {new Date().getFullYear()} W2 B.V. All rights reserved.
				</p>
				<p class="text-muted-foreground text-sm">
					Made with ❤️ by the open-source community
				</p>
			</div>
		</div>
	</footer>
</d>

<style>
	@keyframes float {
		0%,
		100% {
			transform: translateY(0) rotate(-12deg);
		}
		50% {
			transform: translateY(-10px) rotate(-8deg);
		}
	}

	@keyframes float-slow {
		0%,
		100% {
			transform: translateY(0) rotate(12deg);
		}
		50% {
			transform: translateY(-15px) rotate(16deg);
		}
	}

	:global(.animate-float) {
		@media (prefers-reduced-motion: no-preference) {
			animation: float 4s ease-in-out infinite;
		}
	}

	:global(.animate-float-slow) {
		@media (prefers-reduced-motion: no-preference) {
			animation: float-slow 6s ease-in-out infinite;
		}
	}

	:global(.animate-pulse-slow) {
		animation: pulse 3s cubic-bezier(0.4, 0, 0.6, 1) infinite;
	}
</style>
