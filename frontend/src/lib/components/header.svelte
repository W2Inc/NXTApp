<script>
	import CircleHelp from "lucide-svelte/icons/circle-help";
	import Cog from "lucide-svelte/icons/cog";
	import DollarSign from "lucide-svelte/icons/dollar-sign";
	import Github from "lucide-svelte/icons/github";
	import LogOut from "lucide-svelte/icons/log-out";
	import Menu from "lucide-svelte/icons/menu";
	import Moon from "lucide-svelte/icons/moon";
	import PanelLeft from "lucide-svelte/icons/panel-left";
	import Server from "lucide-svelte/icons/server";
	import Sun from "lucide-svelte/icons/sun";
	import User from "lucide-svelte/icons/user";

	import Button from "./ui/button/button.svelte";
	import { page } from "$app/stores";
	import { SignIn, SignOut } from "@auth/sveltekit/components";
	import { toggleMode } from "mode-watcher";
	import * as DropdownMenu from "./ui/dropdown-menu";
	import * as Avatar from "./ui/avatar";
	import Search from "./search/search.svelte";
	import House from "lucide-svelte/icons/house";
</script>

<header class="bg-muted flex h-[var(--header-height)] w-full justify-between p-3 gap-2 items-center z-50">
	<div class="flex-1">
		<Button href="/" variant="outline" size="icon">
			<House/>
		</Button>
			<!-- <img src="https://github.com/w2wizard.png" width="32" height="32" alt="Logo"/> -->
	</div>
	<Search />
	<div class="flex items-center gap-2">
		<Button onclick={toggleMode} variant="outline" size="icon">
			<Sun class="transition-all dark:-rotate-90 dark:scale-0" />
			<Moon class="absolute scale-0 transition-all dark:rotate-0 dark:scale-100" />
			<span class="sr-only">Toggle theme</span>
		</Button>

		{#if $page.data.session}
			<DropdownMenu.Root>
				<DropdownMenu.Trigger>
					<Avatar.Root class="rounded-sm border border-gray-700">
						<Avatar.Image
							src="https://github.com/w2wizard.png"
							class="border-gray-400"
							alt="@w2wizard"
						/>
						<Avatar.Fallback>CN</Avatar.Fallback>
					</Avatar.Root>
				</DropdownMenu.Trigger>
				<DropdownMenu.Content class="w-56">
					<DropdownMenu.Group>
						<DropdownMenu.GroupHeading>My Account</DropdownMenu.GroupHeading>
						<DropdownMenu.Separator />
						<DropdownMenu.Group>
							<DropdownMenu.Item>
								<User />
								Profile
							</DropdownMenu.Item>
							<DropdownMenu.Item>
								<DollarSign />
								Billing
							</DropdownMenu.Item>
							<DropdownMenu.Item>
								<a class="flex w-full items-center gap-2" href="/settings">
									<Cog size="16" />
									Settings
								</a>
							</DropdownMenu.Item>
						</DropdownMenu.Group>
						<DropdownMenu.Separator />
						<DropdownMenu.Item>
							<a
								class="flex w-full items-center gap-2"
								href="https://github.com/W2Inc/NXTApp"
							>
								<Github size="16" />
								GitHub
							</a>
						</DropdownMenu.Item>
						<DropdownMenu.Item>
							<a
								class="flex w-full items-center gap-2"
								href="https://github.com/W2Inc/NXTApp"
							>
								<CircleHelp size="16" />
								Support
							</a>
						</DropdownMenu.Item>
						<DropdownMenu.Item>
							<a
								class="flex w-full items-center gap-2"
								href="https://github.com/W2Inc/NXTApp"
							>
								<Server size="16" />
								API
							</a>
						</DropdownMenu.Item>
						<DropdownMenu.Separator />
						<SignOut className="w-full grid" provider="keycloak" signOutPage="signout">
							<Button variant="destructive" class="w-full" inert slot="submitButton">
								<LogOut />
								Logout
							</Button>
						</SignOut>
					</DropdownMenu.Group>
				</DropdownMenu.Content>
			</DropdownMenu.Root>
		{:else}
			<SignIn provider="keycloak" signInPage="signin">
				<Button inert slot="submitButton">Signin</Button>
			</SignIn>
		{/if}
	</div>
</header>
