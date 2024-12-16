// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { Vector3 } from "three";
import { galaxy } from "./config.json";

// ============================================================================

export function gaussianRandom(mean = 0, stdev = 1) {
	let u = 1 - Math.random();
	let v = Math.random();
	let z = Math.sqrt(-2.0 * Math.log(u)) * Math.cos(2.0 * Math.PI * v);

	return z * stdev + mean;
}

export function clamp(value: number, minimum: number, maximum: number) {
	return Math.min(maximum, Math.max(minimum, value));
}

export function spiral(x: number, y: number, z: number, offset: number) {
	let r = Math.sqrt(x ** 2 + y ** 2);
	let theta = offset;
	theta += x > 0 ? Math.atan(y / x) : Math.atan(y / x) + Math.PI;
	theta += (r / galaxy.ARM_X_DIST) * galaxy.SPIRAL;
	return new Vector3(r * Math.cos(theta), z, r * Math.sin(theta));
}
