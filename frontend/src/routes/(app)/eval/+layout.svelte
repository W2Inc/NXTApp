<script lang="ts">
	import * as Select from "$lib/components/ui/select/index.js";
	import { Button } from "$lib/components/ui/button/index.js";
	import { Input } from "$lib/components/ui/input/index.js";
	import { Search } from "lucide-svelte";

	let { children, data } = $props();

	// Status options
	const statusOptions = [
		{ value: "", label: "All" },
		{ value: "pending", label: "Pending" },
		{ value: "in-progress", label: "In Progress" },
		{ value: "completed", label: "Completed" },
		{ value: "failed", label: "Failed" },
	];

	// Sort options
	const sortOptions = [
		{ value: "newest", label: "Newest first" },
		{ value: "oldest", label: "Oldest first" },
		{ value: "name-asc", label: "Name A-Z" },
	];

	let statusValue = $state("");
	let sortValue = $state("newest");

	const statusTriggerContent = $derived(
		statusOptions.find((opt) => opt.value === statusValue)?.label ?? "Status: All",
	);

	const sortTriggerContent = $derived(
		sortOptions.find((opt) => opt.value === sortValue)?.label ?? "Sort: Newest first",
	);
</script>

{@render children?.()}
