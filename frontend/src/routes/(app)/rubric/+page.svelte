<script lang="ts">
	import Splitplane from "$lib/components/misc/splitplane/Splitplane.svelte";
	import Content from "$lib/components/markdown/rubric.md?raw";
	import MarkdownViewer from "$lib/components/markdown/markdown-viewer.svelte";
	import MarkownEditor from "$lib/components/markdown/markdown-editor.svelte";
	import TocViewer from "$lib/components/rubric/toc-viewer.svelte";
	import ViewCode from "$lib/components/rubric/views/view-code.svelte";
	import Explorer from "$lib/components/rubric/file-explorer/explorer.svelte";
	import CardNotice from "$lib/components/cards/card-notice.svelte";
	import Button from "$lib/components/buttons/button.svelte";
	import { ArrowUp, CheckBadge, Icon } from "svelte-hero-icons";
	import { useStorage } from "$lib/stores/storage";
	import Modal from "$lib/components/dialogs/modal.svelte";
	import type { ReviewStore } from "$lib/types";
	import Conclusion from "./conclusion.svelte";

	let partModal: Modal;
	let currentPart: number = 0;
	let contentSection: HTMLElement;
	let showFinale = false;
	let parts = [
		"Introduction",
		"Code Quality",
		"Functional Operation",
		"Performance",
		"Conclusion",
	];

	let reviewState = useStorage<ReviewStore>(`review-state-${0}`, {
		full: "",
	});

	let reviewNote = $reviewState[parts[currentPart] ?? ""] ?? "";
	function updateReviewPart(previous: number, selected: number) {
		const key = parts[previous];
		if (!key) {
			console.error("Invalid key:", previous, parts);
			return;
		}

		reviewState.update((record) => {
			record[key] = reviewNote;

			const nextKey = parts[selected];
			if (nextKey) reviewNote = record[nextKey] || "";

			// Do we need to update the full record every time or just when we are done?
			record.full = parts
				.map((part) => record[part] ?? "")
				.filter((note) => note !== "" && note && note.length > 0)
				.join("\n\n");

			return record;
		});
	}

	function completeReview(e: MouseEvent) {
		if ($reviewState.full.length === 0) {
			e.preventDefault();
			partModal.show();
			return;
		}
	}
</script>

<Modal bind:this={partModal} title="Finish the Review">
	<p>You need to complete at least 1 part!</p>
</Modal>

<!-- Content view -->
{#if !showFinale}
	<Splitplane
		id="sidebar-view-:rubric:"
		min="360px"
		max="75%"
		pos="33%"
		--thickness="10px"
	>
		<section slot="a" bind:this={contentSection}>
			<div class="back-top">
				<Button
					style="background-color: var(--primary);"
					on:click={() => contentSection.scrollTo({ top: 0, behavior: "smooth" })}
				>
					<Icon src={ArrowUp} size="24" />
				</Button>
			</div>

			<div class="selection">
				<TocViewer
					{parts}
					bind:selected={currentPart}
					on:selection={(e) => updateReviewPart(e.detail.previous, e.detail.selected)}
				>
					<svelte:fragment slot="extra">
						<hr />
						<li>
							<Button
								on:click={() => (showFinale = true)}
								title="Finish the review, you need to fill at least one part."
								style="background-color: var(--primary); width: 100%;"
								disabled={$reviewState.full.length === 0}
							>
								Complete Review
								<Icon src={CheckBadge} solid size="18" />
							</Button>
						</li>
					</svelte:fragment>
				</TocViewer>
			</div>

			<!-- TODO: Display conclusion page with merged notes -->
			<div class="sidebar">
				{#key currentPart}
					{#if currentPart === 0}
						<CardNotice style="margin-top: 4px;">
							<p>
								This evaluation is split into different topics / parts. For each part you
								can write down your observations and notes. You can always come back to a
								part and edit your notes. At the end all your notes will be compiled into
								a single document.
							</p>
						</CardNotice>
						<hr />
					{/if}
					<MarkownEditor bind:md={reviewNote} resizable={true} />
					<MarkdownViewer md={Content} />
				{/key}
			</div>
		</section>

		<!-- Rubric View -->
		<section slot="b">
			<Splitplane type="vertical" --thickness="10px" min="0px" max="90%">
				<svelte:fragment slot="a">
					<Explorer />
				</svelte:fragment>
				<svelte:fragment slot="b">
					<!--<ViewImage selected={$selectedFile} />-->
					<ViewCode />
				</svelte:fragment>
			</Splitplane>
		</section>
	</Splitplane>
{:else}
	<div class="conslusion-page">
		<Conclusion on:back={() => (showFinale = false)} reviewID={0} />
	</div>
{/if}

<style>
	div.conslusion-page {
		margin: 0 auto;
		max-width: var(--max-content-width);
		padding: 3rem 1.5rem;
	}

	.selection {
		position: sticky;
		top: 0;
		background-color: var(--background-color);
		padding: 24px;
		border-bottom: 1px solid var(--shade-02);
		box-shadow: var(--box-shadow);
		z-index: 999999;
	}

	.sidebar {
		padding: var(--padding);
		padding-bottom: 32px;
		display: flex;
		flex-direction: column;
		gap: 8px;
	}

	.back-top {
		position: absolute;
		bottom: 0;
		padding: 8px;
		z-index: 999999;
	}

	section {
		/*padding: var(--padding);*/
		max-height: calc(100vh - var(--header-height));

		&[slot="a"] {
			overflow-y: auto;
			overflow-x: hidden;
		}

		&[slot="b"] {
			/*padding: var(--padding);*/
		}
	}
</style>
