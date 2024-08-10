<script lang="ts">
	import Logo from "$assets/logo.png";
	import { apiURL } from "$lib/api/api";
	import GH from "$assets/github//github-mark-white.svg";
	import Button from "$lib/components/buttons/button.svelte";
	import Odyssey from "$lib/components/misc/odyssey/odyssey.svelte";
	import Link from "$lib/components/links/link.svelte";
	import HardwareCheck from "$lib/components/misc/hardware-check.svelte";
	import { fade, slide } from "svelte/transition";
	import { quintOut } from "svelte/easing";
	import Input from "$lib/components/input/input.svelte";
	import toast from "sonner-svelte";
	import CardNotice from "$lib/components/cards/card-notice.svelte";
	import { ArrowRight, Icon } from "svelte-hero-icons";

	let ppimg: HTMLImageElement;
	function handleImagePreview(e: Event) {
		if (!e.target || !(e.target instanceof HTMLInputElement) || !e.target.files) {
			toast.error("Failed to show image preview, fire the junior dev!");
		}

		const target = e.target as HTMLInputElement;
		const file = target.files![0];
		if (!file || file.size > 1024 * 1024 * 5) {
			target.files = null;
			return toast.error("File too large!");
		} else if (!["image/png", "image/jpeg", "image/gif"].includes(file.type)) {
			target.files = null;
			return toast.error("Invalid file type!");
		}

		const reader = new FileReader();
		reader.addEventListener("load", () => (ppimg.src = reader.result as string));
		reader.readAsDataURL(file);
	}
</script>

<HardwareCheck>
	<Odyssey />
</HardwareCheck>

<img class="logo" src={Logo} width="96" alt="Company logo" />

<div class="content">
	<div class="mask">
		<h1>Setup your profile</h1>

		<form method="POST" action="/boarding">
			<div class="prof center v">
				<img
					bind:this={ppimg}
					src="https://avatars.githubusercontent.com/u/63303990?v=4?s=400"
					width="96"
					alt="Company logo"
				/>
				<input
					type="file"
					title="Upload a profile picture (max 5MB, png, gif, jpeg)"
					accept="image/png, image/gif, image/jpeg"
					on:change={handleImagePreview}
				/>
			</div>

			<div class="names">
				<Input
					required
					type="text"
					label="Username"
					name="username"
					minlength={3}
					pattern="^[a-zA-Z0-9]+$"
					title="Alpha-numeric characters only"
				/>
				<Input
					required
					type="text"
					label="Display Name"
					name="displayname"
					pattern="^[a-zA-Z0-9]+$"
					minlength={3}
					title="Alpha-numeric characters only"
				/>
				<Input
					type="text"
					label="First Name"
					name="firstname"
					pattern="^[a-zA-Z]+$"
					title="Only letters allowed"
				/>
				<Input
					type="text"
					label="Last Name"
					name="lastname"
					pattern="^[a-zA-Z]+$"
					title="Only letters allowed"
				/>
			</div>

			<Input
				required
				type="email"
				label="Email"
				name="email"
				minlength={3}
				pattern="^[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-zA-Z0-9]+$"
				title="Address must be in format of: <name>@<domain>.<tld> Can only contain alphanumeric characters"
			/>

			<hr />
			<CardNotice type="warning">
				<p>
					<strong>You can only set your username once!</strong>
					Regarding the other settings, you can change them later in your profile.
				</p>
			</CardNotice>

			<Button type="submit" style="width: 100%; margin-top: 8px;">
				Start your education
				<Icon src={ArrowRight} size="24px" solid />
			</Button>
		</form>
	</div>
</div>

<style>
	img.logo {
		border-radius: 8px;
		position: absolute;
		top: 0;
		left: 0;
	}

	div.content {
		display: grid;
		place-items: center;
		height: 100vh;
		overflow-x: hidden;
	}

	div.mask {
		max-width: 480px;
		border-radius: var(--br);
		padding: 16px;
		backdrop-filter: blur(8px);
		border: 1px solid rgba(255, 255, 255, 0.1);
		background-color: rgba(0, 0, 0, 0.25);
		box-shadow: var(--box-shadow);
		animation: slidein 1.75s ease-out;
		position: absolute;

		& h1 {
			padding: none;
			text-align: center;
			color: white;
		}

		& p {
			font-size: 0.8rem;
			color: white;
		}

		@keyframes slidein {
			from {
				filter: blur(8px);
				opacity: 0;
				transform: scale(0.8);
			}

			to {
				filter: blur(0px);
				opacity: 1;
				transform: scale(1);
			}
		}
	}

	div.names {
		display: grid;
		grid-template-columns: 1fr 1fr;
		margin-block: 8px;
		gap: 0px 8px;
	}

	div.prof {
		--width: 128px;
		--height: 128px;

		& img {
			border-radius: 50%;
			height: var(--height);
			width: var(--width);
			pointer-events: none;
			user-select: none;
			background-color: var(--shade-01);
		}

		& input[type="file"] {
			color: transparent;
			width: var(--width);

			&::file-selector-button {
				border: 1px solid var(--border-color);
				border-radius: var(--br);
				background-color: var(--shade-01);
				padding: 8px;
				cursor: pointer;
				margin: 0;
				width: var(--width);
				color: var(--foreground-color);

				&:hover {
					background-color: var(--shade-02);
				}
			}
		}
	}
</style>
