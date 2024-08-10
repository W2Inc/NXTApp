// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import type { Action, Goal, Node } from "$lib/graph/builder";
import { clientWritable } from "$lib/utils";

// ============================================================================

export const selectedGoal = clientWritable<Node<Goal> | null>(null);
export const selectedAction = clientWritable<Node<Action> | null>(null);
