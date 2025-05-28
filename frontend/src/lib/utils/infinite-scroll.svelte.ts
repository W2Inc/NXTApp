import type { ActionReturn } from "svelte/action";

interface InfiniteScrollOptions {
	/** Callback function to execute when more content should be loaded */
	onLoadMore: () => void;
	/** Root margin for the intersection observer (default: '100px') */
	rootMargin?: string;
	/** Threshold for triggering the callback (default: 0.1) */
	threshold?: number;
	/** Whether infinite scroll is enabled (default: true) */
	enabled?: boolean;
}

export function infiniteScroll(
	node: HTMLElement,
	options: InfiniteScrollOptions
): ActionReturn<InfiniteScrollOptions> {
	let observer: IntersectionObserver | null = null;

	function createObserver(opts: InfiniteScrollOptions) {
		// Clean up existing observer
		if (observer) {
			observer.disconnect();
		}

		if (!opts.enabled) return;

		observer = new IntersectionObserver(
			(entries) => {
				entries.forEach((entry) => {
					if (entry.isIntersecting) {
						opts.onLoadMore();
					}
				});
			},
			{
				rootMargin: opts.rootMargin || '100px',
				threshold: opts.threshold || 0.1
			}
		);

		observer.observe(node);
	}

	// Initialize observer
	createObserver(options);

	return {
		update(newOptions: InfiniteScrollOptions) {
			createObserver(newOptions);
		},
		destroy() {
			if (observer) {
				observer.disconnect();
			}
		}
	};
}
