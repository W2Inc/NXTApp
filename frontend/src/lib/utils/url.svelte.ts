// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import type {z} from "zod/v4";
import {goto, pushState, replaceState} from "$app/navigation";
import {browser} from "$app/environment";

// ============================================================================

interface QueryOptions {
	replaceState?: boolean;
	keepFocus?: boolean;
	noScroll?: boolean;
	invalidateAll?: boolean;
}

type RouteResource = string | URL | ((url: URL) => boolean);
type ClientResult<K extends string = string> = {
	read(key: K): string | null;
	/**
	 * Write a new value to the URL search parameters.
	 * If the value is null or undefined, it will remove the parameter.
	 *
	 * @param key The query parameter key to write.
	 * @param value The value to set for the query parameter. Can be string, number, boolean, null, or undefined.
	 * @param invalidate Optional array of resources to invalidate after writing the new value. E.g: Changing tabs should re-validate the current page.
	 */
	write(
		key: K,
		value: string | number | boolean | null | undefined,
		invalidate?: Array<RouteResource>
	): void;
	reset(invalidate?: Array<RouteResource>): Promise<void>;
};

type ServerResult<T extends z.ZodType> = {
	read<K extends keyof z.infer<T>>(key: K): z.infer<T>[K] | undefined;
};

/**
 * Universal useQuery function that works on both client and server side.
 *
 * @param url URL object or string
 * @param options Optional query options when using with schema
 */
export function useQuery<K extends string = string>(url: URL | string): ClientResult<K>;
/**
 * Universal useQuery function that works on both client and server side.
 *
 * @param url URL object or string
 * @param schema Schema to parse the URL with.
 */
export function useQuery<T extends z.ZodType>(url: URL | string, schema: T): ServerResult<T>;

export function useQuery<T extends z.ZodType, K extends string = string>(
	url: URL | string,
	schemaOrOptions?: T | QueryOptions,
): ClientResult<K> | ServerResult<T> {
	// Parse arguments to determine if we're in client or server mode
	const isClientMode = schemaOrOptions === undefined;
	const schema = isClientMode ? undefined : schemaOrOptions as T;
	const options = isClientMode ? schemaOrOptions : undefined;

	const defaultOptions: QueryOptions = {
		replaceState: true,
		keepFocus: true,
		noScroll: true,
		invalidateAll: false,
	};

	const mergedOptions = {...defaultOptions, options};
	const urlObject = typeof url === 'string' ? new URL(url) : url;

	if (isClientMode && browser) {
		// CLIENT MODE
		let stateURL = $state(new URL(urlObject.href));
		return {
			read(key: K): string | null {
				return stateURL.searchParams.get(key);
			},

			async write(key: K, value: string | number | boolean | null | undefined, invalidate?: Array<string | URL | ((url: URL) => boolean)>) {
				const newUrl = new URL(window.location.href);
				if (value === null || value === undefined || (typeof value === 'string' && value.length === 0)) {
					newUrl.searchParams.delete(key);
				} else {
					newUrl.searchParams.set(key, String(value));
				}

				// replaceState((stateURL = newUrl), {});
				// window.history.pushState({}, '', newUrl.toString());

				goto(`?${newUrl.searchParams.toString()}`);

				// if (invalidate) {
				// 	await goto((stateURL = newUrl), {invalidate, invalidateAll: false, replaceState: false, keepFocus: true, noScroll: true});
				// } else {
				// 	pushState(newUrl, {});
				// }
			},


			// Reset all parameters
			async reset(invalidate?: Array<string | URL | ((url: URL) => boolean)>) {
				if (!browser) return;

				const newUrl = new URL(window.location.href);
				newUrl.search = "";

				await goto((stateURL = newUrl), {invalidate, invalidateAll: false, replaceState: false, keepFocus: true, noScroll: true});
				// if (invalidate) {
				// } else {
				// 	stateURL = newUrl; // Update internal state
				// 	replaceState((stateURL = newUrl), {}); // Update URL without navigation
				// }
			}
		} satisfies ClientResult<K>;
	}

	const params = new URLSearchParams(urlObject.search);
	const raw = Object.fromEntries(params.entries())
	const result = schema?.safeParse(raw);

	// SERVER MODE
	return {
		read<K extends keyof z.infer<T>>(key: K): z.infer<T>[K] | undefined {
			if (result?.success) {
				return result?.data[key];
			}

			// Since ZOD will not give us a partial result, we will have to go get it ourselves.
			const isBad = result?.error.issues.find((issue) => {
				return issue.path[0] === key;
			});

			if (isBad)
				return undefined;
			// If we still fail to index, just return undefined.
			return raw[key as string] as z.infer<T>[K] | undefined;
		},
	} satisfies ServerResult<T>;
}
