<script lang="ts">
	import EvalRatio from "$lib/components/eval-ratio.svelte";
	import Taskcard from "$lib/components/cards/task-card.svelte";
	import * as Accordion from "$lib/components/ui/accordion";
	import * as Avatar from "$lib/components/ui/avatar";
	import Box from "lucide-svelte/icons/box";
	import Archive from "lucide-svelte/icons/archive";
	import Pen from "lucide-svelte/icons/pen";
	import ExternalLink from "lucide-svelte/icons/external-link";
	import Users from "lucide-svelte/icons/users";
	import Git from "lucide-svelte/icons/git-pull-request";
	import ShieldCheck from "lucide-svelte/icons/shield-check";
	import MessageCircleHeart from "lucide-svelte/icons/message-circle-heart";
	import Separator from "$lib/components/ui/separator/separator.svelte";
	import Button from "$lib/components/ui/button/button.svelte";
	import ReviewCard from "$lib/components/cards/review-card.svelte";
	import Pagination from "$lib/components/pagination.svelte";
	import Markdown from "$lib/components/markdown/markdown.svelte";
	import { Constants } from "$lib/utils.js";
	import Input from "$lib/components/ui/input/input.svelte";
	import { useForm } from "$lib/utils/form.svelte";

	const { data } = $props();
	const md = $state(data.project.markdown);
	const action = $derived(!data.userProject ? "?/subscribe" : "?/unsubscribe");

	const { form, enhance } = useForm(data.form);
</script>

<div class="m-auto max-w-6xl px-4 py-2">
	<div class="grid grid-cols-1 gap-x-2 pt-4 md:grid-cols-[256px,1fr]">
		<div class="flex max-h-fit flex-col gap-1 rounded border p-4">
			<img
				loading="eager"
				src={data.project.thumbnailUrl ?? Constants.FALLBACK_IMG}
				alt="logo"
				class="max-h-64 rounded border object-cover"
			/>
			{#if data.userProject}
				<Separator class="my-1" />
				<EvalRatio />
			{/if}
			{#if data.project.creator?.id === data.session?.user_id}
				<Separator class="my-1" />
				<Button variant="outline" class="justify-start " href="/new/project?edit={data.project.id}">
					<Pen size={16} />
					Edit Project
				</Button>
			{/if}
			<Separator class="my-1" />
			<Button variant="outline" class="justify-start " href={data.project.gitInfo?.url}>
				Project Source
				<ExternalLink size={16} />
			</Button>
			<Separator class="my-1" />
			<form method="post" {action} use:enhance>
				<input
					name="id"
					type="hidden"
					bind:value={form.data.id}
					{...form.constraints.id}
				/>
				<Button type="submit" class="w-full">
					{#if !data.userProject}
						Subscribe
					{:else}
						Unsubscribe
					{/if}
				</Button>
			</form>
		</div>
		<Separator class="my-2 md:hidden" />
		<div class="flex flex-col gap-2">
			{#if data.project}
				<div class="flex flex-col gap-3 overflow-auto rounded border p-4">
					<h1 class="center-content gap-2 text-2xl font-bold">
						<Users size={36} />
						{data.project.name}
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
					{#await data.reviews}
						Loading...
					{:then reviews}
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
						<ul>
							{#each reviews as review}
								<ReviewCard />
							{/each}
						</ul>
						<Separator class="my-1" />
						<div class="center-content">
							<Button class="shadow-l">
								<!-- {#if page.data.session?.user?.id !== ""}
								Create a review
							{:else}
								Request a review
							{/if} -->
								<ShieldCheck />
							</Button>
							<Button variant="outline" class="shadow-l">
								View Git
								<Git />
							</Button>
						</div>
					{/await}
				</div>
				<Separator />
			{/if}
			<Markdown variant="viewer" value={md} />
		</div>
	</div>
</div>
