// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import autoPrefixer from "autoprefixer";
import postcssNested from "postcss-nested";
import postcssAtRoot from "postcss-atroot";
import postcssCustomMedia from "postcss-custom-media";

const config = {
	syntax: "postcss-scss",
	plugins: [autoPrefixer, postcssNested, postcssAtRoot, postcssCustomMedia],
};

export default config;
