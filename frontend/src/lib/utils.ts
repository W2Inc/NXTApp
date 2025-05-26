// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { type ClassValue, clsx } from "clsx";
import { getContext, setContext } from "svelte";
import { error, redirect } from "@sveltejs/kit";
import { twMerge } from "tailwind-merge";
import type { FetchResponse } from "openapi-fetch";

// ============================================================================

export function cn(...inputs: ClassValue[]) {
	return twMerge(clsx(inputs));
}

// ============================================================================

export namespace Constants {
	export const PER_PAGE = 20;
	export const FALLBACK_IMG = "https://avatars.githubusercontent.com/u/0?v=4";
}

// ============================================================================

/**
 * Converts a UUID with hyphens to a URL-friendly format without hyphens
 * @param uuid The UUID with hyphens (e.g., '0927cfb7-6637-4eee-8e29-c11548b23863')
 * @returns The UUID without hyphens (e.g., '0927cfb766374eee8e29c11548b23863')
 */
export const encodeID = (uuid: string) => uuid.replace(/-/g, "");

/**
 * Converts a UUID without hyphens back to standard UUID format with hyphens
 * @param encodedUuid The UUID without hyphens (e.g., '0927cfb766374eee8e29c11548b23863')
 * @returns The UUID with hyphens (e.g., '0927cfb7-6637-4eee-8e29-c11548b23863')
 */
export const decodeID = (encodedUuid: string) => {
	if (encodedUuid.length !== 32 || encodedUuid.includes("-")) return encodedUuid;
	return encodedUuid.replace(/^(.{8})(.{4})(.{4})(.{4})(.{12})$/, "$1-$2-$3-$4-$5");
};

// ============================================================================

/**
 * Provide and Inject mechanism, a thin wrapper.
 * @param key A Key
 * @returns Functions to get or set the context.
 */
export function useContext<T>(key: unknown) {
	return {
		get: () => getContext<T>(key),
		set: (value: T) => setContext(key, value),
	};
}

/**
 * ENSURE that the promise is resolved safely and return the appropriate result.
 *
 * Allows for GO style error handling.
 *
 * @param promise The promise to ensure / await.
 * @returns Either the result or an error.
 */
export async function ensure<T, E = Error>(
	promise: Promise<T>,
): Promise<[T, null] | [null, E]> {
	try {
		return [await promise, null];
	} catch (error) {
		return [null, error as E];
	}
}

/**
 * Tiny wrapper to create a deferred function.
 * @param fn The function to run.
 * @returns None
 * @example
 *
 * ```ts
 * for (let i = 0; i < 3; i++) {
 *		using _ = defer(() => console.log("deferred"));
 *		console.log(i);
 * }
 *```
 */
export function defer(fn: Function) {
	return { [Symbol.dispose]: fn };
}

// ============================================================================
// Based on:
// - Original work copyright © 2010 Johannes Baagøe, under MIT license
// - Derivative work copyright (c) 2017-2020, W. Mac McMeans, under BSD license.
// ============================================================================

/** A pseudorandom number generator based on the Alea algorithm */
export default class AleaPRNG {
	private s0: number = 0;
	private s1: number = 0;
	private s2: number = 0;
	private c: number = 1;
	private initialArgs: string[] = [];

	/**
	 * Creates a new AleaPRNG instance with the provided seeds
	 * @param seeds Seed values to base the number generation of.
	 */
	constructor(...seeds: string[]) {
		this.seed(...seeds);
	}

	/**
	 * Returns a random number between 0 (inclusive) and 1 (exclusive)
	 */
	public random(): number {
		const t = 2091639 * this.s0 + this.c * 2.3283064365386963e-10; // 2^-32
		this.s0 = this.s1;
		this.s1 = this.s2;
		return (this.s2 = t - (this.c = Math.floor(t)));
	}

	/**
	 * Returns a random number with 53-bit resolution between 0 (inclusive) and 1 (exclusive)
	 */
	public fract53(): number {
		return this.random() + ((this.random() * 0x200000) | 0) * 1.1102230246251565e-16; // 2^-53
	}

	/**
	 * Returns a random integer between 0 and 2^32 - 1
	 */
	public int32(): number {
		return Math.floor(this.random() * 0x100000000); // 2^32
	}

	/**
	 * Advances the generator state by a specified number of cycles
	 */
	public cycle(cycles: number = 1): void {
		for (let i = 0; i < cycles; i++) {
			this.random();
		}
	}

	/**
	 * Returns a random number between min (inclusive) and max (inclusive)
	 * Works with both integer and floating point values
	 */
	public range(min: number = 0, max: number = 1): number {
		if (min > max) [min, max] = [max, min];

		if (this.isInteger(min) && this.isInteger(max)) {
			return Math.floor(this.random() * (max - min + 1)) + min;
		} else {
			return this.random() * (max - min) + min;
		}
	}

	/**
	 * Returns a random number that is either between minMin and minMax,
	 * or between maxMin and maxMax, but not in between
	 */
	public rangeBoundary(
		minMin: number,
		minMax: number,
		maxMin: number,
		maxMax: number,
	): number {
		// Ensure correct order of bounds
		if (minMin > minMax) [minMin, minMax] = [minMax, minMin];
		if (maxMin > maxMax) [maxMin, maxMax] = [maxMax, maxMin];
		return this.random() < 0.5 ? this.range(minMin, minMax) : this.range(maxMin, maxMax);
	}

	/** Reinitializes the generator with the initial seed */
	public restart(): void {
		this.seed(...this.initialArgs);
	}

	/** Seeds the generator with new seed values */
	public seed(...seeds: string[]): void {
		this.initialArgs = [...seeds];
		this.init(seeds);
	}

	private init(seeds: string[]): void {
		this.s0 = this.mash(" ");
		this.s1 = this.mash(" ");
		this.s2 = this.mash(" ");
		this.c = 1;

		// Mix in seeds
		for (const seed of seeds) {
			this.s0 -= this.mash(seed);
			if (this.s0 < 0) this.s0 += 1;

			this.s1 -= this.mash(seed);
			if (this.s1 < 0) this.s1 += 1;

			this.s2 -= this.mash(seed);
			if (this.s2 < 0) this.s2 += 1;
		}
	}

	private mash(data: string): number {
		let n = 0xefc8249d; // 4022871197

		for (let i = 0; i < data.length; i++) {
			n += data.charCodeAt(i);

			let h = 0.02519603282416938 * n;
			n = Math.floor(h);
			h -= n;
			h *= n;
			n = Math.floor(h);
			h -= n;
			n += h * 0x100000000; // 2^32
		}

		return (n >>> 0) * 2.3283064365386963e-10; // 2^-32
	}

	private isInteger(value: number): boolean {
		return Math.floor(value) === value;
	}
}
