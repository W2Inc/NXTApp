<script lang="ts">
	import Button from "$lib/components/buttons/button.svelte";
	import Toggle from "$lib/components/buttons/toggle.svelte";
	import Checkbox from "$lib/components/input/checkbox.svelte";
	import Radio from "$lib/components/input/radio.svelte";
	import Select from "$lib/components/input/select.svelte";
	import Slider from "$lib/components/input/slider.svelte";
	import Link from "$lib/components/links/link.svelte";
	import Loading from "$lib/components/misc/loading.svelte";
	import Modal from "$lib/components/dialogs/modal.svelte";
	import { Icon, Check, Bars3, GlobeAlt } from "svelte-hero-icons";

	import Card from "$lib/components/cards/card.svelte";
	import Comment from "$lib/components/cards/card-comment.svelte";
	import MarkdownEditor from "$lib/components/markdown/markdown-editor.svelte";
	import Tags from "$lib/components/input/tags.svelte";
	import Input from "$lib/components/input/input.svelte";
	import TimlineItem from "$lib/components/misc/timline-item.svelte";
	import Feedback from "$lib/components/cards/card-comment.svelte";
	import { page } from "$app/stores";
	import { toast, Toaster } from "sonner-svelte";
	import { useContext } from "$lib/utils";
	import Pagination from "$lib/components/input/pagination.svelte";
	import CardNavigation from "$lib/components/cards/card-navigation.svelte";
	import Meter from "$lib/components/meters/meter.svelte";
	import FetchSelect from "$lib/components/input/fetch-select.svelte";

	let dialog: Modal;

	//const dataContext = useContext<{data: string}>("balls");

	//dataContext.set({data: "balls"});
</script>

<h1>Every element for styling purposes</h1>
<hr />

<FetchSelect />

<Pagination
	pages={5}
	on:next={(e) => console.log("Fetch page:", e.detail)}
	on:previous={(e) => console.log("Fetch page:", e.detail)}
	on:selected={(e) => console.log("Fetch page:", e.detail)}
/>
<hr />
<br />
<Toaster expand />
<button on:click={async () => {

}}>Render toast</button>

<div style="padding: 16px;">


	<Meter />

	<CardNavigation
	title="Home"
	icon={GlobeAlt}
	description="Home"
	href="/"
	/>

	<ul style="list-style: none;">
		<!--<TimlineItem>
			<Feedback user={$page.data.user} />
		</TimlineItem>
		<TimlineItem>
			<Feedback user={$page.data.user} />
		</TimlineItem>
		<TimlineItem>
			<Feedback user={$page.data.user} />
		</TimlineItem>
		<TimlineItem>
			<Feedback user={$page.data.user} />
		</TimlineItem>-->
	</ul>
	<MarkdownEditor />
	<br />

	<form on:submit|preventDefault>
		<Tags />
	</form>

	<Button highlight on:click={() => prompt("Balls")}>
		<Icon src={Bars3} size="18px" />
	</Button>
	<!--<MarkdownEditor />-->
</div>

<div class="vstack">
	<p>
		Visit <Link href="https://kit.svelte.dev">SvelteKit</Link> to read the documentation
	</p>

	<Button on:click={() => console.log("balls")}>
		<Icon src={Bars3} size="18px" />
	</Button>

	<p>Button as button</p>
	<Button>To Test page</Button>

	<p>Button as href</p>
	<Button href="/test">To Test page</Button>

	<div style="width: 400px; ">
		<Card>
			<p>Sample text NO highlight</p>
			<p>Sample text2</p>
			<p>Card</p>
		</Card>
	</div>

	<div style="width: 400px; ">
		<Card hightlight>
			<p>Sample text highlight</p>
			<p>Sample text2</p>
			<p>Card</p>
		</Card>
	</div>

	<Toggle />
	<Button>
		Submit
		<Icon src={Check} size="16px" solid />
	</Button>

	<div class="hstack">
		<Radio name="radio" value="1" />
		<Radio name="radio" value="2" />
		<Radio name="radio" value="3" />
	</div>

	<Input label="Balls" type="date" />

	<Checkbox />
	<Checkbox />

	<details>
		<summary>Small Form</summary>
		<form style="border: 1px solid orange; padding: 8px; border-radius: var(--br);">
			<div style="display: grid; gap: 8px;">
				<Input required type="email" placeholder="Email" />
				<Input required type="text" placeholder="Username" />
				<div style="display: flex; align-items: center; gap: 8px;">
					<Toggle />
					<span>Remember me</span>
				</div>
				<hr />
				<Button type="submit">Submit</Button>
			</div>
		</form>
	</details>

	<form>
		<input type="text" placeholder="Text..." />
		<textarea placeholder="Textarea..." />
		<Input label="Label me here!" type="email" placeholder="Email..." />
		<Input label="Label me here!" type="password" placeholder="Password..." />
		<Input label="Label me here!" type="number" placeholder="Number..." />
		<Input label="Label me here!" type="date" placeholder="Date..." />
		<Input label="Label me here!" type="time" placeholder="Time..." />
		<Input label="Label me here!" type="datetime-local" placeholder="Datetime..." />
		<Input label="Label me here!" type="url" placeholder="URL..." />
		<Input label="Label me here!" type="tel" placeholder="Phone..." />
		<Input label="Label me here!" type="search" placeholder="Search..." />
		<Input label="Label me here!" type="color" placeholder="Color..." />
		<Input label="Label me here!" type="week" placeholder="Week..." />
		<Input label="Label me here!" type="month" placeholder="Month..." />
		<Input label="Label me here!" type="file" placeholder="File..." />
	</form>

	<Modal bind:this={dialog}>
		<h1>404</h1>
		<p>Nothing about you is here :D</p>
		<hr />
		<form method="dialog">
			<Button type="submit">Ok</Button>
		</form>
	</Modal>
	<div style="width: 200px;">
		<Slider />
	</div>
	<Loading />
	<Button on:click={dialog.show}>Show Modal</Button>
	<Select id="TEST-TEST-TEST">
		<option value="1">Option 1</option>
		<option value="2">Option 2</option>
		<option value="3">Option 3</option>
		<option value="4">SUPER DUPER LONG NAME BECAUSE YES</option>
	</Select>
</div>

<style>
	div.vstack {
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: center;
		gap: 16px;
	}

	div.hstack {
		width: 100%;
		display: flex;
		flex-direction: row;
		align-items: center;
		justify-content: center;
		gap: 16px;
	}
</style>
