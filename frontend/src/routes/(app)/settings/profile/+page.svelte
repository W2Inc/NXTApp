<script lang="ts">
	import { applyAction, enhance } from "$app/forms";
	import { page } from "$app/stores";
	import Button from "$lib/components/buttons/button.svelte";
	import Input from "$lib/components/input/input.svelte";
	import { type SubmitFunction } from "@sveltejs/kit";
	import { BarsArrowUp, Icon, Trash, UserMinus } from "svelte-hero-icons";
	import toast from "sonner-svelte";
	import { recentNavs } from "$lib/stores/recent";
	import AwaitForm from "$lib/components/misc/await-form.svelte";
	import Modal from "$lib/components/dialogs/modal.svelte";
	import Link from "$lib/components/links/link.svelte";
	import CardNotice from "$lib/components/cards/card-notice.svelte";

	let cacheModal: Modal;
	let deleteModal: Modal;
	let sessionModal: Modal;
	let isLoading: boolean;
	const me = $page.data.me!;

	function clearCache() {
		localStorage.clear();
		toast.success("Cache cleared", {
			dismissible: true
		});
	}
</script>

<svelte:head>
	<title>Profile</title>
	<meta name="description" content="Profile settings" />
</svelte:head>

<!-- Modals -->

<Modal title="Cache Control" bind:this={cacheModal}>
	<p>Are you sure you want to delete / empty the cache?</p>
	<Button formmethod="dialog" type="submit" on:click={clearCache}>Yes</Button>
</Modal>

<Modal title="Session Control" bind:this={sessionModal} method="post" action="/settings?/nosessions">
	<p>Are you sure you want to close all remote sessions?</p>
	<p>This will log you out of all devices.</p>
	<Button type="submit">Delete all sessions</Button>
</Modal>

<Modal method="post" title="Account Managment (GDPR)" bind:this={deleteModal}>
	<p>
		Due to GDPR regulations, we are required to handle your data in a certain way.
	</p>
	<p>Once your request is confirmed, your data will be deleted within 30 business days.</p>
	<CardNotice>
		<p>You can read about our privacy policy <Link href="/privacy">here</Link>.</p>
	</CardNotice>
	<hr />
	<Button type="submit" href="mailto:gdpr@nextdemy.com">Request Data/Deletion</Button>
</Modal>

<!-- Page -->

<h1>Profile</h1>
<hr />
<AwaitForm bind:isLoading method="post" action="/settings?/profile">
	<CardNotice>
		<p>
			You can update your profile here. Please note that some fields are immutable and cannot be
			changed.
		</p>
	</CardNotice>

	<div>
		<!--<img
			src={me.details.avatar_url ??
				"https://avatars.githubusercontent.com/u/9919?s=200&v=4"}
			alt="avatar"
			height="128"
			width="128"
		/>
		<Input
			type="file"
			name="avatar"
			id="settings-:avatar:"
			label="Avatar"
			accept="image/png, image/jpeg"
		/>
		<hr />-->
		<Input disabled name="id" id="id" type="url" label="User ID" value={me.data.id} />
		<div class="group">
			<Input
				disabled
				type="url"
				name="username"
				id="settings-:login:"
				label="Username"
				value={me.data.login}
				title="Your unique username handle, this cannot be changed"
			/>
			<Input
				type="text"
				name="Displayname"
				id="settings-:display:"
				label="Display Name"
				value={me.data.display_name}
				pattern="^[a-zA-Z0-9]+$"
				placeholder="Alt"
				minlength={3}
				title="Only alphanumeric characters are allowed."
			/>
		</div>

		<!--<Input
			name="firstName"
			id="settings-:first-name:"
			label="First Name"
			placeholder="Johnny"
			maxlength={32}
			value={me.details.first_name}
			title="Your legal first name"
		/>
		<Input
			name="lastName"
			id="settings-:last-name:"
			label="Last Name"
			placeholder="Silverhand"
			maxlength={32}
			value={me.details.last_name}
			title="Your legal last name"
		/>-->

		<!-- Socials -->
		<!--<div class="group">
			<Input
				name="githubURL"
				id="settings-:githubURL:"
				type="url"
				label="GitHub"
				placeholder="https://github.com/w2wizard"
				pattern="^https:\/\/github\.com\/.*$"
				title="Needs to be a valid GitHub profile url"
				maxlength={128}
				value={me.details.github_url}
			/>
			<Input
				name="linkedinURL"
				id="settings-:linkedinURL:"
				type="url"
				label="LinkedIn"
				placeholder="https://linkedin.com/in/username"
				title="Needs to be a valid LinkedIn profile url"
				maxlength={128}
				value={me.details.linkedin_url}
			/>
			<Input
				name="facebookURL"
				id="settings-:facebookURL:"
				type="url"
				label="Facebook"
				placeholder="https://facebook.com/username"
				title="Needs to be a valid Facebook profile url"
				maxlength={128}
				value={me.details.facebook_url}
			/>
			<Input
				name="twitterURL"
				id="settings-:twitterURL:"
				type="url"
				label="Twitter"
				placeholder="https://twitter.com/username"
				pattern="^https:\/\/twitter\.com\/.*$"
				title="Needs to be a valid Twitter profile url"
				maxlength={128}
				value={me.details.twitter_url}
			/>
		</div>-->

		<hr />
		<div class="options">
			<div class="center">
				<Button disabled={isLoading} type="submit">
					<Icon src={BarsArrowUp} size="18px" solid />
					Update Profile
				</Button>
				<Button type="button" on:click={cacheModal.show}>
					<Icon src={Trash} size="18px" solid />
					Clear Cache
				</Button>
				<Button type="button" on:click={sessionModal.show}>
					<Icon src={Trash} size="18px" solid />
					Remove all Sessions
				</Button>
			</div>
			<Button style="background-color: #cc0000; color: white;" type="button" on:click={deleteModal.show}>
				<Icon src={UserMinus} size="18px" solid />
				Delete Account
			</Button>
		</div>
	</div>
</AwaitForm>

<style>
	.options {
		display: flex;
		justify-content: space-between;
		align-items: center;
	}

	img {
		border-radius: var(--br);
	}

	.group {
		margin-top: 8px;
		display: grid;
		gap: 8px;
		grid-template-columns: 1fr 1fr;
	}
</style>
