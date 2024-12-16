// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

const formatter = $state.raw(
	Intl.DateTimeFormat("en-US", {
		year: "numeric",
		month: "short",
		day: "numeric",
		hour: "numeric",
		minute: "numeric",
	}),
);

// ============================================================================

/**
 * Convert a given date timestamp into a human readable form.
 * @param date The Date, unix or string timestamp
 */
export default function(date: string | number | Date) {
	return formatter.format(new Date(date));
}
