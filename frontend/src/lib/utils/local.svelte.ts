import { browser } from "$app/environment";

export function useStorage<T>(storage: Storage = localStorage) {
	if (browser) return; // NOTE(W2): Makes no sense...

	const get = (key: string): T | null => {
		const value = storage.getItem(key);
		return value ? JSON.parse(value) : null;
	};

	const set = (key: string, value: T | null): void => {
		if (value === null) {
			storage.removeItem(key);
		} else {
			storage.setItem(key, JSON.stringify(value));
		}
	};

	return { get, set };
}
