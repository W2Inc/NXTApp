<script lang="ts">
	import * as Avatar from "$lib/components/ui/avatar";
	import { encodeID } from "$lib/utils";
	import FormatDate from "$lib/utils/date.svelte";
	import ArrowRight from "lucide-svelte/icons/arrow-right";

	interface Props {
		review: BackendTypes["ReviewDO"];
	}

	// ReviewDO: {
	//           /** Format: uuid */
	//           id?: string;
	//           /** Format: date-time */
	//           createdAt?: string;
	//           /** Format: date-time */
	//           updatedAt?: string;
	//           kind?: components["schemas"]["ReviewKind"];
	//           state?: components["schemas"]["ReviewState"];
	//           validated?: boolean;
	//           reviewer?: components["schemas"]["MinimalUserDTO"];
	//           /** Format: uuid */
	//           userProjectId?: string;
	//       };

	//           /** Format: uuid */
	//           id?: string;
	//           /** Format: date-time */
	//           createdAt?: string;
	//           /** Format: date-time */
	//           updatedAt?: string;
	//           login?: string;
	//           displayName?: string | null;
	//           avatarUrl?: string | null;
	//           /** Format: uuid */
	//           detailsId?: string | null;

	let { review }: Props = $props();

	review = {
		id: "0231be48-b57a-47f5-9421-f53557931fa6",
		createdAt: "2024-01-15T10:30:00Z",
		updatedAt: "2024-01-16T15:45:00Z",
		kind: "Self",
		state: "Finished",
		validated: true,
		reviewer: {
			id: "123",
			login: "Wizard",
			displayName: "Wizard",
			avatarUrl: "https://github.com/w2wizard.png",
		},
	};
</script>

<li>
	<a
		href="/reviews/{encodeID(review.id ?? '')}"
		class="center-content group cursor-pointer justify-between rounded border bg-muted p-2 pr-4 shadow-sm dark:border-none"
	>
		<div class="center-content">
			<Avatar.Root class="rounded">
				<Avatar.Image src="https://github.com/shadcn.png" alt="@shadcn" />
				<Avatar.Fallback>CN</Avatar.Fallback>
			</Avatar.Root>
			<div class="item flex flex-col justify-center">
				<div class="center-content">
					<span class="font-bold">{review.reviewer?.displayName}</span>
					<span class="text-[0.85em]">commented</span>
				</div>
				<span class="text-xs text-muted-foreground">
					{FormatDate(review.createdAt ?? 0)}
				</span>
			</div>
		</div>
		<ArrowRight class="group-hover:translate-x-1 motion-safe:transition-transform" />
	</a>
</li>
