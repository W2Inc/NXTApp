import type { Icon } from "lucide-svelte";

export type IconLink = {
	icon: typeof Icon;
	title: string;
	href: string;
};
