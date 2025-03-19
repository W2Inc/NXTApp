<script lang="ts">
	import { gfx} from "../config.json";
	import { useThrelte, useTask } from '@threlte/core'
	import * as THREE from 'three'
  import {
    EffectComposer,
    EffectPass,
    RenderPass,
		BloomEffect,
		BlendFunction,
		KernelSize
  } from 'postprocessing'

	const { scene, camera, renderer, size } = useThrelte()
	const composer = new EffectComposer(renderer)

	function setupEffectCompoers() {
		composer.removeAllPasses();
		composer.addPass(new RenderPass(scene, $camera));

		const bloomEffect = new BloomEffect({
			blendFunction: BlendFunction.SCREEN,
			kernelSize: KernelSize.LARGE,
			luminanceThreshold: 0.25,
			luminanceSmoothing: 1.25,
			resolutionScale: gfx.BLOOM.scale,
			intensity: gfx.BLOOM.intensity,
			radius: gfx.BLOOM.radius,
		});

		composer.addPass(new EffectPass($camera, bloomEffect));
	}

	$effect(() => {
		setupEffectCompoers();
		composer.setSize($size.width, $size.height)
	})

  useTask((delta) => {
    composer.render(delta)
  });
</script>
