// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { type ClassValue, clsx } from "clsx";
import { getContext, setContext } from "svelte";
import { twMerge } from "tailwind-merge";
import { Base58 } from "./base58";
import type { ClientRequestMethod, FetchResponse } from "openapi-fetch";
import { error } from "@sveltejs/kit";

// ============================================================================

export function cn(...inputs: ClassValue[]) {
	return twMerge(clsx(inputs));
}

// ============================================================================

export namespace Constants {
	export const PER_PAGE = 20;
	export const FALLBACK_IMG = "https://avatars.githubusercontent.com/u/0?v=4";
	export const ROLES = ["staff:view", "staff:manage", "creator", "student"]; // TODO: A nicer way to streamline this ?
}

// ============================================================================

/**
 * Encode a ID to a url friendy format
 * @param uuid The UUID
 * @returns The shortened UUID Version.
 */
export const encodeID = (uuid: string) => Base58.encodeUUID(uuid);

/**
 *
 * @param uuid
 * @returns
 */
export const decodeID = (uuid: string) => Base58.decodeToUUID(uuid);

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

type APIFetchResponse<T, E> = {
	data: T;
	error?: never;
	response: Response;
} | {
	data?: never;
	error: E;
	response: Response;
};

export async function ensureAPI<T, E>(
	promise: Promise<APIFetchResponse<T, E>>,
): Promise<[NonNullable<T>, null] | [null, E]> {
	const { data, error: err, response } = await promise;

	if (err) {
		// Somehow the request couldn't be made...
		if (response.status === 400 || response.status === 422)
			return [null, err];
		// Everything else we can't honestly handle
		error(response.status, response.statusText);
	}
	return [data!!, null]
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

export function isOkResponse<T>(response: FetchResponse<T>) {}
