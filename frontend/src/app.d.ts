/// <reference types="@auth/sveltekit" />

import type { GitHub } from "arctic";
import type { Client } from "openapi-fetch";
import type { paths as BackendRoutes, components } from "$lib/api/types";
import type { paths as KeycloakRoutes } from "$lib/api/keycloak";

// See https://svelte.dev/docs/kit/types#app.d.ts
// for information about these interfaces

declare global {
	type GUID = string;
	type BackendTypes = components["schemas"];
	type Problem = {
		type: string;
		title: string;
		status: number;
		traceId: string;
	};

	type PaginationMeta = {
		pages?: number;
		perPage?: number;
	};

	namespace App {
		// interface Error {}
		interface Locals {
			api: Client<BackendRoutes>;
			keycloak: Client<KeycloakRoutes>;
		}
		// interface PageData {}
		// interface PageState {}
		// interface Platform {}
	}
}

export {};
