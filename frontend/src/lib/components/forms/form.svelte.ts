import { applyAction } from "$app/forms";
import { toast } from "svelte-sonner";
import { superForm, type SuperValidated } from "sveltekit-superforms";
import { dialog as useDialog, type ConfirmProps } from "../dialog/state.svelte";

interface FormOptions {
	confirm?: ConfirmProps | boolean;
	dataType?: "json" | "form";
	successMessage?: string;
	failMessage?: string;
}

/**
 * Thin utility wrapper around superForms.
 * @param formData The form data.
 * @param options Additional options for handling the form.
 * @returns
 */
export function useForm<T extends Record<string, unknown>>(
	formData: SuperValidated<T>,
	options?: FormOptions,
) {
	const formOptions = options ?? {
		dataType: "form",
	};

	return superForm(formData, {
		dataType: formOptions.dataType,
		multipleSubmits: "prevent",
		async onSubmit({ cancel }) {
			if (formOptions.confirm !== undefined || formOptions.confirm === true) {
				const c = await useDialog.confirm(
					typeof formOptions.confirm === "boolean" ? undefined : formOptions.confirm,
				);
				if (!c) {
					cancel();
					return;
				}
			}

			toast.dismiss();
			toast.loading("Please wait...");
		},
		onResult({ result }) {
			if (result.type === "success") {
				toast.dismiss();
				toast.success(formOptions.successMessage ?? "Success!");
			}
			if (result.type === "failure") {
				toast.dismiss();
				toast.error(formOptions.failMessage ?? "Something went wrong...");
			}

			applyAction(result);
		},
	});
}

// export function useForm<T extends Record<string, unknown>>(formData: SuperValidated<T>, confirm = false, dataType: "json" | "form" = "form") {
// return superForm(formData, {
// 	dataType,
// 	async onSubmit({ formData, cancel }) {
// 		if (confirm) {
// 			const c = await useDialog.confirm();
// 			if (!c) {
// 				cancel();
// 				return;
// 			}
// 		}

// 		toast.dismiss();
// 		toast.loading("Loading");
// 	},
// 	onResult({ result }) {
// 		if (result.type === "success") {
// 			toast.dismiss();
// 			toast.success("Success!");
// 		}
// 		if (result.type === "failure") {
// 			toast.dismiss();
// 			toast.error("Something went wrong...");
// 		}

// 		applyAction(result);
// 	},
// });
// }
