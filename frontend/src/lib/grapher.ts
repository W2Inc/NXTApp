// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import AleaPNG from "./utils";
import type { Vector3 } from "three";

// ============================================================================
// Grapher is the scenic graph builder for the cursus graph.
// It's responsible of converting the DB Stored JSONB structure into a
// renderable object that we can render in both 2D & 3D.
//
// The Cursus Track is not in a fixed way but essentially just describes which
// goal owns what.
//
// Why is everything prefixed with X ? Because originally things were named
// under the term 'XGraph', helped with keeping things separated.
// ============================================================================

export type XNodeTypes = XGoal | XAction;

export type XActionTypes = "new:goal" | "expand:goal";


export interface XAction {
	type: XActionTypes;
};

export interface XGoal {

}

export interface XNode {

}

export interface XGraph {

}

// ============================================================================

/**
 * @var data The incoming API data.
 * @var variant The variant of data to generate.
 */
export async function build(data: BackendTypes["CursusTrackDO"], variant: "2d" | "3d") {

	return;
}

