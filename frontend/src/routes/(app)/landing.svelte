<script>
	import { Accessibility, ArrowRight, FlaskConical, Github, Rss, Twitter, Users } from 'lucide-svelte';
  import { fade, fly, scale } from 'svelte/transition';

  // State management with Svelte 5's $state
  let isLoaded = $state(false);
  let visibleSections = $state({
    hero: false,
    features: false,
    stats: false,
    community: false
  });

  // Intersection observer for scroll animations
  $effect(() => {
    // Show hero section immediately after a brief delay
    setTimeout(() => {
      visibleSections.hero = true;
      isLoaded = true;
    }, 300);

    // Setup intersection observers for scroll animations
    const sections = ['features', 'stats', 'community'];

    const observer = new IntersectionObserver((entries) => {
      entries.forEach(entry => {
        if (entry.isIntersecting) {
          const sectionId = entry.target.id;
          visibleSections[sectionId] = true;
          observer.unobserve(entry.target);
        }
      });
    }, { threshold: 0.2 });

    sections.forEach(section => {
      const element = document.getElementById(section);
      if (element) observer.observe(element);
    });
  });

  // Features data
  const features = [
    {
      icon: "🎯",
      title: "Personalized Learning Path",
      description: "Tailored courses and recommendations based on your skill level, goals, and learning style."
    },
    {
      icon: "🌐",
      title: "Global Community Access",
      description: "Connect with learners and mentors from around the world to enhance your educational journey."
    },
    {
      icon: "🧠",
      title: "Interactive Content",
      description: "Engage with immersive exercises, quizzes, and projects that reinforce learning through practice."
    },
    {
      icon: "🏆",
      title: "Achievement System",
      description: "Earn badges and certificates to showcase your progress and newly acquired skills."
    }
  ];

  // Stats data
  const stats = [
    { value: "500+", label: "Free Courses" },
    { value: "50K+", label: "Active Learners" },
    { value: "100+", label: "Countries" },
    { value: "12K+", label: "Community Projects" }
  ];
</script>

<main class="overflow-hidden">
  <!-- HERO SECTION -->
  <section class="relative pt-32 pb-24 md:min-h-[calc(100vh-80px)] flex items-center overflow-hidden bg-background dark:bg-background">
    <!-- Background decoration elements -->
    <div class="absolute inset-0 overflow-hidden">
      <div class="absolute -top-40 -right-40 w-96 h-96 bg-primary/10 dark:bg-primary/5 rounded-full blur-3xl"></div>
      <div class="absolute top-1/3 -left-20 w-72 h-72 bg-accent/10 dark:bg-accent/5 rounded-full blur-3xl"></div>
      <div class="absolute bottom-20 right-10 w-60 h-60 bg-secondary/10 dark:bg-secondary/5 rounded-full blur-3xl"></div>
    </div>

    <!-- Animated decorative elements -->
    <div class="absolute hidden md:block top-20 right-10">
      <div class="h-24 w-24 rounded-xl bg-gradient-to-br from-primary/20 to-primary/10 dark:from-primary/10 dark:to-primary/5 backdrop-blur-sm rotate-12 animate-[float_6s_ease-in-out_infinite]"></div>
    </div>
    <div class="absolute hidden md:block bottom-20 left-10">
      <div class="h-16 w-16 rounded-lg bg-gradient-to-br from-accent/20 to-accent/10 dark:from-accent/10 dark:to-accent/5 backdrop-blur-sm -rotate-12 animate-[float_4s_ease-in-out_infinite]"></div>
    </div>

    <div class="container mx-auto px-4 md:px-6 relative z-10">
      <div class="grid md:grid-cols-2 gap-12 md:gap-8 items-center">
        {#if visibleSections.hero}
          <div class="flex flex-col space-y-8">
            <div in:fly={{ y: 50, delay: 200, duration: 700 }}>
              <span class="bg-primary/10 text-primary dark:bg-primary/20 dark:text-primary-foreground px-4 py-1.5 rounded-full text-sm font-medium inline-block">
                Free & Open Source Learning
              </span>
            </div>

            <h1
              class="text-4xl md:text-5xl lg:text-6xl font-bold leading-tight text-foreground"
              in:fly={{ y: 50, delay: 400, duration: 700 }}
            >
              <span class="block">Unlock Your Potential with</span>
              <span class="bg-gradient-to-r from-primary to-accent bg-clip-text text-transparent">
                Interactive Learning
              </span>
            </h1>

            <p
              class="text-lg md:text-xl text-muted-foreground max-w-xl"
              in:fly={{ y: 50, delay: 600, duration: 700 }}
            >
              Join our community-driven platform designed to make learning accessible to everyone. Discover courses, connect with mentors, and grow your skills—all for free.
            </p>

            <div class="flex flex-wrap gap-4" in:fly={{ y: 50, delay: 800, duration: 700 }}>
              <a
                href="#get-started"
                class="bg-primary text-primary-foreground hover:bg-primary/90 font-medium px-6 py-3 rounded-lg shadow-md hover:shadow-lg transition-all flex items-center"
              >
                Get Started
								<ArrowRight />
              </a>
              <a
                href="#features"
                class="bg-secondary text-secondary-foreground hover:bg-secondary/90 font-medium px-6 py-3 rounded-lg shadow-sm hover:shadow transition-all"
              >
                Explore Features
              </a>
            </div>

            <div in:fly={{ y: 50, delay: 1000, duration: 700 }} class="flex items-center space-x-4">
              <div class="flex -space-x-2">
                {#each Array(4) as _, i}
                  <div class="w-8 h-8 rounded-full border-2 border-background dark:border-background bg-gradient-to-br from-muted to-muted/50 dark:from-muted/50 dark:to-muted/30" style={`z-index: ${4-i}`}></div>
                {/each}
              </div>
              <p class="text-sm text-muted-foreground">
                <span class="font-semibold text-foreground">5,000+</span> learners joined this week
              </p>
            </div>
          </div>

          <div class="relative" in:fade={{ delay: 1200, duration: 1000 }}>
            <div class="absolute -inset-1 bg-gradient-to-r from-primary to-accent rounded-2xl blur-md opacity-30 dark:opacity-20 animate-pulse"></div>
            <div class="relative z-10 bg-card dark:bg-card rounded-2xl shadow-xl p-1 border border-border">
              <div class="aspect-[4/3] overflow-hidden rounded-xl bg-gradient-to-br from-muted/30 to-background/30 dark:from-background/10 dark:to-muted/10 flex items-center justify-center">
                <!-- Platform UI mockup -->
                <div class="w-full h-full flex flex-col">
                  <!-- Mock navigation -->
                  <div class="bg-background dark:bg-card border-b border-border p-4 flex items-center justify-between">
                    <div class="flex items-center space-x-3">
                      <div class="w-3 h-3 rounded-full bg-destructive"></div>
                      <div class="w-3 h-3 rounded-full bg-amber-400 dark:bg-amber-500"></div>
                      <div class="w-3 h-3 rounded-full bg-emerald-400 dark:bg-emerald-500"></div>
                    </div>
                    <div class="h-4 w-32 bg-muted rounded-full"></div>
                    <div class="h-4 w-4 bg-muted rounded-full"></div>
                  </div>

                  <!-- Mock content -->
                  <div class="flex-1 p-6 grid grid-cols-4 gap-4">
                    <div class="col-span-1 space-y-4">
                      <div class="h-10 w-full bg-primary/20 dark:bg-primary/10 rounded-lg"></div>
                      <div class="h-6 w-3/4 bg-muted rounded-lg"></div>
                      <div class="h-6 w-2/3 bg-muted rounded-lg"></div>
                      <div class="h-6 w-5/6 bg-muted rounded-lg"></div>
                      <div class="h-6 w-4/5 bg-muted rounded-lg"></div>
                    </div>
                    <div class="col-span-3 bg-card rounded-xl shadow-sm p-4 flex flex-col">
                      <div class="h-8 w-1/3 bg-primary rounded-lg mb-4"></div>
                      <div class="grid grid-cols-2 gap-3 mb-4">
                        <div class="h-32 bg-muted/50 dark:bg-muted/20 rounded-lg flex items-center justify-center">
                          <div class="w-10 h-10 rounded-full bg-primary/20 dark:bg-primary/10"></div>
                        </div>
                        <div class="h-32 bg-muted/50 dark:bg-muted/20 rounded-lg flex items-center justify-center">
                          <div class="w-10 h-10 rounded-full bg-accent/20 dark:bg-accent/10"></div>
                        </div>
                      </div>
                      <div class="h-4 w-full bg-muted rounded-full mb-2"></div>
                      <div class="h-4 w-5/6 bg-muted rounded-full"></div>
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
  <section id="features" class="py-20 bg-card dark:bg-card relative overflow-hidden">
    <div class="absolute top-0 left-0 w-full h-24 bg-gradient-to-b from-background to-transparent dark:from-background dark:to-transparent"></div>

    <div class="absolute right-0 top-40 w-64 h-64 bg-primary/10 dark:bg-primary/5 rounded-full blur-3xl opacity-30"></div>
    <div class="absolute left-20 bottom-20 w-72 h-72 bg-accent/10 dark:bg-accent/5 rounded-full blur-3xl opacity-30"></div>

    <div class="container mx-auto px-4 md:px-6 relative z-10">
      <div class="text-center max-w-3xl mx-auto mb-16">
        {#if visibleSections.features}
          <div in:fly={{ y: 30, duration: 700, delay: 200 }}>
            <span class="text-primary font-semibold">Why Choose Us</span>
            <h2 class="text-3xl md:text-4xl font-bold mt-2 mb-4 text-foreground">Powerful Features for Effective Learning</h2>
            <p class="text-lg text-muted-foreground">Our platform combines cutting-edge technology with proven educational methods to create a learning experience that's engaging, effective, and accessible.</p>
          </div>
        {/if}
      </div>

      <div class="grid md:grid-cols-2 lg:grid-cols-4 gap-8">
        {#if visibleSections.features}
          {#each features as feature, i}
            <div
              class="bg-card dark:bg-card rounded-xl p-6 shadow-lg shadow-muted/20 dark:shadow-black/20 border border-border hover:shadow-xl hover:-translate-y-1 transition-all duration-300"
              in:fly={{ y: 30, duration: 700, delay: 300 + i * 100 }}
            >
              <div class="w-14 h-14 bg-gradient-to-br from-primary/20 to-accent/10 dark:from-primary/10 dark:to-accent/5 flex items-center justify-center rounded-xl mb-5">
                <span class="text-3xl">{feature.icon}</span>
              </div>
              <h3 class="text-xl font-semibold mb-3 text-foreground">{feature.title}</h3>
              <p class="text-muted-foreground">{feature.description}</p>
            </div>
          {/each}
        {/if}
      </div>
    </div>
  </section>

  <!-- STATS SECTION -->
  <section id="stats" class="py-20 bg-gradient-to-br from-primary to-accent text-primary-foreground relative overflow-hidden">
    <!-- Decorative elements -->
    <div class="absolute top-0 left-0 right-0 bottom-0 opacity-10">
      <div class="absolute top-10 left-10 w-40 h-40 rounded-full border-4 border-primary-foreground"></div>
      <div class="absolute bottom-10 right-10 w-60 h-60 rounded-full border-4 border-primary-foreground"></div>
      <div class="absolute top-1/2 left-1/2 w-80 h-80 rounded-full border-4 border-primary-foreground transform -translate-x-1/2 -translate-y-1/2"></div>
    </div>

    <div class="container mx-auto px-4 md:px-6 relative z-10">
      {#if visibleSections.stats}
        <div class="text-center mb-16" in:fly={{ y: 30, duration: 700 }}>
          <h2 class="text-3xl md:text-4xl font-bold">Making an Impact Worldwide</h2>
        </div>

        <div class="grid grid-cols-2 lg:grid-cols-4 gap-8">
          {#each stats as stat, i}
            <div
              class="text-center"
              in:fly={{ y: 30, duration: 700, delay: 200 + i * 100 }}
            >
              <div class="text-4xl md:text-5xl font-bold mb-2">{stat.value}</div>
              <div class="text-lg md:text-xl opacity-90">{stat.label}</div>
            </div>
          {/each}
        </div>
      {/if}
    </div>
  </section>

  <!-- COMMUNITY SECTION -->
  <section id="community" class="py-24 bg-gradient-to-br from-muted/50 to-background dark:from-background dark:to-muted/10 relative overflow-hidden">
    <div class="container mx-auto px-4 md:px-6 relative z-10">
      <div class="grid md:grid-cols-2 gap-12 items-center">
        {#if visibleSections.community}
          <div in:fly={{ x: -50, duration: 700 }}>
            <span class="text-primary font-semibold">Join Our Community</span>
            <h2 class="text-3xl md:text-4xl font-bold mt-2 mb-6 text-foreground">Become Part of Something Bigger</h2>
            <p class="text-xl text-muted-foreground mb-8">Our open-source community brings together learners, educators, and professionals from around the world. Collaborate on projects, share knowledge, and help shape the future of education.</p>

            <div class="flex flex-wrap gap-4">
              <a
                href="#contribute"
                class="bg-primary text-primary-foreground hover:bg-primary/90 font-medium px-6 py-3 rounded-lg shadow-md hover:shadow-lg transition-all flex items-center"
              >
                Contribute
								<ArrowRight />
              </a>
              <a
                href="#github"
                class="border border-muted hover:bg-muted/10 text-foreground font-medium px-6 py-3 rounded-lg transition-all flex items-center"
              >
								 <Github />
                GitHub
              </a>
            </div>

            <div class="mt-10 grid grid-cols-3 gap-4">
              <div class="text-center p-4 bg-card/50 dark:bg-card/50 rounded-lg backdrop-blur-sm">
                <div class="font-bold text-2xl text-foreground">150+</div>
                <div class="text-muted-foreground">Contributors</div>
              </div>
              <div class="text-center p-4 bg-card/50 dark:bg-card/50 rounded-lg backdrop-blur-sm">
                <div class="font-bold text-2xl text-foreground">25+</div>
                <div class="text-muted-foreground">Languages</div>
              </div>
              <div class="text-center p-4 bg-card/50 dark:bg-card/50 rounded-lg backdrop-blur-sm">
                <div class="font-bold text-2xl text-foreground">1000+</div>
                <div class="text-muted-foreground">Pull Requests</div>
              </div>
            </div>
          </div>

          <div in:fly={{ x: 50, duration: 700 }}>
            <div class="bg-card/50 dark:bg-card/50 backdrop-blur-sm p-8 rounded-xl border border-border">
              <h3 class="text-2xl font-bold mb-6 text-foreground">Our Values</h3>

              <div class="space-y-6">
                <div class="flex">
                  <div class="flex-shrink-0 w-12 h-12 bg-primary/20 dark:bg-primary/10 rounded-full flex items-center justify-center mr-4">
										<Accessibility />
                  </div>
                  <div>
                    <h4 class="text-lg font-semibold text-foreground">Accessibility for All</h4>
                    <p class="text-muted-foreground">Education should be accessible regardless of financial situation or background.</p>
                  </div>
                </div>

                <div class="flex">
                  <div class="flex-shrink-0 w-12 h-12 bg-accent/20 dark:bg-accent/10 rounded-full flex items-center justify-center mr-4">
										<Users />
                  </div>
                  <div>
                    <h4 class="text-lg font-semibold text-foreground">Community-Driven</h4>
                    <p class="text-muted-foreground">We believe in the power of community collaboration to create better educational resources.</p>
                  </div>
                </div>

                <div class="flex">
                  <div class="flex-shrink-0 w-12 h-12 bg-secondary/20 dark:bg-secondary/10 rounded-full flex items-center justify-center mr-4">
										<FlaskConical />
                  </div>
                  <div>
                    <h4 class="text-lg font-semibold text-foreground">Continuous Innovation</h4>
                    <p class="text-muted-foreground">We're constantly evolving our platform to incorporate new teaching methods and technologies.</p>
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
  <section id="get-started" class="py-20 bg-card dark:bg-card">
    <div class="container mx-auto px-4 md:px-6">
      <div class="bg-gradient-to-r from-primary to-accent rounded-xl p-8 md:p-12 shadow-xl relative overflow-hidden">
        <!-- Decorative elements -->
        <div class="absolute top-0 right-0 w-64 h-64 bg-white/10 rounded-full translate-x-1/2 -translate-y-1/2"></div>
        <div class="absolute bottom-0 left-0 w-64 h-64 bg-white/10 rounded-full -translate-x-1/2 translate-y-1/2"></div>

        <div class="relative z-10 text-center max-w-3xl mx-auto">
          <h2 class="text-3xl md:text-4xl font-bold text-primary-foreground mb-6">Start Your Learning Journey Today</h2>
          <p class="text-xl text-primary-foreground/80 mb-8">Join thousands of learners who are already benefiting from our free, open-source platform. No credit card required, no hidden fees.</p>
          <a
            href="#signup"
            class="bg-card text-foreground hover:bg-background dark:hover:bg-background/90 font-medium px-8 py-4 rounded-lg shadow-md hover:shadow-lg transition-all inline-block text-lg"
          >
            Create Free Account
          </a>

          <p class="text-primary-foreground/70 mt-6">Already have an account? <a href="#login" class="text-primary-foreground underline hover:no-underline">Log in</a></p>
        </div>
      </div>
    </div>
  </section>

  <!-- FOOTER -->
  <footer class="bg-muted/20 dark:bg-muted/10 text-foreground pt-16 pb-8">
    <div class="container mx-auto px-4 md:px-6">
      <div class="grid md:grid-cols-2 lg:grid-cols-4 gap-12 mb-12">
        <div>
          <div class="text-2xl font-bold bg-gradient-to-r from-primary to-accent bg-clip-text text-transparent mb-4">
            W2Edu
          </div>
          <p class="text-muted-foreground mb-6">Free, open-source education platform dedicated to making learning accessible to everyone.</p>
          <div class="flex space-x-4">
            <!-- Social media icons would go here -->
            <a href="#twitter" class="w-10 h-10 bg-muted dark:bg-muted/30 rounded-full flex items-center justify-center hover:bg-primary/20 dark:hover:bg-primary/20 transition-colors">
							<Twitter />
            </a>
            <a href="#github" class="w-10 h-10 bg-muted dark:bg-muted/30 rounded-full flex items-center justify-center hover:bg-primary/20 dark:hover:bg-primary/20 transition-colors">
							<Github />
            </a>
            <a href="#discord" class="w-10 h-10 bg-muted dark:bg-muted/30 rounded-full flex items-center justify-center hover:bg-primary/20 dark:hover:bg-primary/20 transition-colors">
							<Rss />
            </a>
          </div>
        </div>

        <div>
          <h3 class="text-lg font-semibold mb-4 text-foreground">Platform</h3>
          <ul class="space-y-3">
            <li><a href="#courses" class="text-muted-foreground hover:text-foreground transition-colors">Courses</a></li>
            <li><a href="#topics" class="text-muted-foreground hover:text-foreground transition-colors">Topics</a></li>
            <li><a href="#resources" class="text-muted-foreground hover:text-foreground transition-colors">Resources</a></li>
            <li><a href="#events" class="text-muted-foreground hover:text-foreground transition-colors">Events</a></li>
          </ul>
        </div>

        <div>
          <h3 class="text-lg font-semibold mb-4 text-foreground">Community</h3>
          <ul class="space-y-3">
            <li><a href="#forums" class="text-muted-foreground hover:text-foreground transition-colors">Forums</a></li>
            <li><a href="#discord" class="text-muted-foreground hover:text-foreground transition-colors">Discord</a></li>
            <li><a href="#contribute" class="text-muted-foreground hover:text-foreground transition-colors">Contribute</a></li>
            <li><a href="#blog" class="text-muted-foreground hover:text-foreground transition-colors">Blog</a></li>
          </ul>
        </div>

        <div>
          <h3 class="text-lg font-semibold mb-4 text-foreground">Legal</h3>
          <ul class="space-y-3">
            <li><a href="#privacy" class="text-muted-foreground hover:text-foreground transition-colors">Privacy Policy</a></li>
            <li><a href="#terms" class="text-muted-foreground hover:text-foreground transition-colors">Terms of Service</a></li>
            <li><a href="#cookies" class="text-muted-foreground hover:text-foreground transition-colors">Cookie Policy</a></li>
            <li><a href="#license" class="text-muted-foreground hover:text-foreground transition-colors">License</a></li>
          </ul>
        </div>
      </div>

      <div class="border-t border-border pt-8 flex flex-col md:flex-row justify-between items-center">
        <p class="text-muted-foreground text-sm mb-4 md:mb-0">© {new Date().getFullYear()} W2Edu. All rights reserved.</p>
        <p class="text-muted-foreground text-sm">Made with ❤️ by the open-source community</p>
      </div>
    </div>
  </footer>
</main>

<style>
  @keyframes float {
    0%, 100% { transform: translateY(0) rotate(-12deg); }
    50% { transform: translateY(-10px) rotate(-8deg); }
  }

  @keyframes float-slow {
    0%, 100% { transform: translateY(0) rotate(12deg); }
    50% { transform: translateY(-15px) rotate(16deg); }
  }

  :global(.animate-float) {
    animation: float 4s ease-in-out infinite;
  }

  :global(.animate-float-slow) {
    animation: float-slow 6s ease-in-out infinite;
  }

  :global(.animate-pulse-slow) {
    animation: pulse 3s cubic-bezier(0.4, 0, 0.6, 1) infinite;
  }
</style>
