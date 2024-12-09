// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { browser } from "$app/environment";
import { goto } from "$app/navigation";
import { type ClassValue, clsx } from "clsx";
import { SvelteURL, SvelteURLSearchParams } from "svelte/reactivity";
import { twMerge } from "tailwind-merge";
import type { z, ZodSchema } from "zod";

// ============================================================================

export function cn(...inputs: ClassValue[]) {
	return twMerge(clsx(inputs));
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
