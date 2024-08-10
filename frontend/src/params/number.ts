import type { ParamMatcher } from "@sveltejs/kit";

// Matches any integer
export const match: ParamMatcher = (param) => {
	return /^\d+$/.test(param);
};
