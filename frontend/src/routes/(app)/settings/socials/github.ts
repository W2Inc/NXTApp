import { error } from "@sveltejs/kit";

export async function getGithubClientInfo(locals: App.Locals) {
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
	return data.filter((i) => i.providerId === "github").pop() ?? error(501);
}

/**
 * Initiates the GitHub OAuth flow by redirecting to the authorization page
 */
export function initiateGithubAuth(clientId: string, redirectUri: string, scope: string) {
	const params = new URLSearchParams({
		client_id: clientId,
		redirect_uri: redirectUri,
		scope: scope,
		state: "testing",
		allow_signup: "true",
	});

	// Instead of fetching, we return the URL that the client should redirect to
	return `https://github.com/login/oauth/authorize?${params.toString()}`;
}

/**
 * Exchanges the temporary code for an access token
 */
export async function getGithubAccessToken(
	clientId: string,
	clientSecret: string,
	code: string,
	redirectUri: string,
) {
	const params = new URLSearchParams({
		client_id: clientId,
		client_secret: clientSecret,
		code: code,
		redirect_uri: redirectUri,
	});

	const response = await fetch("https://github.com/login/oauth/access_token", {
		method: "POST",
		headers: {
			Accept: "application/json",
			"Content-Type": "application/json",
		},
		body: JSON.stringify(Object.fromEntries(params)),
	});

	if (!response.ok) {
		throw new Error(`Failed to get access token: ${response.statusText}`);
	}

	const data = await response.json();
	return data;
}
