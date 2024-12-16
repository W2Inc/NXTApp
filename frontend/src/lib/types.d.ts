import type { Icon } from "lucide-svelte";

export type NamedLink = {
	title: string;
	href: string;
}

export interface IconLink extends NamedLink {
	icon: typeof Icon;
}
