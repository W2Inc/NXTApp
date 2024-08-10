<script lang="ts">
	import {
		Icon,
		ExclamationTriangle,
		ExclamationCircle,
		ShieldExclamation,
		CheckCircle,
	} from "svelte-hero-icons";

	import type { IconSource } from "svelte-hero-icons";
	import type { HTMLBaseAttributes } from "svelte/elements";
	type NoticeType = "info" | "warning" | "error" | "success";

	export let type: NoticeType = "info";
	interface $$Props extends HTMLBaseAttributes {
		type?: NoticeType;
	}

	let icon: IconSource;
	switch (type) {
		case "info":
			icon = ExclamationCircle;
			break;
		case "warning":
			icon = ExclamationTriangle;
			break;
		case "error":
			icon = ShieldExclamation;
			break;
		case "success":
			icon = CheckCircle;
			break;
		default:
			icon = ExclamationCircle;
			break;
	}
</script>

<div class="center" {...$$restProps} data-type={type}>
	<span style="color: var(--color); display: flex;">
		<Icon src={icon} size="32px" solid />
	</span>
	<slot />
</div>

<style>
	div {
		--info-color: #096bde;
		--warning-color: #e0c00a;
		--error-color: #bf3939;
		--success-color: #39893a;

		padding: 16px;
		border-radius: var(--br);
		box-shadow: var(--box-shadow);
		border: 1px solid var(--border-color);
		background-color: var(--shade-01);
		font-size: 14px;

		&[data-type="info"] {
			--color: var(--info-color);
			border-left: 4px solid var(--color);
		}

		&[data-type="warning"] {
			--color: var(--warning-color);
			border-left: 4px solid var(--color);
		}

		&[data-type="error"] {
			--color: var(--error-color);
			border-left: 4px solid var(--color);
		}

		&[data-type="success"] {
			--color: var(--success-color);
			border-left: 4px solid var(--color);
		}
	}
</style>
