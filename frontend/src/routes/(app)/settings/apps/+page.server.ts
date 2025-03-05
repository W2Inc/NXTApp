// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { z } from "zod";
import { error, fail, redirect } from "@sveltejs/kit";
import type { PageServerLoad } from "./$types";
import { getGithubClientInfo, initiateGithubAuth } from "./github";
import { generateState, GitHub } from "arctic";

// ============================================================================

const schema = z.object({
	id: z.string().uuid().readonly(),
	userName: z.string().readonly(),

	email: z.string().email(),

	displayName: z.string(),
	firstName: z.string(),
	lastName: z.string(),
	twitter: z.string().url(),
	linkedin: z.string().url(),
	github: z.string().url(),
	website: z.string().url(),
});

// ============================================================================

export const load: PageServerLoad = async ({ locals }) => {
	const session = await locals.session();
	const [userIdentities, realmIdentities] = await Promise.all([
		await locals.keycloak.GET(
			"/admin/realms/{realm}/users/{user-id}/federated-identity",
			{
				params: {
					path: {
						"user-id": session!.user_id,
						realm: "student",
					},
				},
			},
		),
		await locals.keycloak.GET("/admin/realms/{realm}/identity-provider/instances", {
			params: {
				query: {
					briefRepresentation: true,
				},
				path: {
					realm: "student",
				},
			},
		}),
	]);

	/*
	  {
    identityProvider: "github",
    userId: "63303990",
    userName: "w2wizard",
  }
	*/

	return {
		realmIdentities: realmIdentities.data ?? [],
		identities: userIdentities.data ?? [],
	};
};

export const actions = {
	link: async ({ request, locals, url }) => {
		const form = await request.formData();
		const provider = form.get("providerId")?.toString();
		if (!provider) return fail(500);

		// Get the client...
		const { data } = await locals.keycloak.GET(
			"/admin/realms/{realm}/identity-provider/instances",
			{
				params: {
					query: {
						briefRepresentation: false,
					},
					path: {
						realm: "student",
					},
				},
			},
		);

		if (!data) error(500);
		const client =  data.filter((i) => i.providerId === "github").pop() ?? error(501);
		const github = new GitHub(
			client.config!["clientId"],
			client.config!["defaultScope"]!,
			"http://localhost:5173/socials/github",
		);

		switch (provider) {
			case "github":
				const state = generateState();
				const scopes = ["user", "repo", "openid"];
				redirect(303, github.createAuthorizationURL(state, scopes));
			default:
				return fail(501);
		}
	},
	// Unlink a social account
	unlink: async ({ request, locals }) => {
		const session = await locals.session();
		const form = await request.formData();
		const provider = form.get("providerId")?.toString();
		if (!provider) return fail(500);

		const { data, response, error } = await locals.keycloak.DELETE(
			"/admin/realms/{realm}/users/{user-id}/federated-identity/{provider}",
			{
				params: {
					path: {
						realm: "student",
						provider: provider,
						"user-id": session!.user_id,
					},
				},
			},
		);

		return {
			data,
		};
	},
};
