// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

type Config = { delay?: number };
type ValidCopy = string | number;

const copyMethods: Array<(str: string) => Promise<void>> = [
	async (str) => {
		await navigator.clipboard.writeText(str);
	},
	async (str) => {
		const textarea = document.createElement("textarea");
		textarea.value = str;
		textarea.style.position = "fixed";
		textarea.style.opacity = "0";
		document.body.appendChild(textarea);
		textarea.focus();
		textarea.select();
		document.execCommand("copy");
		document.body.removeChild(textarea);
	},
];

// ============================================================================

const useClipboard = (copyString: ValidCopy, config: Config = {}) => {
	const { delay = 1000 } = config;

	let lastCopied = $state<number | null>(null);
	let copied = $derived(lastCopied !== null);

	const copyToClipboard = async (overwriteCopyStr: ValidCopy | undefined) => {
		const strToCopy = String(
			overwriteCopyStr === undefined ? copyString : overwriteCopyStr,
		);

		for (const method of copyMethods) {
			try {
				await method(strToCopy);
				return;
			} catch (error) {
				console.error("Copy method failed:", error);
			}
		}
		throw new Error("Copy failed, browser not supported.");
	};

	const copy = async (newCopyString: ValidCopy | undefined) => {
		if (!["string", "number"].includes(typeof newCopyString)) {
			throw new Error("Invalid copy type: Only string and number are supported.");
		}

		const time = Date.now();
		lastCopied = time;
		await copyToClipboard(newCopyString);
		await new Promise((res) => setTimeout(res, delay));
		if (time !== lastCopied) return;
		lastCopied = null;
	};

	return {
		get copied() {
			return copied;
		},
		copy,
	};
};

export default useClipboard;
