<script>
	import { enhance } from "$app/forms";
	import Button from "$lib/components/ui/button/button.svelte";
	import { ArrowRight, LogIn, User } from "lucide-svelte";
	import { fade, fly, scale } from "svelte/transition";
	import Moon from "lucide-svelte/icons/moon";
	import Sun from "lucide-svelte/icons/sun";
	import { toggleMode } from "mode-watcher";
	import { Tween } from "svelte/motion";
	import Pixelate from "$lib/components/pixelate.svelte";

	let beginAnimation = $state(false);
	$effect(() => {
		setTimeout(() => {
			beginAnimation = true;
			tween.set(0);
		}, 300);
	});
</script>

<div class="overflow-hidden">
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
				class="from-primary/20 to-primary/10 dark:from-primary/10 dark:to-primary/5 bg-linear-to-br h-24 w-24 rotate-12 rounded-xl backdrop-blur-sm"
			></div>
		</div>
		<div class="animate-float-slow absolute bottom-20 left-10 hidden md:block">
			<div
				class="from-primary/20 to-primary/10 dark:from-primary/10 dark:to-primary/5 bg-linear-to-br h-16 w-16 -rotate-12 rounded-lg backdrop-blur-sm"
			></div>
		</div>

		<div class="container relative z-10 mx-auto px-4 md:px-6">
			<div class="grid items-center gap-12 md:grid-cols-2 md:gap-8">
				{#if beginAnimation}
					<!-- Left column with text and login buttons -->
					<div class="flex flex-col space-y-6" in:fly={{ y: 50, duration: 700 }}>
						<div>
							<span
								class="bg-primary/10 text-primary dark:bg-primary/20 dark:text-primary-foreground inline-block rounded-full px-4 py-1.5 text-sm font-medium"
							>
								Free & Open Source Education
							</span>
						</div>

						<Pixelate delay={320}>
							<h1
								class="text-foreground text-4xl font-bold leading-tight md:text-5xl lg:text-6xl"
							>
								<span class="block">Access Your</span>
								<span
									class="from-primary to-accent bg-linear-to-r bg-clip-text text-transparent"
								>
									Learning Potential
								</span>
							</h1>

							<p class="text-muted-foreground max-w-xl text-lg md:text-xl">
								Continue your project-based learning experience.
							</p>
						</Pixelate>

						<form method="post" use:enhance>
							<div class="flex flex-wrap gap-4 pt-4">
								<Button type="submit" formaction="/auth/keycloak?/signin">
									Signin
									<LogIn />
								</Button>
								<!-- <Button
									href="/explore"
									class="bg-secondary text-secondary-foreground hover:bg-secondary/90 flex items-center rounded-lg px-6 py-3 font-medium shadow-sm transition-all hover:shadow"
								>
									Continue as Guest
									<User />
								</Button> -->
							</div>
						</form>
					</div>

					<!-- Right column with UI mockup -->
					<div
						class="relative flex justify-center"
						in:fade={{ delay: 1200, duration: 250 }}
					>
						<div
							class="bg-card dark:bg-card animate-mock relative z-10 w-full max-w-md rounded-2xl border p-1 shadow-xl"
						>
							<div
								class="from-muted/30 to-background/30 dark:from-background/10 dark:to-muted/10 aspect-4/3 bg-linear-to-br flex items-center justify-center overflow-hidden rounded-xl"
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
										<div class="bg-card col-span-3 flex flex-col rounded-xl border p-4">
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
	<Button
		onclick={toggleMode}
		variant="outline"
		size="icon"
		class="absolute left-[10px] top-[10px]"
	>
		<Sun class="transition-all dark:-rotate-90 dark:scale-0" />
		<Moon class="absolute scale-0 transition-all dark:rotate-0 dark:scale-100" />
		<span class="sr-only">Toggle theme</span>
	</Button>
</div>

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

	@keyframes float-mock {
		0%,
		100% {
			transform: translateY(0) rotate(-1.5deg);
		}
		50% {
			transform: translateY(-10px) rotate(1.5deg);
		}
	}

	:global(.animate-float) {
		@media (prefers-reduced-motion: no-preference) {
			animation: float 4s ease-in-out infinite;
		}
	}

	:global(.animate-mock) {
		animation-delay: 250ms;
		@media (prefers-reduced-motion: no-preference) {
			animation: float-mock 10s ease-in-out infinite;
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
