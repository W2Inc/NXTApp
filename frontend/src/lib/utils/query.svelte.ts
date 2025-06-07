// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import type {z} from "zod";
import {goto} from "$app/navigation";
import {browser} from "$app/environment";

// ============================================================================


export function useQuery2<T extends z.ZodObject<any>>(schema: T | URL)
{

}


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

		// Process parameters to handle validation failures gracefully
		const result: Partial<z.infer<T>> = {};

		// Process each field according to the schema
		for (const key in schema.shape) {
			if (key in parameters) {
				// Try to parse the individual field
				const fieldSchema = schema.shape[key];
				const parseResult = fieldSchema.safeParse(parameters[key]);

				if (parseResult.success) {
					// If valid, use the validated value
					result[key] = parseResult.data;
				} else {
					// If invalid, try to use default if available
					try {
						// This looks for a default in the schema
						const defaultValue = fieldSchema.parse(undefined);
						if (defaultValue !== undefined) {
							result[key] = defaultValue;
						}
						// If no default, we omit the field
					} catch {
						// No default, so we omit the field
					}
				}
			} else {
				// If parameter is not provided, try to use default
				try {
					const defaultValue = schema.shape[key].parse(undefined);
					if (defaultValue !== undefined) {
						result[key] = defaultValue;
					}
				} catch {
					// No default, field remains undefined
				}
			}
		}

		return result;
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

			const newParams = new URLSearchParams(url.search);
			const fieldSchema = schema.shape[key as string];
			const parseResult = fieldSchema.safeParse(value);

			if (parseResult.success) {
				// If validation succeeds, use the validated value
				if (parseResult.data === null || parseResult.data === undefined) {
					newParams.delete(key as string);
				} else {
					newParams.set(key as string, String(parseResult.data));
				}
			} else {
				// If validation fails, try to use default or omit
				try {
					const defaultValue = fieldSchema.parse(undefined);
					if (defaultValue !== undefined) {
						newParams.set(key as string, String(defaultValue));
					} else {
						newParams.delete(key as string);
					}
				} catch {
					// No default, so remove the parameter
					newParams.delete(key as string);
				}
			}

			await updateUrl(newParams);
		},

		// Read all parameters
		readAll(): Partial<z.infer<T>> {
			return state;
		},

		// Write multiple parameters at once
		async writeAll(values: Partial<z.infer<T>>) {
			if (!browser) return;

			const newParams = new URLSearchParams(url.search);

			for (const [key, value] of Object.entries(values)) {
				const fieldSchema = schema.shape[key];
				if (!fieldSchema) continue;

				const parseResult = fieldSchema.safeParse(value);

				if (parseResult.success) {
					// If validation succeeds, use the validated value
					if (parseResult.data === null || parseResult.data === undefined) {
						newParams.delete(key);
					} else {
						newParams.set(key, String(parseResult.data));
					}
				} else {
					// If validation fails, try to use default or omit
					try {
						const defaultValue = fieldSchema.parse(undefined);
						if (defaultValue !== undefined) {
							newParams.set(key, String(defaultValue));
						} else {
							newParams.delete(key);
						}
					} catch {
						// No default, so remove the parameter
						newParams.delete(key);
					}
				}
			}

			await updateUrl(newParams);
		},

		// Reset all parameters
		async reset() {
			if (!browser) return;
			await updateUrl(new URLSearchParams());
		},
	};
}
