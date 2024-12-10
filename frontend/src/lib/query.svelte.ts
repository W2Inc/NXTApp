// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { browser } from "$app/environment";
import { goto } from "$app/navigation";
import { SvelteURL, SvelteURLSearchParams } from "svelte/reactivity";
import type { z } from "zod";

// ============================================================================

/** Query page handler. Lets you get and set query parameters in the URL. */
export class QueryEffect<T extends z.ZodObject<any>> {
	private schema: T;
	private url: SvelteURL;
	private params: SvelteURLSearchParams;
	private state = $state<Partial<z.infer<T>>>({});

	constructor(initialUrl: string, schema: T) {
		this.schema = schema;
		this.url = new SvelteURL(initialUrl);
		this.params = new SvelteURLSearchParams(this.url.search);

		if (browser) {
			this.updateState();
			window.addEventListener("popstate", () => {
				this.url = new SvelteURL(window.location.href);
				this.params = new SvelteURLSearchParams(this.url.search);
				this.updateState();
			});
		}
	}

	private updateState() {
		this.state = this.parseCurrentParams();
	}

	private parseCurrentParams() {
		const params: Record<string, any> = {};
		this.params.forEach((value, key) => {
			if (/^\d+$/.test(value)) {
				params[key] = Number(value);
			} else {
				params[key] = value;
			}
		});

		try {
			return this.schema.parse(params);
		} catch (error) {
			return this.schema.partial().parse({});
		}
	}

	public get<K extends keyof z.infer<T>>(key: K): z.infer<T>[K] | undefined {
		if (!browser) return undefined;
		return this.state[key];
	}

	public async set<K extends keyof z.infer<T>>(
		key: K,
		value: z.infer<T>[K],
	): Promise<void> {
		if (!browser) return;

		try {
			const fieldSchema = this.schema.shape[key as string];
			fieldSchema.parse(value);

			if (value === undefined || value === null) {
				this.params.delete(key as string);
			} else {
				this.params.set(key as string, String(value));
			}

			// Update URL using Svelte's goto
			await goto(`${this.url.pathname}?${this.params.toString()}`, {
				keepFocus: true,
				replaceState: true,
				noScroll: true,
			});

			// Update the state after URL change
			this.updateState();
		} catch (error) {
			throw new Error(`Invalid value for key "${String(key)}": ${error}`);
		}
	}

	public getAll() {
		return $state.snapshot(this.state);
	}
}
