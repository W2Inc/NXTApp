// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { z } from "zod";
import { error, fail } from "@sveltejs/kit";
import { enhance } from "$app/forms";

// ============================================================================

/** .NET backend error response body (RFC 7807 Problem Details) */
export interface Problem {
	type: string;
	title: string;
	detail?: string; // Often optional
	status: number;
	traceId?: string; // Often optional
	/** Validation errors, keyed by field name (potentially capitalized) */
	errors?: Record<string, string[]>; // Make errors optional as not all problems are validation errors
};

/** Represents validation errors for a specific form data structure T */
export type FormErrors<T extends object> = {
	// Partial allows only fields with errors to be present
	[K in keyof T]?: string[];
};

/** Represents the result of validation, containing data and errors */
export interface FormValidation<T extends object> {
	/**
	 * The data associated with the form.
	 * If validation failed, this is typically the original data submitted by the user.
	 * If validation succeeded, this might be the successful response data from the backend.
	 */
	data: T;
	/** Validation errors keyed by field name. Empty if validation passed. */
	errors: FormErrors<T>;
}

export interface FormOptions {
	confirm?: boolean;
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
export type PageFormBundle<S, U, C> = Omit<{
	[K in keyof S & keyof U]-?: NonNullable<S[K]>;
}, keyof C> & C;


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

export function success<T>(message: string, rest: T = undefined as T, redirect?: string) {
	return { message, ...rest, redirect };
}

export function problem<T>(status = 400, message: string, rest: T = undefined as T) {
	return fail(status, { message, ...rest });
}

// ============================================================================

export function useForm<TDTO>(options: FormOptions) {
	let opt = options ?? { confirm: false };

	return {
		enhance,
		form: {} as TDTO,
	}
};

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

type TypeToZod<T> = {
	[K in keyof T]-?: T[K] extends
	| Date
	| string
	| number
	| boolean
	| null
	| undefined
	? undefined extends T[K]
	? z.ZodOptional<z.ZodType<Exclude<T[K], undefined>>>
	: z.ZodType<T[K]>
	: z.ZodObject<TypeToZod<T[K]>>
	| z.ZodArray<z.ZodType<T[K]>>
	| z.ZodNullable<z.ZodType<T[K]>>
	| z.ZodUnion<readonly [z.ZodType<T[K]>, ...z.ZodType<T[K]>[]]> };

export const createDTOSchema = <T>(obj: TypeToZod<T>) => {
	return z.object(obj);
};


export async function preprocessForm<T>(formData: Promise<FormData>, callback?: (key: keyof T, value: FormDataEntryValue) => FormDataEntryValue | Promise<FormDataEntryValue>): Promise<T> {
	let body: Record<string, any> = {};
	const formDataObj = await formData;
	formDataObj.forEach(async (value, key) => {
		if (callback) {
			value = await callback(key as keyof T, value);
			console.log("Callback", key, value);
		}

		console.log("FormData", key, value);
		const strValue = value.toString();
		if (strValue === "true" || strValue === "false") {
			body[key] = strValue === "true";
		}
		else if (/^-?\d+(\.\d+)?$/.test(strValue)) {
			body[key] = Number(strValue);
		}
		else {
			body[key] = strValue;
		}
	});

	return body as T;
}

