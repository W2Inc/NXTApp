<script>
	import EvalRatio from "$lib/components/eval-ratio.svelte";
	import Taskcard from "$lib/components/cards/task-card.svelte";
	import * as Accordion from "$lib/components/ui/accordion";
	import * as Avatar from "$lib/components/ui/avatar";
	import Box from "lucide-svelte/icons/box";
	import Archive from "lucide-svelte/icons/archive";
	import Users from "lucide-svelte/icons/users";
	import Git from "lucide-svelte/icons/git-pull-request";
	import ShieldCheck from "lucide-svelte/icons/shield-check";
	import MessageCircleHeart from "lucide-svelte/icons/message-circle-heart";
	import Separator from "$lib/components/ui/separator/separator.svelte";
	import Button from "$lib/components/ui/button/button.svelte";
	import ReviewCard from "$lib/components/cards/review-card.svelte";
	import Pagination from "$lib/components/pagination.svelte";
	import { page } from "$app/stores";

	const { data } = $props();
</script>

{#await data.project}
	<p>Loading...</p>
{:then project}
	<div class="m-auto max-w-6xl px-4 py-2">
		<div class="grid grid-cols-1 gap-x-2 pt-4 md:grid-cols-[256px,1fr]">
			<div class="flex max-h-fit flex-col gap-1 rounded border p-4">
				<img
					src="https://github.com/w2wizard.png"
					alt="logo"
					class="max-h-64 rounded border object-cover"
				/>
				<Separator class="my-1" />
				<EvalRatio />
				<Separator class="my-1" />
				<a class="center-content text-primary underline" href="#">
					<Box size={16} />
					Project Source
				</a>
				<a class="center-content text-primary underline" href="#">
					<Archive size={16} />
					Resources
				</a>
				<Separator class="my-1" />
				<a class="center-content text-primary underline" href="#">
					<Archive size={16} />
					Resources
				</a>
			</div>
			<Separator class="my-2 md:hidden" />
			<div class="flex flex-col gap-2">
				<div class="flex flex-col gap-3 overflow-auto rounded border p-4">
					<h1 class="center-content gap-2 text-2xl font-bold">
						<Users size={36} />
						W2Wizard's Rust Webserver
					</h1>
					<ul class="center-content">
						<li>
							<Avatar.Root class="rounded">
								<Avatar.Image src="https://github.com/shadcn.png" alt="@shadcn" />
								<Avatar.Fallback>CN</Avatar.Fallback>
							</Avatar.Root>
						</li>
						<li>
							<Avatar.Root class="rounded">
								<Avatar.Image src="https://github.com/shadcn.png" alt="@shadcn" />
								<Avatar.Fallback>CN</Avatar.Fallback>
							</Avatar.Root>
						</li>
					</ul>
					<Separator class="my-1" />
					<div class="center-content justify-between">
						<h3 class="center-content gap-2 text-xl font-bold">
							<ShieldCheck size={24} />
							Reviews: 2
						</h3>
						<div class="center-content">
							<Pagination variant="small" />
							<Button variant="outline" class="shadow-l">
								All Reviews
								<MessageCircleHeart />
							</Button>
						</div>
					</div>
					<ul class="[&>*:not(:last-child)]:pb-2">
						<ReviewCard />
						<ReviewCard />
					</ul>
					<Separator class="my-1" />
					<div class="center-content">
						<Button class="shadow-l">
							{#if $page.data.session?.user?.id !== ""}
								Create a review
							{:else}
								Request a review
							{/if}
							<ShieldCheck />
						</Button>
						<Button variant="outline" class="shadow-l">
							View Git
							<Git />
						</Button>
					</div>
				</div>
				<Separator />
				<div class="overflow-auto rounded border p-4">Markdown content goes here.</div>
			</div>
		</div>
	</div>
{/await}
