/// <reference types="@auth/sveltekit" />

import type { GitHub } from "arctic";
import type { Client } from "openapi-fetch";
import type { paths as BackendRoutes, components } from "$lib/api/types";
import type { paths as KeycloakRoutes } from "$lib/api/keycloak";
import type { useRetryAfter } from "$lib/utils/limiter.svelte";

// See https://svelte.dev/docs/kit/types#app.d.ts
// for information about these interfaces

declare global {
	type GUID = string;
	type BackendTypes = components["schemas"];

	type PaginationMeta = {
		pages?: number;
		perPage?: number;
	};

	// NOTE(W2): Figure out a way to autogenerate this ?
	type S3Buckets = "thumbnail";

	namespace App {
		// interface Error {}
		interface Locals {
			api: Client<BackendRoutes>;
			keycloak: Client<KeycloakRoutes>;
			limiter: ReturnType<typeof useRetryAfter>;
			buckets: Record<S3Buckets, Bun.S3Client>;
		}
		// interface PageData {}
		// interface PageState {}
		// interface Platform {}
	}
}

export {};
