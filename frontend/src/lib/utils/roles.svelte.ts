// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { page } from "$app/state";

// ============================================================================

export type Role = "student" | "staff" | "creator";
/** Check if the current sessions (if it exists) has these roles. */
export function hasRole(required: Role | Role[]): boolean {
	const session = page.data.session;
	if (!session) return false;

	const requiredPerms = Array.isArray(required) ? required : [required];
	return requiredPerms.every((perm) => session.roles.includes(perm));
}
