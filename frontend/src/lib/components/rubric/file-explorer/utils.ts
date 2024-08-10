import type { DirectoryStub, Stub } from "$lib/types";
import { files, selectedName } from "./state";

export function get_depth(name: string) {
	const len = name.split('/').length - 1;
	console.log(name, "LEN:", len);
	return len;
}

export function resetFiles(newFiles: Stub[]) {
	selectedName.update(($selectedName) => {
		const file = newFiles.find((file) => file.name === $selectedName);
		return file?.name ?? null;
	});

	files.set(newFiles);
}

/**
 * @param {string} name
 * @param {import('$lib/types').Stub[]} files
 */
export function createDirs(name: string, files: Stub[]) {
	const existing = new Set();

	for (const file of files) {
		if (file.type === 'directory') {
			existing.add(file.name);
		}
	}

	const directories: DirectoryStub[] = [];

	const parts = name.split('/');
	while (parts.length) {
		parts.pop();

		const dir = parts.join('/');
		if (existing.has(dir)) {
			break;
		}

		directories.push({
			type: 'directory',
			name: dir,
			basename: parts.at(-1) as string
		});
	}

	return directories;
}
