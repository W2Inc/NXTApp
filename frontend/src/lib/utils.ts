// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { browser } from "$app/environment";
import { goto } from "$app/navigation";
import { type ClassValue, clsx } from "clsx";
import { SvelteURL, SvelteURLSearchParams } from "svelte/reactivity";
import { twMerge } from "tailwind-merge";
import type { FunctionLikeDeclaration } from "typescript";
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
