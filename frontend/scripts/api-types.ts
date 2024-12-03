import { $ } from "bun";

await $`bun --bun run keycloak:types`;
await $`bun --bun run backend:types`;
process.exit(0);
