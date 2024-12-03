// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { signIn } from "$lib/auth"
import { redirect } from "@sveltejs/kit"
import type { Actions, PageServerLoad } from "./$types"

// ============================================================================

export const load: PageServerLoad = async () => redirect(301, "/");
export const actions: Actions = { default: signIn }
