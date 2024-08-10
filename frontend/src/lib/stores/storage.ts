// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { writable } from "svelte/store";
import { browser } from "$app/environment";
import type { Updater, Writable } from "svelte/store";

// ============================================================================

// Dummy storage for SSR, DO NOT ACTUALLY USE THIS.
const SSRStorage: Storage = {
	length: 0,
	clear: (): void => {},
	getItem: (_: string): string | null => null,
	key: (_: number): string | null => null,
	removeItem: (_: string): void => {},
	setItem: (_: string, _1: string): void => {},
};

/** Options for the useCache store. */
export interface StorageOptions<T> {
	persist?: boolean;
	serializer?: {
		parse: (text: string) => T;
		stringify: (value: T) => string;
	};
}

/**
 * A writeable store that persists to localStorage or sessionStorage.
 * @param key Key for the store.
 * @param defaultValue Default value for the store.
 * @param options Options for the store (persist, serializer, ...)
 */
export function useStorage<T>(key: string, defaultValue: T, options?: StorageOptions<T>) {
	const { persist, serializer } = {
		persist: true,
		serializer: JSON,
		...options,
	};
	const storage = browser ? (persist ? localStorage : sessionStorage) : SSRStorage;

	const storedItem = storage.getItem(key);
	const parsed = storedItem ? serializer.parse(storedItem) : defaultValue;

	let value =
		typeof defaultValue === "object"
			? { ...defaultValue, ...(storedItem ? parsed : {}) }
			: storedItem
				? parsed
				: defaultValue;

	const { subscribe, set: _set } = writable<T>(value, () => {
		if (!browser || !persist) return;

		function handler(e: StorageEvent) {
			if (e.key !== key) return;
			_set((value = e.newValue ? serializer.parse(e.newValue) : defaultValue));
		}

		addEventListener("storage", handler);
		return () => removeEventListener("storage", handler);
	});

	const set = (v: T) => {
		_set((value = v));
		storage.setItem(key, serializer.stringify(value));
	};

	const update = (updater: Updater<T>) => {
		set(updater(value));
	};

	return {
		subscribe,
		set,
		update,
	} as Writable<T>;
}
