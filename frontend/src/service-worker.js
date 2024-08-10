// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

/// <reference types="@sveltejs/kit" />
import { build, files, version } from "$service-worker";

// Create a unique cache name for this deployment
const CACHE = `cache-${version}`;
const ASSETS = [
	...build, // the app itself
	...files, // everything in `static`
];

// ============================================================================

/**
 * Adds a request and its response to the cache.
 * @param {Request} request - The request to cache.
 * @param {Response} response - The response to cache.
 * @returns {Promise<void>}
 */
async function addToCache(request, response) {
	const cache = await caches.open(CACHE);
	await cache.put(request, response);
}

/**
 * Adds multiple resources to the cache.
 * @param {Array<string>} resources - An array of resource URLs to be cached.
 * @returns {Promise<void>} - A Promise that resolves when all resources have been added to the cache.
 */
async function addManyToCache(resources) {
	const cache = await caches.open(CACHE);
	await cache.addAll(resources);
}

self.addEventListener("activate", async (event) => {
	const keys = await caches.keys();
	for (const key of keys) {
		key !== CACHE && (await caches.delete(key));
	}
});

// Cache specified assets during installation
self.addEventListener("install", (event) => {
	event.waitUntil(addManyToCache(ASSETS));
});

// Serve assets from the cache if available, otherwise fetch them from the network
self.addEventListener("fetch", (e) => {
	if (e.request.method !== "GET") return;

	e.respondWith(caches.match(e.request).then((s) => s || fetch(e.request)));

	e.waitUntil(
		(async () => {
			const s = await e.preloadResponse;
			s && (await (await caches.open(p)).put(e.request, s.clone()));
		})()
	);
});
