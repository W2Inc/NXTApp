// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { z } from "zod";
import { error, fail } from "@sveltejs/kit";
import { applyAction, enhance } from "$app/forms";
import { env } from "$env/dynamic/public";
import { toast } from "svelte-sonner";
import {
	dialog as useDialog,
	type ConfirmProps,
} from "$lib/components/dialog/state.svelte";
import { goto } from "$app/navigation";

// ============================================================================

/** (RFC 7807 Problem Details) */
export interface Problem<T = unknown> {
	type?: string;
	title: string;
	detail?: string; // Often optional
	status: number;
	traceId?: string; // Often optional
	/** Validation errors, keyed by field name (potentially capitalized) */
	errors?: Record<Capitalize<Extract<keyof T, string>>, string[]>;
}

type FormResult<T> =
	| {
			type: "success";
			status: number;
			data: {
				message: string;
				redirect?: string;
			};
	  }
	| {
			type: "failure";
			status: number;
			data: {
				problem: Problem<T>;
			};
	  };

// interface FormResult<T> {

// 	message: string;
// 	redirect?: string;
// 	// form: FormValidation<T>;
// }

/** Represents validation errors for a specific form data structure T */
// export type FormErrors<T extends object> = {
// 	// Partial allows only fields with errors to be present
// 	[K in keyof T]?: string[];
// };

/** Represents the result of validation, containing data and errors */
// export interface FormValidation<T extends object> {
// 	/**
// 	 * The data associated with the form.
// 	 * If validation failed, this is typically the original data submitted by the user.
// 	 * If validation succeeded, this might be the successful response data from the backend.
// 	 */
// 	data: T;
// 	/** Validation errors keyed by field name. Empty if validation passed. */
// 	errors: FormErrors<T>;
// }

export interface FormOptions {
	/** First confirm before submit. */
	confirm?: ConfirmProps | boolean;
	reset?: boolean;
}

export interface FormState<T> {
	isLoading: boolean;
	errors: Record<Capitalize<Extract<keyof T, string>>, string[]>;
	data: T;
}

/**
 * Builds a convenient type for form bundles.
 *
 * A form bundle is essentially a combination of:
 * - A SET type (post) that contains the original properties
 * - A UPDATE type (put) that contains properties to be combined with the source
 * - A CUSTOM type that contains properties that should override the combined result
 *
 * @template S - The source type containing original properties
 * @template U - The update type containing properties to be combined with the source
 * @template C - The custom type containing properties that should override the combined result
 */
export type PageFormBundle<S, U, C> = Omit<
	{
		[K in keyof S & keyof U]-?: NonNullable<S[K]>;
	},
	keyof C
> &
	C;

// ============================================================================
// Zod Schema for the Problem Details structure
// Make fields optional where appropriate according to RFC 7807 and common usage
export const ProblemSchema = z.object({
	type: z.string(),
	title: z.string(),
	detail: z.string().optional(),
	status: z.number().int(),
	traceId: z.string().optional(),
	errors: z.record(z.string(), z.array(z.string())).optional(), // Key is string, value is array of strings
});

// ============================================================================

// export function success2<T>(message: string, rest: T = undefined as T, redirect?: string) {
// 	return { message, ...rest, redirect };
// }

// export function problem2<T>(status = 400, message: string, rest: T = undefined as T) {
// 	return fail(status, { message, ...rest });
// }

export function success(message: string, redirect?: string) {
	return { message, redirect };
}

export function problem(problem: Problem) {
	return fail(problem.status, { problem });
}

/**
 * Extracts a value from the form data and preps it to be uploaded to S3.
 *
 * @template T - A generic type representing the shape the form is mapped to.
 * @param form - The FormData instance containing the file to be uploaded.
 * @param key - The key within the form that references the file to upload.
 * @param maxSize - An optional maximum allowed file size in bytes (defaults to 5 MB).
 * @throws {Error} Throws if no file is provided or if the file size exceeds the allowed limit.
 * @returns An object containing both the S3 file reference and the URL where the file can be accessed.
 */
export async function formValueToS3<T>(
	form: FormData,
	key: keyof T,
	maxSize: number = 5 * 1024 * 1024,
) {
	const thumbnail = form.get(key as string) as File | null;
	if (thumbnail === null || thumbnail.size === 0)
		throw new Error("Please upload a image.");
	if (thumbnail.size > maxSize)
		throw new Error(`File size too large. Maximum size is ${maxSize}MB.`);

	const fileNameParts = thumbnail.name.split(".");
	const ext = fileNameParts.length > 1 ? fileNameParts.pop() : null;
	if (!ext) throw new Error("Invalid file");

	const fileName = `${Bun.randomUUIDv7()}.${ext}`;
	return {
		client: Bun.s3.file(fileName),
		file: thumbnail,
		url: `${env.PUBLIC_S3_ENDPOINT}/${env.PUBLIC_S3_BUCKET}/${fileName}`,
	};
}

// ============================================================================

export function useForm<T>(defaultValue: FormState<T>, options: FormOptions) {
	const formState = $state<FormState<T>>(defaultValue);
	let opt = options ?? { confirm: false };

	function kitEnhance(form: HTMLFormElement) {
		enhance(form, async ({ formElement, formData, action, cancel }) => {
			if (opt.confirm) {
				const props = typeof options.confirm === "boolean" ? undefined : options.confirm;
				if (!(await useDialog.confirm(props))) {
					cancel();
					return;
				}
			}

			toast.dismiss();
			toast.loading("Please wait...");
			formState.isLoading = true;
			return async ({ result, update }) => {
				formState.isLoading = false;
				if (result.type === "error" && result.status === 404) {
					toast.error("Not implemented form");
					return;
				}
				if (result.type === "success" || result.type === "failure") {
					toast.dismiss();
					const formResult = result as FormResult<T>;

					if (formResult.type === "success") {
						toast.success(formResult.data.message);
					} else {
						toast.error(formResult.data.problem.title);
						console.log(formResult.data.problem.errors);
						if (formResult.data.problem.errors)
							formState.errors = formResult.data.problem.errors;
					}
					if (formResult.type === "success" && formResult.data.redirect) {
						return goto(formResult.data.redirect);
					}
				}

				if (options.reset) await update({ reset: true });
				await applyAction(result);
			};
		});
	}

	return {
		enhance: kitEnhance,
		get form() {
			return formState;
		},
	};
}

// ============================================================================

// Define the structure for the validation result
interface ValidationResult<K> {
	/**
	 * Contains the data extracted from FormData for the keys that were successfully found.
	 * Values are FormDataEntryValue (string | File) and may require further parsing/coercion.
	 * This is Partial because not all keys might be present if validation fails for some.
	 */
	data: Partial<Record<K, FormDataEntryValue>>;

	/**
	 * Maps keys (from the expected keys) to an array of error messages for that key.
	 * This is Partial because a key will only be present here if it has errors.
	 */
	errors: Partial<Record<K, string[]>>;

	/**
	 * A boolean flag indicating if any errors were found.
	 */
	hasErrors: boolean;
}
