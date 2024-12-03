/// <reference types="@auth/sveltekit" />

import type { paths as BackendRoutes } from "$lib/api/types";
import type { paths as KeycloakRoutes } from "$lib/api/keycloak";
import type { Client } from "openapi-fetch";

// See https://svelte.dev/docs/kit/types#app.d.ts
// for information about these interfaces
declare global {
	type GUID = string;

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
