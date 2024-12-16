import { error } from "@sveltejs/kit";

export default class KeycloakClient {
	private clientId: string;
	private clientSecret: string;
	private url: string;
	private token: string | null;
	private refreshToken: string | null;

	constructor(clientId: string, clientSecret: string, issuer: string) {
		this.clientId = clientId;
		this.clientSecret = clientSecret;
		this.url = issuer;
		this.token = null;
		this.refreshToken = null;
	}

	/**
	 * Get the client from keycloak and it's configuration
	 *
	 * Why this ? So we can avoid pasting client .env values all over the
	 *
	 * @param locals THe locals.
	 * @param clientId E.g: "github", "twitter", "etc..."
	 * @returns The client configuration
	 */
	public async getClientConfig(locals: App.Locals, clientId: string) {
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

		if (!data) throw new Error("Unable to client, is github active ?");
		return data.filter((i) => i.providerId === clientId).pop() ?? error(501);
	}

	private async requestToken(): Promise<void> {
		const response = await fetch(`${this.url}/protocol/openid-connect/token`, {
			method: "POST",
			headers: { "Content-Type": "application/x-www-form-urlencoded" },
			body: new URLSearchParams({
				client_id: this.clientId,
				client_secret: this.clientSecret,
				grant_type: "client_credentials",
			}),
		});

		if (!response.ok) throw new Error(response.statusText);
		const data = await response.json();
		this.token = data.access_token;
		this.refreshToken = data.refresh_token;
	}

	private async requestRefreshToken(): Promise<void> {
		if (!this.refreshToken)
			throw new Error(
				"No refresh token available. Configure your keycloak to allow sending refresh_tokens",
			);

		const response = await fetch(`${this.url}/protocol/openid-connect/token`, {
			method: "POST",
			headers: { "Content-Type": "application/x-www-form-urlencoded" },
			body: new URLSearchParams({
				client_id: this.clientId,
				client_secret: this.clientSecret,
				grant_type: "refresh_token",
				refresh_token: this.refreshToken,
			}),
		});

		const data = await response.json();
		if (!response.ok)
			throw new Error(`Unable to request refresh token: ${response.statusText}, ${JSON.stringify(data)}`);
		this.token = data.access_token;
		this.refreshToken = data.refresh_token;
	}

	public async getToken(): Promise<string> {
		if (!this.token) {
			await this.requestToken();
		} else {
			const tokenPayload = JSON.parse(
				Buffer.from(this.token.split(".")[1], "base64").toString(),
			);
			const expiry = tokenPayload.exp * 1000;
			if (Date.now() >= expiry) {
				await this.requestRefreshToken();
			}
		}
		return this.token!;
	}
}
