// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { dev } from "$app/environment";
import type { JWT } from "@auth/core/jwt";
import { env } from "$env/dynamic/private";
import { SvelteKitAuth, type Account, type Profile, type User } from "@auth/sveltekit";
import Keycloak from "@auth/sveltekit/providers/keycloak";
import { redirect } from "@sveltejs/kit";

// ============================================================================

declare module "@auth/core/jwt" {
	interface JWT {
		name: string;
		givenName: string;
		lastName: string;
		username: string;
		email: string;
		verified: boolean;
		userID: string;
		sub: string;
		idToken: string;
		accessToken: string;
		refreshToken: string;
		expiresAt: number;
		iat: number;
		exp: number;
		jti: string;
	}
}

declare module "@auth/core/types" {
	interface Session {
		access_token: string;
		error?: "RefreshTokenError";
	}
}

declare module "@auth/core/types" {
	interface User {
		username: string;
		firstName: string;
		lastName: string;
		verified: boolean;
	}
}

interface TokenEndpointResponse {
	readonly access_token: string;
	readonly expires_in: number;
	readonly id_token: string;
	readonly refresh_token: string;
	readonly scope: string;
	readonly token_type: "bearer" | "dpop" | Lowercase<string>;
	readonly [parameter: string]: unknown | undefined;
}

// ============================================================================

export async function requestRefreshToken(token: string) {
	const response = await fetch(`${env.KC_ISSUER}/protocol/openid-connect/token`, {
		headers: { "Content-Type": "application/x-www-form-urlencoded" },
		body: new URLSearchParams({
			client_id: env.KC_CLIENT_ID,
			client_secret: env.KC_CLIENT_SECRET,
			grant_type: "refresh_token",
			refresh_token: token,
		}),
		method: "POST",
		cache: "no-store",
	});

	if (!response.ok) throw new Error("Unable to request refresh token");
	return response.json() as Promise<TokenEndpointResponse>;
}

// ============================================================================

export const { handle, signIn, signOut } = SvelteKitAuth({
	debug: false,
	providers: [
		Keycloak({
			clientId: env.KC_CLIENT_ID,
			clientSecret: env.KC_CLIENT_SECRET,
			issuer: env.KC_ISSUER,
			wellKnown: `${env.KC_ISSUER}/.well-known/openid-configuration`,
		}),
	],
	callbacks: {
		async jwt({ token, account, profile }) {
			if (account && profile) {
				if (
					!account.id_token ||
					!account.access_token ||
					!account.refresh_token ||
					!account.expires_at
				) {
					throw new TypeError("Missing required account properties");
				}

				token.idToken = account.id_token;
				token.accessToken = account.access_token;
				token.refreshToken = account.refresh_token;
				token.expiresAt = account.expires_at;
				token.username = profile.preferred_username!;
				token.verified = profile.email_verified!;
				token.userID = profile.sub!;
				token.picture = profile.picture;
				token.givenName = profile.given_name!;
				token.lastName = profile.family_name!;
				return token;
			} else if (token.expiresAt > Math.floor(Date.now() / 1000)) {
				return token;
			}

			if (!token.refreshToken) throw new TypeError("Missing refresh_token");

			try {
				const newToken = await requestRefreshToken(token.refreshToken);
				token.accessToken = newToken.access_token;
				token.expiresAt = Math.floor(Date.now() / 1000 + newToken.expires_in);
				if (newToken.refresh_token) token.refreshToken = newToken.refresh_token;
				return token;
			} catch (error) {
				redirect(301, "/");
			}
		},
		async session({ session, token }) {
			session.user.id = token.userID;
			session.user.verified = token.verified;
			session.user.username = token.username;
			session.access_token = token.accessToken;
			session.user.image = token.picture ?? null;
			session.user.firstName = token.givenName;
			session.user.lastName = token.lastName;
			return session;
		},
	},
	cookies: {
		callbackUrl: { name: env.KC_COOKIE_NAME },
		sessionToken: { name: env.KC_CALLBACK_NAME },
	},
	trustHost: true,
	secret: env.APP_KEY,
	useSecureCookies: !dev,
});
