// See https://kit.svelte.dev/docs/types#app
// for information about these interfaces

import type { components } from "$lib/api/types";

export interface User {
	data: components["schemas"]["User"];
	details: components["schemas"]["UserDetails"];
}

export interface Session {
	user: User;
	sessionID: string;
	session: components["schemas"]["UserSession"];
}

declare global {
	namespace App {
		// interface Error {
		//	status: number;
		//	message: string;
		// }
		// interface Platform {}

		//interface PageState {
		//	page: number | undefined;
		//}

		interface Locals {
			session: Session | null;
		}

		interface PageData {
			me: User | null;
		}
	}
}

export {};
