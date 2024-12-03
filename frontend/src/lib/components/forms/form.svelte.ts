import { applyAction } from "$app/forms";
import { toast } from "svelte-sonner";
import { superForm, type SuperValidated } from "sveltekit-superforms";

export function useForm<T extends Record<string, unknown>>(formData: SuperValidated<T>) {
	return superForm(formData, {
		onSubmit({ formData }) {
			toast.dismiss();
			toast.loading("Loading");
		},
		onResult({ result }) {
			if (result.type === "success") {
				toast.dismiss();
				toast.success("Yay!");
			}

			applyAction(result);
		},
	});
}
