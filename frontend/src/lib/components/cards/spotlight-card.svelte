<script lang="ts">
	import { enhance } from "$app/forms";
	import X from "lucide-svelte/icons/x";
	import { Button } from "../ui/button";
	import Separator from "../ui/separator/separator.svelte";

	let parent: HTMLDivElement;
</script>

<div bind:this={parent} class="text-white">
	<div class="rounded flex flex-col gap-1 bg-cover bg-center p-4" style="background-image: url('bg.png');">
		<input type="hidden" name="id" value={123} />

		<h1 class="text-2xl center-content justify-between">
			Hackathon
			<button onclick={() => parent.remove()} class="motion-safe:hover:scale-110 transition-transform" type="submit">
				<X size={18} />
			</button>
		</h1>
		<Separator class="bg-white" />
		<p>Are you ready to learn?</p>
		<p>Join our hackathon on the 20th of June!</p>
		<Button class="w-full" href="https://www.youtube.com/watch?v=xvFZjo5PgG0">
			Join our Waitlist!
		</Button>
	</div>
</div>

<style>
	form {
		position: relative;
	}

</style>
