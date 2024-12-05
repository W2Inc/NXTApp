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

	private async requestToken(): Promise<void> {
		const response = await fetch(
			`${this.url}/protocol/openid-connect/token`,
			{
				method: "POST",
				headers: { "Content-Type": "application/x-www-form-urlencoded" },
				body: new URLSearchParams({
					client_id: this.clientId,
					client_secret: this.clientSecret,
					grant_type: "client_credentials",
				}),
			},
		);

		if (!response.ok) throw new Error(response.statusText);
		const data = await response.json();
		this.token = data.access_token;
		this.refreshToken = data.refresh_token;
	}

	private async requestRefreshToken(): Promise<void> {
		if (!this.refreshToken) throw new Error("No refresh token available");

		const response = await fetch(
			`${this.url}/protocol/openid-connect/token`,
			{
				method: "POST",
				headers: { "Content-Type": "application/x-www-form-urlencoded" },
				body: new URLSearchParams({
					client_id: this.clientId,
					client_secret: this.clientSecret,
					grant_type: "refresh_token",
					refresh_token: this.refreshToken,
				}),
			},
		);

		if (!response.ok) throw new Error(`Unable to request refresh token: ${response.statusText}`);
		const data = await response.json();
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
