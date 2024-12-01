/// <reference types="./types.d.ts" />
/// <reference types="@auth/sveltekit" />

import type { paths } from "$lib/api/types";
import type { Client } from "openapi-fetch";

// See https://svelte.dev/docs/kit/types#app.d.ts
// for information about these interfaces
declare global {
	type GUID = string;

	namespace App {
		// interface Error {}
		interface Locals {
			api: Client<paths>;
		}
		// interface PageData {}
		// interface PageState {}
		// interface Platform {}
	}
}

export {};
