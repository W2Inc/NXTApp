// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { useContext } from "$lib/utils";
import type { Writable } from "svelte/store";

// ============================================================================

/** Context for the file tree */
export namespace ExplorerContext {
	export type FileTreeContext = {
		/** Record of which directories are collapsed */
		collapsed: Writable<Record<string, boolean>>;
		/** The currently selected file or directory */
		select: (name: string) => void;
		/** Add a file or directory to the tree */
		add: (name: string, type: "file" | "directory") => Promise<void>;
	};

	export const { get, set } = useContext<FileTreeContext>({});
}
