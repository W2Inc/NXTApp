// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================
// See: https://github.com/sveltejs/kit/issues/9785
//
// I swear to fucking god so much time wasted because no one decided to
// document this shite.
// ============================================================================
// So what does this do ?
//
// Basically when we want to stream back an API call, and it fails, we need to
// somehow *handle* this. When streaming, we return a 200. Great! But oh uh, now
// the API goes, 401! Logically you check for that and do error(status, ...)
//
// Big mistake! Now you're throwing an error and the promise is rejected and
// unhandled. These utilities let us stop this nightmare by simply checking and
// properly (and always) resolve the issue on the server (unless we can't)
// and then on the client trigger a promise rejection to then catch it and
// display whatever error view we want.
//
// And if we do end up with a thrown error regardless (because idk) we can then
// use the error hook in hooks.ts and report it to things like sentry.
// ============================================================================

import { dev } from "$app/environment";
import { goto } from "$app/navigation";
import { isHttpError, isRedirect, type Redirect } from "@sveltejs/kit";

// ============================================================================

export type SafeStream<T> = { data: T } | Redirect | { status: number; message: string };
export type StreamPromise<T> = Promise<SafeStream<T>>;

/**
 * Stream a streamable function back to the client
 * @param fn
 * @returns
 */
export async function stream<T>(fn: () => Promise<T>): StreamPromise<T> {
	const { promise, resolve } = Promise.withResolvers<SafeStream<T>>();
	try {
		resolve({ data: await fn() });
	} catch (err: unknown) {
		if (isHttpError(err)) {
			const hideMessage = !dev && err.status === 500;
			resolve({
				status: err.status,
				message: hideMessage ? "Something went wrong..." : err.body.message,
			});
		} else if (isRedirect(err)) {
			resolve({ status: err.status, location: err.location });
		} else {
			throw err;
		}
	}

	promise.catch(() => {}); // out of tick UnhandledPromiseRejection
	return promise;
}

/**
 * Correctly inteprid the rejected promise and act accordingly
 * @param promise
 *
 * TODO: Handle 401s by deleting the cookies / killing the session on the client.
 * @returns
 */
export async function unstream<T>(promise: StreamPromise<T>): Promise<T> {
	const { promise: p, resolve, reject } = Promise.withResolvers<T>();
	const result = await promise;
	if ("status" in result) {
		if ("location" in result) {
			await goto(result.location);
		} else {
			reject(new Error(result.message));
		}
	} else {
		resolve(result.data);
	}
	return p;
}
