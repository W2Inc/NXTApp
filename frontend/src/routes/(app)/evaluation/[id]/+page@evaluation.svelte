<script lang="ts">
	import Base from "$lib/components/base.svelte";
	import { useTreeState } from "$lib/components/code/context.svelte.js";
	import Explorer from "$lib/components/code/explorer.svelte";
	import Markdown from "$lib/components/markdown/markdown.svelte";
	import Separator from "$lib/components/ui/separator/separator.svelte";
	import * as Menubar from "$lib/components/ui/menubar";

	const { data } = $props();
	let bookmarks = $state(false);
	let fullUrls = $state(true);
	let profileRadioValue = $state("benoit");
</script>

<Base variant="splitpane">
	{#snippet left()}
		<div class="max-h-dvh overflow-auto">
			<div class="sticky top-0 bg-background z-10">
				<h1 class="px-4 py-2 text-2xl">W2Wizard Evaluation</h1>
				<Separator />
				<Menubar.Root class="border-none px-4">
					<Menubar.Menu>
						<Menubar.Trigger>File</Menubar.Trigger>
						<Menubar.Content>
							<Menubar.Item>
								New Tab <Menubar.Shortcut>⌘T</Menubar.Shortcut>
							</Menubar.Item>
							<Menubar.Item>
								New Window <Menubar.Shortcut>⌘N</Menubar.Shortcut>
							</Menubar.Item>
							<Menubar.Item>New Incognito Window</Menubar.Item>
							<Menubar.Separator />
							<Menubar.Sub>
								<Menubar.SubTrigger>Share</Menubar.SubTrigger>
								<Menubar.SubContent>
									<Menubar.Item>Email link</Menubar.Item>
									<Menubar.Item>Messages</Menubar.Item>
									<Menubar.Item>Notes</Menubar.Item>
								</Menubar.SubContent>
							</Menubar.Sub>
							<Menubar.Separator />
							<Menubar.Item>
								Print... <Menubar.Shortcut>⌘P</Menubar.Shortcut>
							</Menubar.Item>
						</Menubar.Content>
					</Menubar.Menu>
					<Menubar.Menu>
						<Menubar.Trigger>Edit</Menubar.Trigger>
						<Menubar.Content>
							<Menubar.Item>
								Undo <Menubar.Shortcut>⌘Z</Menubar.Shortcut>
							</Menubar.Item>
							<Menubar.Item>
								Redo <Menubar.Shortcut>⇧⌘Z</Menubar.Shortcut>
							</Menubar.Item>
							<Menubar.Separator />
							<Menubar.Sub>
								<Menubar.SubTrigger>Find</Menubar.SubTrigger>
								<Menubar.SubContent>
									<Menubar.Item>Search the web</Menubar.Item>
									<Menubar.Separator />
									<Menubar.Item>Find...</Menubar.Item>
									<Menubar.Item>Find Next</Menubar.Item>
									<Menubar.Item>Find Previous</Menubar.Item>
								</Menubar.SubContent>
							</Menubar.Sub>
							<Menubar.Separator />
							<Menubar.Item>Cut</Menubar.Item>
							<Menubar.Item>Copy</Menubar.Item>
							<Menubar.Item>Paste</Menubar.Item>
						</Menubar.Content>
					</Menubar.Menu>
					<Menubar.Menu>
						<Menubar.Trigger>View</Menubar.Trigger>
						<Menubar.Content>
							<Menubar.CheckboxItem bind:checked={bookmarks}
								>Always Show Bookmarks Bar</Menubar.CheckboxItem
							>
							<Menubar.CheckboxItem bind:checked={fullUrls}>
								Always Show Full URLs
							</Menubar.CheckboxItem>
							<Menubar.Separator />
							<Menubar.Item inset>
								Reload <Menubar.Shortcut>⌘R</Menubar.Shortcut>
							</Menubar.Item>
							<Menubar.Item inset>
								Force Reload <Menubar.Shortcut>⇧⌘R</Menubar.Shortcut>
							</Menubar.Item>
							<Menubar.Separator />
							<Menubar.Item inset>Toggle Fullscreen</Menubar.Item>
							<Menubar.Separator />
							<Menubar.Item inset>Hide Sidebar</Menubar.Item>
						</Menubar.Content>
					</Menubar.Menu>
					<Menubar.Menu>
						<Menubar.Trigger>Profiles</Menubar.Trigger>
						<Menubar.Content>
							<Menubar.RadioGroup bind:value={profileRadioValue}>
								<Menubar.RadioItem value="andy">Andy</Menubar.RadioItem>
								<Menubar.RadioItem value="benoit">Benoit</Menubar.RadioItem>
								<Menubar.RadioItem value="Luis">Luis</Menubar.RadioItem>
							</Menubar.RadioGroup>
							<Menubar.Separator />
							<Menubar.Item inset>Edit...</Menubar.Item>
							<Menubar.Separator />
							<Menubar.Item inset>Add Profile...</Menubar.Item>
						</Menubar.Content>
					</Menubar.Menu>
				</Menubar.Root>
				<Separator />
			</div>

			<div class="p-4">
				<Markdown value={data.rubric} variant="viewer" />
			</div>
		</div>
	{/snippet}
	{#snippet right()}
		{#await data.code}
			<p>Loading...</p>
		{:then code}
		<!-- <iframe src='https://view.officeapps.live.com/op/embed.aspx?src=http%3A%2F%2Fieee802%2Eorg%3A80%2Fsecmail%2FdocIZSEwEqHFr%2Edoc' width='100%' height='100%' frameborder='0'>This is an embedded <a target='_blank' href='http://office.com'>Microsoft Office</a> document, powered by <a target='_blank' href='http://office.com/webapps'>Office Online</a>.</iframe> -->
		<embed class="h-dvh" src="/dummy.pdf" width="100%" />
			<!-- {@html code} -->
		{/await}
		<!-- TODO: Explorer with file tree -->
		<!-- <iframe src="http://docs.google.com/gview?url=https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf"
			style="width:600px; height:500px;" frameborder="0"></iframe> -->

		<!-- <Explorer item={tree} /> -->
	{/snippet}
</Base>
