// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { browser } from "$app/environment";

// ============================================================================

export function useDebounce<T>(delay = 250) {
	let timeout = $state<ReturnType<typeof setTimeout> | null>(null);

	return (fn: Function, ...args: unknown[]) => {
		if (timeout) clearTimeout(timeout);
		timeout = setTimeout(() => {
			fn(...args);
		}, delay);
	}
}
