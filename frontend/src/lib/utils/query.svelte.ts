import { browser } from "$app/environment";
import { goto } from "$app/navigation";
import { noop } from "@tanstack/table-core";
import { SvelteURL, SvelteURLSearchParams } from "svelte/reactivity";
import type { z } from "zod";

export function useQuery<T extends z.ZodObject<any>>(initialUrl: string, schema: T) {
	let state = $state<Partial<z.infer<T>>>({});
	let url = $state(new SvelteURL(initialUrl));
	let params = $state(new SvelteURLSearchParams(url.search));

	function parseCurrentParams() {
		const parameters: Record<string, unknown> = {};
		params.forEach((value, key) => parameters[key] = value);

		try {
			return schema.parse(parameters);
		} catch (error) {
			return schema.partial().parse({});
		}
	}

	function updateState() {
		state = parseCurrentParams();
	}

	if (browser) {
		updateState();
		window.addEventListener("popstate", () => {
			url = new SvelteURL(window.location.href);
			params = new SvelteURLSearchParams(url.search);
			updateState();
		});
	}

	return {
		read<K extends keyof z.infer<T>>(key: K): z.infer<T>[K] | undefined {
			if (!browser) return undefined;
			return state[key];
		},
		write<K extends keyof z.infer<T>>(key: K, value: z.infer<T>[K]) {
			if (!browser) return;

			try {
				const fieldSchema = schema.shape[key as string];
				fieldSchema.parse(value);

				if (value === undefined || value === null) {
					params.delete(key as string);
				} else {
					params.set(key as string, encodeURIComponent(String(value)));
				}

				goto(`${url.pathname}?${params.toString()}`, {
					keepFocus: true,
					replaceState: true,
					noScroll: true,
				}).then(() => updateState());
			} catch (error) {
				throw new Error(`Invalid value for key "${String(key)}": ${error}`);
			}
		},
		readAll() {
			return state;
		},
	};
}
