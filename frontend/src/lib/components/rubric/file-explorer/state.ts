// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { derived } from "svelte/store";
import { clientWritable } from "$lib/utils";
import type { FileStub, Stub } from "$lib/types";

// ============================================================================

/** The files being displayed. */
export const files = clientWritable<Stub[]>([]);
/** The current file name being displayed. */
export const selectedName = clientWritable<string | null>(null);
/** The currently selected file. */
export const selectedFile = derived([files, selectedName], ([$files, $selected_name]) => {
	return (
		($files.find((stub) => stub.name === $selected_name) as FileStub | undefined) ?? null
	);
});
