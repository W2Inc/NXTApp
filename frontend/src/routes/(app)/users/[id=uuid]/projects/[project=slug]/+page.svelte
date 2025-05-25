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
	import { page } from "$app/state";
	import { useForm } from "$lib/utils/api.svelte.js";
	import Card from "$lib/components/ui/card/card.svelte";

	const { data } = $props();
	const md = $state(data.markdown ?? undefined);
	const action = $derived(!data.userProject ? "?/subscribe" : "?/unsubscribe");

	const { form, enhance } = useForm(data.form, { confirm: true });
</script>

{#snippet img(src: string, fallback: string, className: string, alt?: string)}
	<Avatar.Root class="rounded">
		<Avatar.Image class={className} {src} {alt} />
		<Avatar.Fallback class={className}>{fallback}</Avatar.Fallback>
	</Avatar.Root>
{/snippet}

<div class="m-auto max-w-6xl px-4 py-2">
	<div class="grid grid-cols-1 gap-x-2 pt-4 md:grid-cols-[256px_1fr]">
		<Card class="flex max-h-fit flex-col gap-1">
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
				<Button
					variant="outline"
					class="justify-start "
					href="/new/project?edit={data.project.id}"
				>
					<Pen size={16} />
					Edit Project
				</Button>
			{/if}
			<Separator class="my-1" />
			{#if data.project.gitInfo}
				<Button variant="outline" class="justify-start" href={data.project.gitInfo.url} target="_blank">
					Project Source
					<ExternalLink size={16} />
				</Button>
			{/if}
			<Separator class="my-1" />
			<form method="post" {action} use:enhance>
				<input name="id" type="hidden" value={form.data.id} />
				<Button type="submit" class="w-full">
					{#if !data.userProject}
						Subscribe
					{:else}
						Unsubscribe
					{/if}
				</Button>
			</form>
		</Card>
		<Separator class="my-2 md:hidden" />
		<div class="flex flex-col gap-2">
			{#if data.userProject}
				<Card class="flex flex-col gap-3 overflow-auto">
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
					{#await data.userProject}
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
							<Button href="{data.project.slug}/review" class="shadow-l">
								Create a review
								<ShieldCheck />
							</Button>
							<Button variant="outline" class="shadow-l">
								View Git
								<Git />
							</Button>
						</div>
					{/await}
				</Card>
				<Separator />
			{/if}
			<Card class="px-4">
				<Markdown value={md} variant="viewer" />
			</Card>
		</div>
	</div>
</div>
