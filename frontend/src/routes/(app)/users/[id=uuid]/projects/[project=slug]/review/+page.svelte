<script>
	const reviewTypes = [
		{
			id: "auto",
			name: "Auto Review",
			icon: "🤖",
			description:
				"Get instant feedback on your project using AI and automated testing tools.",
		},
		{
			id: "self",
			name: "Self Review",
			icon: "👤",
			description:
				"Review your own project using guided questions and reflection prompts.",
		},
		{
			id: "async",
			name: "Async Review",
			icon: "⏱️",
			description: "Submit your project for review by someone at their convenience.",
		},
		{
			id: "peer",
			name: "Peer Review",
			icon: "👥",
			description: "Get direct feedback from peers in your field.",
		},
	];

	// Define rubrics for each review type
	const rubrics = {
		auto: [
			{
				id: "code-quality",
				name: "Code Quality Assessment",
				description: "Evaluates code cleanliness, efficiency, and organization.",
			},
			{
				id: "auto-tests",
				name: "Automated Tests",
				description: "Runs test suite to verify functionality.",
			},
			{
				id: "ai-feedback",
				name: "AI Code Review",
				description: "Uses AI to analyze code patterns and suggest improvements.",
			},
		],
		self: [
			{
				id: "self-reflection",
				name: "Self-Reflection Guide",
				description: "Structured questions to evaluate your own work.",
			},
			{
				id: "goals-assessment",
				name: "Goals Assessment",
				description: "Evaluate if project meets your initial goals.",
			},
		],
		async: [
			{
				id: "comprehensive",
				name: "Comprehensive Review",
				description: "In-depth analysis of all aspects of your project.",
			},
			{
				id: "specific-focus",
				name: "Specific Focus Areas",
				description: "Request feedback on particular components or features.",
			},
		],
		peer: [
			{
				id: "collaborative",
				name: "Collaborative Feedback",
				description: "Group assessment with multiple peer reviewers.",
			},
			{
				id: "expert-review",
				name: "Expert Domain Review",
				description: "Focused feedback from specialists in your field.",
			},
		],
	};

	let selectedReviewType = reviewTypes[0].id;
	// Track selections for each review type
	let selectedRubricsByType = {
		auto: new Set(),
		self: new Set(),
		async: new Set(),
		peer: new Set()
	};

	$: currentRubrics = rubrics[selectedReviewType] || [];

	// Toggle selection of a rubric
	function toggleRubricSelection(rubric) {
		const currentSelections = new Set(selectedRubricsByType[selectedReviewType]);
		if (currentSelections.has(rubric.id)) {
			currentSelections.delete(rubric.id);
		} else {
			currentSelections.add(rubric.id);
		}

		selectedRubricsByType = {
			...selectedRubricsByType,
			[selectedReviewType]: currentSelections
		};
	}

	// Check if a rubric is selected
	function isRubricSelected(rubricId) {
		return selectedRubricsByType[selectedReviewType].has(rubricId);
	}

	// Check if at least one from each review type is selected
	$: hasAllRequired = Object.entries(selectedRubricsByType).every(([_, set]) => set.size > 0);
</script>

<div class="flex h-screen">
	<!-- Left sidebar for review type selection -->
	<div class="flex w-64 flex-col border-r">
		<div class="border-b p-4">
			<h2 class="text-xl font-semibold">Review Types</h2>
		</div>
		<nav class="flex-1 overflow-y-auto p-2">
			{#each reviewTypes as reviewType}
				<button
					class="mb-2 flex w-full items-center space-x-3 rounded-lg p-3 text-left
							 {selectedReviewType === reviewType.id ? 'bg-primary text-white' : 'hover:bg-muted'}"
					on:click={() => (selectedReviewType = reviewType.id)}
				>
					<div
						class="bg-primary/10 flex h-8 w-8 items-center justify-center rounded-full"
					>
						<span>{reviewType.icon}</span>
					</div>
					<span>{reviewType.name}</span>
					{#if selectedRubricsByType[reviewType.id].size > 0}
						<span class="ml-auto bg-green-500 text-white rounded-full w-6 h-6 flex items-center justify-center text-xs">
							{selectedRubricsByType[reviewType.id].size}
						</span>
					{/if}
				</button>
			{/each}
		</nav>
	</div>

	<!-- Right content area for rubrics -->
	<div class="flex flex-1 flex-col">
		<div class="border-b p-6 flex justify-between items-center">
			<div>
				<h1 class="text-2xl font-bold">
					{reviewTypes.find((rt) => rt.id === selectedReviewType)?.name || ""} Rubrics
				</h1>
				<p class="mt-2 text-slate-500">Select at least one rubric from each review type</p>
			</div>

			<button
				class="bg-primary rounded-md px-4 py-2 text-white disabled:opacity-50"
				disabled={!hasAllRequired}
			>
				Continue with Selected Rubrics
			</button>
		</div>

		<div class="flex-1 overflow-y-auto p-6">
			<div class="grid grid-cols-1 gap-6 md:grid-cols-2">
				{#each currentRubrics as rubric}
					<div
						class="bg-card rounded-lg border shadow-sm transition-shadow hover:shadow-md
								{isRubricSelected(rubric.id) ? 'ring-primary ring-2' : ''}"
						on:click={() => toggleRubricSelection(rubric)}
					>
						<div class="flex flex-col space-y-4 p-6">
							<h3 class="text-lg font-semibold">{rubric.name}</h3>
							<p class="text-muted-foreground text-sm">{rubric.description}</p>
							<button class="bg-primary rounded-md px-4 py-2 text-white">
								{isRubricSelected(rubric.id) ? "Selected" : "Select"}
							</button>
						</div>
					</div>
				{/each}
			</div>

			<!-- Summary of selected rubrics -->
			<div class="mt-8 rounded-lg border p-6">
				<h2 class="text-xl font-semibold mb-4">Your Selected Rubrics</h2>

				{#each reviewTypes as reviewType}
					{#if selectedRubricsByType[reviewType.id].size > 0}
						<div class="mb-4">
							<h3 class="text-lg font-medium mb-2">{reviewType.name}</h3>
							<ul class="ml-4 space-y-1">
								{#each Array.from(selectedRubricsByType[reviewType.id]) as rubricId}
									{@const rubric = rubrics[reviewType.id].find(r => r.id === rubricId)}
									{#if rubric}
										<li class="flex items-center gap-2">
											<span class="text-primary">✓</span>
											<span class="font-medium">{rubric.name}</span>
										</li>
									{/if}
								{/each}
							</ul>
						</div>
					{/if}
				{/each}

				{#if !hasAllRequired}
					<div class="mt-4 text-amber-600">
						<p>Please select at least one rubric from each review type to continue.</p>
					</div>
				{/if}
			</div>
		</div>

		<div class="border-t p-6">
			<div class="flex justify-end">
				<button class="mr-3 rounded-md bg-slate-200 px-4 py-2 hover:bg-slate-300">
					Cancel
				</button>
			</div>
		</div>
	</div>
</div>
