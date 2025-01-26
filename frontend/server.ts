import * as Bun from "bun";

Bun.serve({
	port: process.env.PORT || 443,
	hostname: process.env.HOST || "prov2.schiphol.nl",
	serverName: "prov2.schiphol.nl",
	fetch(req, server) {
		const ip = server.requestIP(req);
		return new Response(`Hello user! Your IP: ${ip}`, {
			headers: {
				"Server": "Bun 1.2.0"
			}
		});
	},

	tls: {
		key: Bun.file("./key.pem"),
		cert: Bun.file("./cert.pem"),
		passphrase: "1nnc6ZR5KycZMKxNp2LT"
	},
});
