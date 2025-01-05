<script lang="ts">
  import { Handle, Position, type NodeProps } from '@xyflow/svelte';
	import SearchProject from '../search-project.svelte';

  type $$Props = NodeProps;

  export let data: $$Props['data'];
  export let isConnectable: $$Props['isConnectable'];

  const { color } = data;
</script>

<Handle type="target" position={Position.Left} style="background: #555;" {isConnectable} />

<SearchProject />

<Handle
  type="source"
  position={Position.Right}
  id="a"
  style="top: 10px; background: #555;"
  {isConnectable}
/>
<Handle
  type="source"
  position={Position.Right}
  id="b"
  style="bottom: 10px; top: auto; background: #555;"
  {isConnectable}
/>

<style>
  :global(.svelte-flow__node-selectorNode) {
    font-size: 12px;
    border: 1px solid #555;
    border-radius: 5px;
    text-align: center;
  }
</style>
