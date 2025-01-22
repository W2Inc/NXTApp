import { toast } from "svelte-sonner";
import type { Action } from "svelte/action";

interface ImagePreviewProps {
	input: HTMLInputElement;
	maxSize?: number;
	allowedTypes?: string[];
}

/** use:action to handle image previewing */
export const preview: Action<HTMLImageElement, ImagePreviewProps> = (
	node,
	{ input, maxSize = 5, allowedTypes = ["image/png", "image/jpeg", "image/gif"] },
) => {
	const controller = new AbortController();
	if (!input || input.type !== "file") {
		console.error("Image preview action requires an input file element reference");
		return;
	}

	input.title = "Upload a image";
	input.accept = allowedTypes.join(",");
	$effect(() => {
		const onChange = (e: Event) => {
			if (!e.target || !(e.target instanceof HTMLInputElement) || !e.target.files) {
				return toast.error("Failed to show image preview");
			}

			const file = e.target.files[0];
			if (!file || file.size > maxSize * 1024 * 1024) {
				e.target.value = "";
				return toast.error(`File too large! Maximum size is ${maxSize}MB`);
			}

			if (!allowedTypes.includes(file.type)) {
				e.target.value = "";
				return toast.error(`Invalid file type!`);
			}

			const reader = new FileReader();
			reader.addEventListener("load", () => (node.src = reader.result as string));
			reader.readAsDataURL(file);
		};

		input.addEventListener("change", onChange, {
			signal: controller.signal,
		});

		return () => controller.abort();
	});
};
