<script lang="ts">
	import Button from "$lib/components/ui/button/button.svelte";
	import Separator from "$lib/components/ui/separator/separator.svelte";
	import * as Table from "$lib/components/ui/table/";
	import Logout from "lucide-svelte/icons/log-out";

	const { data } = $props();

	const dateFormat = new Intl.DateTimeFormat("en-US", {
		year: "numeric",
		month: "short",
		day: "2-digit",
		hour: "2-digit",
		minute: "2-digit",
		second: "2-digit",
	});

	const invoices = [
		{
			invoice: "INV001",
			paymentStatus: "Paid",
			totalAmount: "$250.00",
			paymentMethod: "Credit Card",
		},
		{
			invoice: "INV002",
			paymentStatus: "Pending",
			totalAmount: "$150.00",
			paymentMethod: "PayPal",
		},
		{
			invoice: "INV003",
			paymentStatus: "Unpaid",
			totalAmount: "$350.00",
			paymentMethod: "Bank Transfer",
		},
		{
			invoice: "INV004",
			paymentStatus: "Paid",
			totalAmount: "$450.00",
			paymentMethod: "Credit Card",
		},
		{
			invoice: "INV005",
			paymentStatus: "Paid",
			totalAmount: "$550.00",
			paymentMethod: "PayPal",
		},
		{
			invoice: "INV006",
			paymentStatus: "Pending",
			totalAmount: "$200.00",
			paymentMethod: "Bank Transfer",
		},
		{
			invoice: "INV007",
			paymentStatus: "Unpaid",
			totalAmount: "$300.00",
			paymentMethod: "Credit Card",
		},
	];
</script>

<h1 class="text-2xl">Current Sessions</h1>
<p class="text-muted-foreground text-sm">These are all your currently active sessions.</p>
<Separator class="my-2" />
<Table.Root>
	<Table.Header>
		<Table.Row>
			<Table.Head class="w-[100px]">IP</Table.Head>
			<Table.Head>Login At</Table.Head>
			<Table.Head>Last Access</Table.Head>
			<Table.Head class="text-right">Action</Table.Head>
		</Table.Row>
	</Table.Header>
	<Table.Body>
		{#each data.sessions as session, i}
			<Table.Row>
				<Table.Cell class="font-medium">{session.ipAddress}</Table.Cell>
				<Table.Cell>{dateFormat.format(session.start)}</Table.Cell>
				<Table.Cell>{dateFormat.format(session.lastAccess)}</Table.Cell>
				<Table.Cell class="text-right">
					<form method="POST" enctype="multipart/form-data" action="?/delete">
						<input name="id" type="text" readonly required value={session.id} hidden />
						<Button type="submit" size="sm" variant="destructive">
							<Logout />
							Logout
						</Button>
					</form>
				</Table.Cell>
			</Table.Row>
		{/each}
	</Table.Body>
	<Table.Footer>
		<Table.Row>
			<Table.Cell colspan={3}>
				Session count: {data.sessions.length}
			</Table.Cell>
			<Table.Cell class="text-right">
				<form method="POST" enctype="multipart/form-data" action="?/deleteAll">
					<Button size="sm" variant="destructive" type="submit">
						<Logout />
						Logout all sessions
					</Button>
				</form>
			</Table.Cell>
		</Table.Row>
	</Table.Footer>
</Table.Root>
