import { applyAction } from "$app/forms";
import { toast } from "svelte-sonner";
import { superForm, type SuperValidated } from "sveltekit-superforms";
import { dialog as useDialog } from "../dialog/state.svelte";

export function useForm<T extends Record<string, unknown>>(formData: SuperValidated<T>, confirm = false) {
	return superForm(formData, {
		async onSubmit({ formData, cancel }) {
			if (confirm) {
				const c = await useDialog.confirm();
				if (!c) {
					cancel();
					return;
				}
			}

			toast.dismiss();
			toast.loading("Loading");
		},
		onResult({ result }) {
			if (result.type === "success") {
				toast.dismiss();
				toast.success("Yay!");
			}
			if (result.type === "failure") {
				toast.dismiss();
				toast.error("Something went wrong...");
			}

			applyAction(result);
		},
	});
}
