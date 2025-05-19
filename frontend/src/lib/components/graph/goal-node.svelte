<script lang="ts">
	import { Handle, Position, type NodeProps, type Node } from '@xyflow/svelte';

	// Pass data to the node via this interface instead...
	interface GoalNodeData extends Record<string, unknown> {
		direction: Position;
		parentPosition: [number, number] // X, Y
	}

	interface Props extends Omit<NodeProps<Node>, 'data'> {
		data: GoalNodeData
	}

	let { data }: Props = $props();
	const isRoot = $derived(data.node.id === 0);
</script>

<div class="bg-muted rounded-full p-4 shadow-lg border-2 border-primary text-white flex items-center justify-center min-w-[150px] min-h-[150px]">
	<div class="text-center">
		<h3 class="font-bold mb-1">Goal</h3>
		<p class="text-sm opacity-90">{ data.node.id }</p>
	</div>
	{#if isRoot}
		<Handle id="t" type="source" position={Position.Top} class="!bg-white" />
		<Handle id="r" type="source" position={Position.Right} class="!bg-white" />
		<Handle id="b" type="source" position={Position.Bottom} class="!bg-white" />
		<Handle id="l" type="source" position={Position.Left} class="!bg-white" />
	{:else if data.direction === Position.Top}
		<!-- Node is progressing "up" meaning the bottom is the target the rest are sources -->
		<Handle id="t" type="source" position={Position.Top} class="!bg-white" />
		<Handle id="r" type="source" position={Position.Right} class="!bg-white" />
		<Handle id="l" type="source" position={Position.Left} class="!bg-white" />
		<Handle id="b" type="target" position={Position.Bottom} class="!bg-white" />
	{:else if data.direction === Position.Right}
		<!-- Node is progressing to the "right" -->
		<Handle id="t" type="source" position={Position.Top} class="!bg-white" />
		<Handle id="r" type="source" position={Position.Right} class="!bg-white" />
		<Handle id="b" type="source" position={Position.Bottom} class="!bg-white" />
		<Handle id="l" type="target" position={Position.Left} class="!bg-white" />
	{:else if data.direction === Position.Bottom}
		<!-- Node is progressing "down" -->
		<Handle id="r" type="source" position={Position.Right} class="!bg-white" />
		<Handle id="b" type="source" position={Position.Bottom} class="!bg-white" />
		<Handle id="l" type="source" position={Position.Left} class="!bg-white" />
		<Handle id="t" type="target" position={Position.Top} class="!bg-white" />
	{:else if data.direction === Position.Left}
		<!-- Node is progressing to the "left" -->
		<Handle id="t" type="source" position={Position.Top} class="!bg-white" />
		<Handle id="b" type="source" position={Position.Bottom} class="!bg-white" />
		<Handle id="l" type="source" position={Position.Left} class="!bg-white" />
		<Handle id="r" type="target" position={Position.Right} class="!bg-white" />
	{:else}
		<svelte:boundary>Missing Direction</svelte:boundary>
	{/if}
</div>
