// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { dev } from "$app/environment";
import { createLogger, format, transports, Logger } from "winston";
import { consoleFormat } from "winston-console-format";

// ============================================================================

/** Logger for writing logs */
export let logger: Logger;

/** Initialize the logger */
export function initLogger() {
	logger = createLogger({
		level: dev ? "debug" : "info",
		format: format.combine(
			format.timestamp(),
			format.ms(),
			format.errors({ stack: true }),
			format.splat(),
			format.json(),
		),
		transports: [
			//
			// - Write all logs with importance level of `error` or higher to `error.log`
			//   (i.e., error, fatal, but not other levels)
			//
			new transports.File({ filename: "error.log", level: "error" }),
			//
			// - Write all logs with importance level of `info` or higher to `combined.log`
			//   (i.e., fatal, error, warn, and info, but not trace)
			//
			new transports.File({ filename: "combined.log" }),
		],
	});

	//
	// If we're not in production then log to the `console` with the format:
	// `${info.level}: ${info.message} JSON.stringify({ ...rest }) `
	//
	if (Bun.env.NODE_ENV !== "production") {
		logger.add(
			new transports.Console({
				format: format.combine(
					format.colorize({ all: true }),
					format.padLevels(),
					consoleFormat({
						showMeta: true,
						metaStrip: ["timestamp", "service"],
						inspectOptions: {
							depth: Infinity,
							colors: true,
							maxArrayLength: Infinity,
							breakLength: 120,
							compact: Infinity,
						},
					}),
				),
			}),
		);
	}
}
