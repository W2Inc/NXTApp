<script lang="ts">
	import { tweened } from "svelte/motion";
	import type { Audio as ThreeAudio } from "three";
	import { useAudioListener, Audio } from "@threlte/extras";

	//= Public =//

	export let volume = 0.5;
  export let isPlaying = false
	export let src: string = `music.mp3`;
	export const toggle = async () => {
		if (!hasStarted) {
			await context.resume();
			hasStarted = true;
		}
		// TODO: Actually set the context to suspended or not
		if (isPlaying) {
			playRate.set(0);
			isPlaying = false;
		} else {
			playRate.set(1);
			isPlaying = true;
		}
	};

	//= Private =//

	let audio: ThreeAudio;
	let hasStarted = false;
	let tweenDuration = 650;
  let playRate = tweened(0, {
    duration: tweenDuration
  })

	const { context } = useAudioListener();
	$: if (!audio) isPlaying = false;
</script>

<Audio bind:ref={audio} {src} loop {volume} playbackRate={$playRate} />
