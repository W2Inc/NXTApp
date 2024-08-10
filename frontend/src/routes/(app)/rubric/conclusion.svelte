<script lang="ts">
	import Button from "$lib/components/buttons/button.svelte";
	import CardNotice from "$lib/components/cards/card-notice.svelte";
	import Card from "$lib/components/cards/card.svelte";
	import Modal from "$lib/components/dialogs/modal.svelte";
	import Checkbox from "$lib/components/input/checkbox.svelte";
	import Input from "$lib/components/input/input.svelte";
	import Textarea from "$lib/components/input/textarea.svelte";
	import MarkdownEditor from "$lib/components/markdown/markdown-editor.svelte";
	import AwaitForm from "$lib/components/misc/await-form.svelte";
	import { useStorage } from "$lib/stores/storage";
	import type { ReviewStore } from "$lib/types";
	import { createEventDispatcher } from "svelte";
	import { ArrowLeftCircle, ArrowRightCircle, Icon, RocketLaunch } from "svelte-hero-icons";

	export let reviewID: number;

	let isLoading = false;
	let partModal: Modal;
	const dispatch = createEventDispatcher<{ back: void }>();
	const reviewState = useStorage<ReviewStore>(`review-state-${reviewID}`, { full: "" });

	function clearReviewState() {
		localStorage.removeItem(`review-state-${reviewID}`);
	}
</script>

<Modal bind:this={partModal} title="Review Part">
	<p>You are about to complete the review. Are you sure you want to continue?</p>
	<AwaitForm bind:isLoading method="post" action="/rubric?/finish" on:result={clearReviewState}>
		<!-- TODO: Add warning that some parts are empty? -->
		<Button disabled={isLoading} type="submit">
			Submit
			<Icon src={RocketLaunch} size="18" solid />
		</Button>
	</AwaitForm>
</Modal>

<div class="conclusion">
	<h1>Conclusion</h1>
	<p>
		You are almost <em>done!</em> Please provide a conclusion to your review. This is where
		you can summarize your thoughts and provide any additional feedback you may have.
	</p>
	<p>
		Perhaps you have some suggestions for the person being reviewed? Or maybe you have
		want to gloss over some of the key points you made in the previous sections? Now is
		the time to do so.
	</p>

	<CardNotice>
		<p>
			Need to go back and check the rubric? No problem! You can always go back and edit
			your review before submitting it. Just click the "Go Back" button.
		</p>
	</CardNotice>

	<CardNotice type="warning">
		<p>
			<strong>TODO:</strong> Add check boxes to confirm which learning objectives have been
			met?
		</p>
	</CardNotice>

	<!--<Checkbox id="conclusion-confirmed" name="conclusion-confirmed" required />-->

	<MarkdownEditor bind:md={$reviewState.full} name="conclusion" required />

	<div class="submission">
		<Button type="reset" on:click={() => dispatch("back")}>
			<Icon src={ArrowLeftCircle} size="18" solid />
			Go Back
		</Button>
		<Button on:click={partModal.show}>
			Finish
			<Icon src={ArrowRightCircle} size="18" solid />
		</Button>
	</div>
</div>

<style>
	.conclusion {
		display: grid;
		gap: 8px;
	}

	.submission {
		display: grid;
		grid-template-columns: 1fr 1fr;
		gap: 8px;
	}
</style>
