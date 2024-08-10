<script lang="ts">
	export let href = "#";
	export let width: number | string | undefined = undefined;
	export let height: number | string | undefined = undefined;
	export let src: string | undefined = undefined;
	export let alt: string | undefined = undefined;
	export let inert: boolean = false;
</script>

<a {href} {inert} >
	<img {src} {alt} {width} {height}/>
</a>

<style>
	a {
		display: flex;
		align-items: flex-start;
		flex-shrink: 0;

		& img {
			border-radius: var(--br);
		}
	}
</style>
