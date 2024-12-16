// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { browser } from "$app/environment";

// ============================================================================

export function useStorage(type: "local" | "session" = "local") {
	const storage = browser ? (type === "local" ? localStorage : sessionStorage) : null;

	function get<T>(key: string): T | null {
		if (!storage) return null;
		const value = storage.getItem(key);
		return value ? JSON.parse(value) : null;
	}

	function set<T>(key: string, value: T | null): void {
		if (!storage) return;

		if (value === null) {
			storage.removeItem(key);
		} else {
			storage.setItem(key, JSON.stringify(value));
		}
	}

	return { get, set };
}
