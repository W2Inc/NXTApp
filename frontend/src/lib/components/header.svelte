<script lang="ts">
	import CircleHelp from "lucide-svelte/icons/circle-help";
	import Cog from "lucide-svelte/icons/cog";
	import DollarSign from "lucide-svelte/icons/dollar-sign";
	import Github from "lucide-svelte/icons/github";
	import LogOut from "lucide-svelte/icons/log-out";
	import LogIn from "lucide-svelte/icons/log-in";
	import Menu from "lucide-svelte/icons/menu";
	import Moon from "lucide-svelte/icons/moon";
	import Plus from "lucide-svelte/icons/plus";
	import Server from "lucide-svelte/icons/server";
	import Sun from "lucide-svelte/icons/sun";
	import User from "lucide-svelte/icons/user";
	import House from "lucide-svelte/icons/house";
	import Sparkles from "lucide-svelte/icons/sparkles";
	import Trophy from "lucide-svelte/icons/trophy";
	import Archive from "lucide-svelte/icons/archive";
	import FileBox from "lucide-svelte/icons/file-box";
	import Ellipsis from "lucide-svelte/icons/ellipsis";
	import Bell from "lucide-svelte/icons/bell";
	import BellDot from "lucide-svelte/icons/bell-dot";

	import Button from "./ui/button/button.svelte";
	import { page } from "$app/state";
	import { toggleMode } from "mode-watcher";
	import * as DropdownMenu from "./ui/dropdown-menu";
	import * as Avatar from "./ui/avatar";
	import * as Breadcrumb from "./ui//breadcrumb";
	import Search from "./search/search.svelte";
	import type { IconLink } from "$lib/types";
	import { Constants, encodeID } from "$lib/utils";
	import { hasRole } from "$lib/utils/roles.svelte";
	import { enhance } from "$app/forms";

	const links: IconLink[] = [
		{
			icon: Archive,
			title: "New Project",
			href: `/new/project`,
		},
		{
			icon: Trophy,
			title: "New Goal",
			href: `/new/goal`,
		},
		{
			icon: Sparkles,
			title: "New Cursus",
			href: `/new/cursus`,
		},
		{
			icon: FileBox,
			title: "New Rubric",
			href: `/new/rubric`,
		},
	];
</script>

{#snippet link({ icon, href, title }: IconLink)}
	<a class="flex w-full items-center gap-2" {href}>
		<svelte:component this={icon} size="16"></svelte:component>
		{title}
	</a>
{/snippet}

<header
	class="bg-muted dark:bg-card z-50 flex h-[var(--header-height)] w-full items-center justify-between gap-2 border-b p-3"
>
	<!-- Right side -->
	<div class="flex flex-1 items-center gap-2">
		<Button href="/" variant="outline" size="icon">
			<House />
		</Button>
		<Search />
		<!-- <Breadcrumb.Root>
			<Breadcrumb.List class="hidden md:flex">
				<Breadcrumb.Item>
					<Breadcrumb.Link href="/">Home</Breadcrumb.Link>
				</Breadcrumb.Item>
				<Breadcrumb.Separator />
				<Breadcrumb.Item>
					<Breadcrumb.Link href="/components">Components</Breadcrumb.Link>
				</Breadcrumb.Item>
				<Breadcrumb.Separator />
				<Breadcrumb.Item>
					<Breadcrumb.Page>Breadcrumb</Breadcrumb.Page>
				</Breadcrumb.Item>
			</Breadcrumb.List>
		</Breadcrumb.Root> -->
	</div>

	<!-- Left Side -->
	<div class="flex items-center gap-2">
		<Button onclick={toggleMode} variant="outline" size="icon">
			<Sun class="transition-all dark:-rotate-90 dark:scale-0" />
			<Moon class="absolute scale-0 transition-all dark:rotate-0 dark:scale-100" />
			<span class="sr-only">Toggle theme</span>
		</Button>
		{#if page.data.session}
			<Button href="/notifications" variant="outline" size="icon">
				<Bell />
			</Button>
			{#if hasRole("creator")}
				<DropdownMenu.Root>
					<DropdownMenu.Trigger>
						<Button variant="outline" size="icon">
							<Plus />
							<span class="sr-only">New</span>
						</Button>
					</DropdownMenu.Trigger>
					<DropdownMenu.Content class="">
						<DropdownMenu.Group>
							{#each links as obj}
								<DropdownMenu.Item>
									{@render link(obj)}
								</DropdownMenu.Item>
							{/each}
						</DropdownMenu.Group>
						<DropdownMenu.Separator />
						<DropdownMenu.Item>
							{@render link({
								icon: Ellipsis,
								title: "More",
								href: `/new`,
							})}
						</DropdownMenu.Item>
					</DropdownMenu.Content>
				</DropdownMenu.Root>
			{/if}

			<DropdownMenu.Root>
				<DropdownMenu.Trigger>
					<Avatar.Root class="rounded-sm border border-gray-700">
						<Avatar.Image
							src={page.data.session.avatarUrl ?? Constants.FALLBACK_IMG}
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
								{@render link({
									icon: User,
									title: "Profile",
									href: `/users/${encodeID(page.data.session?.user_id)}`,
								})}
							</DropdownMenu.Item>
							<!-- <DropdownMenu.Item>
								{@render link({
									icon: DollarSign,
									title: "Billing",
									href: `/billing`,
								})}
							</DropdownMenu.Item> -->
							<DropdownMenu.Item>
								{@render link({
									icon: Cog,
									title: "Settings",
									href: `/settings`,
								})}
							</DropdownMenu.Item>
						</DropdownMenu.Group>
						<DropdownMenu.Separator />
						<DropdownMenu.Item>
							{@render link({
								icon: Github,
								title: "Github",
								href: `https://github.com/W2Inc/NXTApp`,
							})}
						</DropdownMenu.Item>
						<!-- <DropdownMenu.Item>
							{@render link({
								icon: CircleHelp,
								title: "Support",
								href: `/help`,
							})}
						</DropdownMenu.Item> -->
						<DropdownMenu.Item>
							{@render link({
								icon: Server,
								title: "API",
								href: `http://localhost:3001/scalar/v1`,
							})}
						</DropdownMenu.Item>
						<DropdownMenu.Separator />
						<form
							method="post"
							use:enhance
							class="grid w-full"
							action="/auth/keycloak?/signout"
						>
							<Button type="submit" variant="destructive" class="w-full">
								<LogOut />
								Logout
							</Button>
						</form>
					</DropdownMenu.Group>
				</DropdownMenu.Content>
			</DropdownMenu.Root>
		{:else}
			<form method="post" action="/auth/keycloak?/signin">
				<Button type="submit">
					<LogIn />
					Signin
				</Button>
			</form>
		{/if}
	</div>
</header>
