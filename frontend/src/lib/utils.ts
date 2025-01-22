// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { type ClassValue, clsx } from "clsx";
import { getContext, setContext } from "svelte";
import { twMerge } from "tailwind-merge";

// ============================================================================

export function cn(...inputs: ClassValue[]) {
	return twMerge(clsx(inputs));
}

// ============================================================================

export namespace Constants {
	export const PER_PAGE = 10;
	export const FALLBACK_IMG = "https://avatars.githubusercontent.com/u/0?v=4";
}

// ============================================================================

/**
 * Encode a UUID V4/7 into a URL Friendly short format.
 * @param uuid The UUID
 * @returns The shortened UUID Version.
 */
export function encodeUUID64(uuid: string) {
	const hex = uuid.replace(/-/g, "");
	const binary =
		hex
			.match(/.{1,2}/g)
			?.map((byte) => String.fromCharCode(parseInt(byte, 16)))
			.join("") || "";
	return btoa(binary).replace(/\+/g, "-").replace(/\//g, "_").replace(/=/g, "");
}

export function decodeUUID64(encoded: string) {
	const base64 = encoded.replace(/-/g, "+").replace(/_/g, "/");
	const binary = atob(base64);
	const hex = Array.from(binary)
		.map((char) => char.charCodeAt(0).toString(16).padStart(2, "0"))
		.join("");
	return [
		hex.slice(0, 8),
		hex.slice(8, 12),
		hex.slice(12, 16),
		hex.slice(16, 20),
		hex.slice(20),
	].join("-");
}

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
