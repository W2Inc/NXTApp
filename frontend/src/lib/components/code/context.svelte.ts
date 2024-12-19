export type TreeItem = {
	id: string;
	name: string;
	type: "file" | "root";
	children: TreeItem[];
};

let tree: TreeItem = $state({
	id: "1",
	name: "Root",
	type: "root",
	children: [
		{
			id: "2",
			name: "Hierarchy A",
			type: "file",
			children: [
				{ id: "9", name: "Hierarchy E", type: "file", children: [] },
				{ id: "10", name: "Hierarchy F", type: "file", children: [] },
				{ id: "11", name: "Hierarchy G", type: "file", children: [] },
			],
		},
		{ id: "3", name: "Hierarchy B", type: "file", children: [] },
		{ id: "4", name: "Hierarchy C", type: "file", children: [] },
		{
			id: "5",
			name: "Hierarchy D",
			type: "file",
			children: [
				{ id: "6", name: "Hierarchy H", type: "file", children: [] },
				{ id: "7", name: "Hierarchy I", type: "file", children: [] },
				{ id: "8", name: "Hierarchy J", type: "file", children: [] },
			],
		},
	],
});

export function useTreeState() {
	// Handler for drag start
	const dragstartHandler = (ev: DragEvent, itemId: string) => {
		ev.dataTransfer?.setData("text/plain", itemId);
	};

	// Handler for drag over
	const dragoverHandler = (ev: DragEvent) => {
		ev.preventDefault(); // Allow the drop
	};

	// Handler for drop
	const dropHandler = (ev: DragEvent, targetItem: TreeItem) => {
		ev.preventDefault();
		ev.stopPropagation();

		const draggedItemId = ev.dataTransfer?.getData("text/plain");
		if (!draggedItemId) return;

		const sourceItem = findItemById(tree, draggedItemId);
		if (!sourceItem) return;

		moveItem(sourceItem, targetItem);
	};

	// Recursive helper to find an item by ID in the tree
	const findItemById = (tree: TreeItem, id: string): TreeItem | null => {
		if (tree.id === id) return tree;

		for (const child of tree.children) {
			const foundItem = findItemById(child, id);
			if (foundItem) return foundItem;
		}

		return null;
	};

	// Helper to move a source item to a target location
	const moveItem = (source: TreeItem, target: TreeItem) => {
		if (source.id === target.id) return;

		// Recursively remove the source item from its current position
		const removeItemFromTree = (tree: TreeItem, itemId: string): boolean => {
			const index = tree.children.findIndex((child) => child.id === itemId);
			if (index !== -1) {
				tree.children.splice(index, 1);
				return true;
			}

			return tree.children.some((child) => removeItemFromTree(child, itemId));
		};

		removeItemFromTree(tree, source.id);
		target.children.push(source);
	};

	return {
		dragstartHandler,
		dragoverHandler,
		dropHandler,
		tree,
	};
}
