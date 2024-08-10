<script lang="ts">
	// ============================================================================
	// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
	// See README in the root project for more information.
	// ============================================================================
	// Modified by: Nextdemy B.V @ 20/11/2023
	// Source: https://github.com/threlte/threlte/tree/main/apps/docs/src/components/hackathon
	// ============================================================================

	import { useRender, useThrelte } from "@threlte/core";
	import * as THREE from "three";
	import {
		BloomEffect,
		BrightnessContrastEffect,
		ChromaticAberrationEffect,
		EffectComposer,
		EffectPass,
		FXAAEffect,
		RenderPass,
		ToneMappingEffect,
		ToneMappingMode,
	} from "postprocessing";

	const { renderer, scene, camera, size } = useThrelte();
	scene.background = new THREE.Color(0x000000);

	const composer = new EffectComposer(renderer);

	//= Setup effects =//

	const fxaaEffect = new FXAAEffect();

	const chromaticAberrationEffect = new ChromaticAberrationEffect();
	chromaticAberrationEffect.offset.x = 0.0008;
	chromaticAberrationEffect.offset.y = 0;

	const toneMappingEffect = new ToneMappingEffect({
		mode: ToneMappingMode.ACES_FILMIC,
	});

	const bcEffect = new BrightnessContrastEffect({
		brightness: -0.1,
		contrast: 0.1,
	});

	const bloomEffect = new BloomEffect({
		luminanceThreshold: 0.1,
		radius: 0.86,
		mipmapBlur: true,
		intensity: 4,
	});

	//= Setup passes =//

	function setup() {
		composer.removeAllPasses();
		composer.addPass(new RenderPass(scene, camera.current));
		composer.addPass(new EffectPass(camera.current, fxaaEffect));
		composer.addPass(new EffectPass(camera.current, bloomEffect, toneMappingEffect));
	}

	$: $camera && setup();
	$: composer.setSize($size.width, $size.height);

	useRender(() => composer.render());
</script>
