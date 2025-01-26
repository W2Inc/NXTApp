// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import type { z } from "zod";
import { goto } from "$app/navigation";
import { browser } from "$app/environment";
import { SvelteURL, SvelteURLSearchParams } from "svelte/reactivity";

// ============================================================================

interface QueryOptions {
	replaceState?: boolean;
	keepFocus?: boolean;
	noScroll?: boolean;
	invalidateAll?: boolean;
}

export function useQuery<T extends z.ZodObject<any>>(
	schema: T,
	options: QueryOptions = {
		replaceState: true,
		keepFocus: true,
		noScroll: true,
		invalidateAll: true,
	},
) {
	// Initialize state with URL parameters
	let state = $state<Partial<z.infer<T>>>({});
	let url = $state(new URL(browser ? window.location.href : "http://localhost"));

	// Parse and validate URL parameters against schema
	function parseUrlParams(): Partial<z.infer<T>> {
		if (!browser) return {};

		const params = new URLSearchParams(url.search);
		const parameters: Record<string, unknown> = {};

		params.forEach((value, key) => {
			// Handle different types of values
			if (/^\d+$/.test(value)) {
				parameters[key] = Number(value);
			} else if (value === "true" || value === "false") {
				parameters[key] = value === "true";
			} else if (value === "null") {
				parameters[key] = null;
			} else {
				parameters[key] = value;
			}
		});

		try {
			// Attempt to parse with full schema
			return schema.parse(parameters);
		} catch {
			// Fall back to partial schema if full validation fails
			return schema.partial().parse(parameters);
		}
	}

	// Update URL and state
	async function updateUrl(newParams: URLSearchParams) {
		if (!browser) return;

		const newUrl = new URL(window.location.href);
		newUrl.search = newParams.toString();

		await goto(newUrl.toString(), {
			...options,
			replaceState: options.replaceState ?? true,
		});

		url = newUrl;
		state = parseUrlParams();
	}

	// Initialize state if in browser
	if (browser) {
		state = parseUrlParams();

		// Listen for browser navigation events
		window.addEventListener("popstate", () => {
			url = new URL(window.location.href);
			state = parseUrlParams();
		});
	}

	return {
		// Read a single parameter value
		read<K extends keyof z.infer<T>>(key: K): z.infer<T>[K] | undefined {
			return state[key];
		},

		// Write a single parameter value
		async write<K extends keyof z.infer<T>>(key: K, value: z.infer<T>[K]) {
			if (!browser) return;

			try {
				// Validate the value against the schema
				schema.shape[key as string].parse(value);

				const newParams = new URLSearchParams(url.search);

				if (value === null || value === undefined) {
					newParams.delete(key as string);
				} else {
					newParams.set(key as string, String(value));
				}

				await updateUrl(newParams);
			} catch (error) {
				throw new Error(`Invalid value for key "${String(key)}": ${error}`);
			}
		},

		// Read all parameters
		readAll(): Partial<z.infer<T>> {
			return state;
		},

		// Write multiple parameters at once
		async writeAll(values: Partial<z.infer<T>>) {
			if (!browser) return;

			try {
				// Validate all values against the schema
				schema.partial().parse(values);

				const newParams = new URLSearchParams(url.search);

				for (const [key, value] of Object.entries(values)) {
					if (value === null || value === undefined) {
						newParams.delete(key);
					} else {
						newParams.set(key, String(value));
					}
				}

				await updateUrl(newParams);
			} catch (error) {
				throw new Error(`Invalid values: ${error}`);
			}
		},

		// Reset all parameters
		async reset() {
			if (!browser) return;
			await updateUrl(new URLSearchParams());
		},
	};
}
