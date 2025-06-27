<script>
	import HourGlass from "lucide-svelte/icons/hourglass";
	import { onMount } from "svelte";
	import Card from "./ui/card/card.svelte";

	const funFacts = [
		"The first computer bug was an actual moth found in a Harvard computer in 1947.",
		"The average person spends 6 months of their lifetime waiting at traffic lights.",
		"A group of flamingos is called a 'flamboyance'.",
		"Honey never spoils. Archaeologists have found pots of honey in ancient Egyptian tombs that are over 3,000 years old.",
		"The Eiffel Tower can be 15 cm taller during the summer due to thermal expansion.",
		"A day on Venus is longer than a year on Venus.",
		"The shortest war in history was between Britain and Zanzibar in 1896, lasting only 38 minutes.",
		"Octopuses have three hearts and blue blood.",
		"The first message sent over the internet was 'LO'. It was supposed to be 'LOGIN' but the system crashed.",
		"10% of the world's population is left-handed.",
	];

	let index = $state(0);
	const currentFact = $derived(funFacts[index]);

	const useIndex = () => (index + 1) % funFacts.length;

	$effect(() => {
		const interval = setInterval(() => (index = useIndex()), 2000);
		return () => clearInterval(interval);
	});
</script>

<Card class="flex flex-col items-center justify-center space-y-4 p-6 text-center">
	<div class="max-w-md">
		<div class="mt-2 flex min-h-16 items-center justify-center">
			<p class="text-base transition-opacity duration-300">{currentFact}</p>
		</div>
		<p
			class="text-muted-foreground flex animate-pulse items-center justify-center gap-2 text-sm font-medium"
		>
			Loading...
			<HourGlass class="h-4 w-4 animate-spin" />
		</p>
	</div>
</Card>
