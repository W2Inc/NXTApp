<script lang="ts">
	import { Tween } from "svelte/motion";
	import type { Audio as ThreeAudio } from "three";
	import { useAudioListener, Audio } from "@threlte/extras";

	interface Props {
		volume?: number;
		playing: boolean;
	}

	const src = `/music.mp3`;
	let { volume = 0.5, playing = $bindable(false) }: Props = $props();

	let audio = $state<ThreeAudio>();
	let hasStarted = $state(false);
	let playRate = new Tween(0, {
		duration: 250,
	});

	const { context } = useAudioListener();
	$effect(() => {
		if (!hasStarted && playing == true) {
			context.resume().then(() => {
				hasStarted = true;
			});
		}
		if (!playing) {
			playRate.set(0);
			// TODO: Properly halt the audio context
		} else {
			playRate.set(1);
		}
	});
</script>

<Audio bind:ref={audio} {src} loop {volume} autoplay playbackRate={playRate.current} />
