// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import type { State } from "./graph/builder";
import type { IconSource } from "svelte-hero-icons";
import { Color, MeshStandardMaterial, type MeshStandardMaterialParameters } from "three";

export type GUID = string;

export interface Link {
	title: string;
	href?: string;
}

export interface Iconlink {
	link: Link;
	icon: IconSource;
}

// ============================================================================

export type ThemeStore = {
	scheme: string;
	primary: string;
};

export type RecentStore = {
	links: Link[];
};

export interface ReviewStore extends Record<string, string> {
	full: string;
}

// File Explorer (for now used for rubrics)
// ============================================================================

export interface FileStub {
	type: 'file';
	name: string;
	basename: string;
	contents: string;
	text: boolean;
}

export interface DirectoryStub {
	type: 'directory';
	name: string;
	basename: string;
}

export type Stub = FileStub | DirectoryStub;

// Graph
// ============================================================================

export const highlighted = new MeshStandardMaterial({
	roughness: 0.5,
	emissive: new Color("white"),
	emissiveIntensity: 0.8,
});

export const stateColors: Record<State, string> = {
	suggested: "#C4E538",
	available: "#ffffff",
	in_progress: "#FFC312",
	unavailable: "#474747",
	validated: "#009432",
};

/**
 * The various material parameters for the goal mesh of a given state.
 *
 * @see https://threejs.org/docs/#api/en/math/Color
 * @see https://threejs.org/docs/#api/en/materials/MeshStandardMaterial
 */
export const goalParams: Record<State, MeshStandardMaterialParameters> = {
	suggested: {
		// color: new Color(0xEE5A24),
		emissive: new Color(stateColors["suggested"]),
		emissiveIntensity: 0.8,
	},
	available: {
		emissive: new Color(stateColors["available"]),
		emissiveIntensity: 0.8,
	},
	in_progress: {
		emissive: new Color(stateColors["in_progress"]),
		emissiveIntensity: 0.8,
	},
	unavailable: {
		emissive: new Color(stateColors["unavailable"]),
		emissiveIntensity: 0.8,
	},
	validated: {
		emissive: new Color(stateColors["validated"]),
		emissiveIntensity: 2,
	},
};
