// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import remarkMath from "remark-math";
import rehypeKatex from "rehype-katex";
import rehypeHighlight from "rehype-highlight";
import remarkFrontmatter from "remark-frontmatter";
import remarkYamlConfig from "remark-yaml-config";
import remarkMentions from 'remark-mentions'
import { gfmPlugin as exGFM } from "svelte-exmarkdown/gfm";
import type { HastNode, Plugin } from "svelte-exmarkdown/types";

// ============================================================================

/**
 * A plugin to parse github flavored markdown.
 * @param options - Options to pass to remark-gfm.
 * @returns A plugin to parse github flavored markdown.
 *
 * @note Re-exports from svelte-exmarkdown/gfm.
 */
export const gfmPlugin = exGFM;

/**
 * A plugin to parse math stuff in markdown.
 * - Plugins used:
 *  - remark-math: For parsing math in markdown.
 *  - rehype-katex: For rendering math in html.
 * @param options
 * @returns
 */
export const mathPlugin = (
	options: Parameters<typeof remarkMath>[0] = {}
): Plugin => ({
	remarkPlugin: [remarkMath, options],
	rehypePlugin: [rehypeKatex],
});

/**
 * Code syntax highlighting plugin.
 * @param options - Options to pass to rehype-highlight.
 * @returns A plugin to highlight code syntax.
 */
export const highlightPlugin = (
	options: Parameters<typeof rehypeHighlight>[0] = {}
): Plugin => ({
	rehypePlugin: [rehypeHighlight, options],
});

/**
 * Frontmatter plugin for parsing frontmatter.
 * @param options - Options to pass to remark-frontmatter.
 * @returns A plugin to parse frontmatter.
 */
export const metaDataPlugin = (
	options: Parameters<typeof remarkFrontmatter>[0] = undefined
): Plugin => ({
	remarkPlugin: [remarkFrontmatter, options],
});

export const metaParserPlugin = (
	options: Parameters<typeof remarkFrontmatter>[0] = undefined
): Plugin => ({
	remarkPlugin: [remarkFrontmatter, options],
});

// TODO: Where the fuck is this going ? How do I access this shit?
export const yamlMatterPlugin = (): Plugin => ({
	remarkPlugin: [remarkYamlConfig],
});

export const mentionsPlugin = (
	options: Parameters<typeof remarkMentions>[0] = undefined
): Plugin => ({
	//@ts-ignore
	remarkPlugin: [remarkMentions, options],
});

// Find and replace keywords in the rubric

/**
 * Parse the markdown file as a NXDM-Rubric
 * NXDM Rubric should be designed with a spec in mind atm, we yolo it.
 *
 * # Did user do X?
 * Users should have done xyz la la la bla bla bla
 * <Toggle/>
 *
 * <Decision>
 * 	<Pass />
 *  <MemoryLeak />
 *  <Failed Compile />
 * </Decision>
 * @returns
 */
export const rubricPlugin = (): Plugin => ({
	remarkPlugin: [(e) => {
		console.log("HI!", e)
	}],
	rehypePlugin: [(e) => {
		console.log("HO", e)
	}]
});

// ============================================================================

/** A list of plugins to parse markdown. */
export default [
	gfmPlugin(),
	mathPlugin(),
	highlightPlugin(),
	metaDataPlugin(),
	yamlMatterPlugin(),
	mentionsPlugin(),
	//rubricPlugin()
] as Plugin[];

