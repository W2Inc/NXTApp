import { applyAction, enhance } from "$app/forms";
import { error, fail, type RequestEvent } from "@sveltejs/kit";
import { toast } from "svelte-sonner";
import { ZodType, type z, type ZodObject, type ZodRawShape, type ZodTypeAny } from "zod";
import {
	dialog as useDialog,
	type ConfirmProps,
} from "$lib/components/dialog/state.svelte";

interface FormResult<T> {
	message: string;
	form: FormValidation<T>;
}

export interface FormOptions {
	/** Confirm before submitting the form request. */
	confirm?: ConfirmProps | boolean;
	/** Reset the form after submission */
	reset?: boolean;
}

export type FormErrors<T> = {
	[K in keyof T]?: string[];
};

export type InputConstraint = {
	pattern?: string;
	min?: number | string;
	max?: number | string;
	required?: boolean;
	step?: number | "any";
	minlength?: number;
	maxlength?: number;
	readonly?: boolean;
};

export type FieldConstraints<T> = {
	[K in keyof T]?: InputConstraint;
};

export interface FormValidation<T> {
	valid: boolean;
	submitting: boolean;
	data: T;
	errors: FormErrors<T>;
	constraints: FieldConstraints<T>;
}

export function problem<T>(status = 400, message: string, rest: T = undefined as T) {
	return fail(status, { message, ...rest });
}

export function success<T>(message: string, rest: T = undefined as T) {
	return { message, ...rest };
}

/**
 * Type guard for Zod array type definitions
 */
function isArrayType(def: ZodTypeAny["_def"]): boolean {
	return def.typeName === "ZodArray";
}

/**
 * Type guard for nullish or optional Zod types
 */
function isNullishType(field: ZodTypeAny): boolean {
	const typeName = field._def.typeName;
	return (
		typeName === "ZodOptional" ||
		typeName === "ZodNullable" ||
		field._def.hasOwnProperty("defaultValue")
	);
}

/**
 * Processes Zod type checks into HTML input constraints
 */
function processTypeChecks(checks: any[]): Partial<InputConstraint> {
	const constraints: Partial<InputConstraint> = {};

	for (const check of checks) {
		switch (check.kind) {
			case "min":
				if (typeof check.value === "number") {
					constraints.min = check.value;
					constraints.minlength = check.value;
				} else {
					constraints.min = check.value;
					constraints.minlength = check.value;
				}
				break;

			case "max":
				if (typeof check.value === "number") {
					constraints.max = check.value;
					constraints.maxlength = check.value;
				} else {
					constraints.max = check.value;
					constraints.maxlength = check.value;
				}
				break;

			case "regex":
				constraints.pattern = check.regex.source;
				break;

			case "step":
				constraints.step = check.value;
				break;
		}
	}

	return constraints;
}

/**
 * Gets the base Zod type, unwrapping any optional/nullable/default wrappers
 */
function getBaseType(field: ZodTypeAny): ZodTypeAny {
	const typeName = field._def.typeName;

	if (
		typeName === "ZodOptional" ||
		typeName === "ZodNullable" ||
		typeName === "ZodDefault"
	) {
		return getBaseType(field._def.innerType);
	}

	return field;
}

/**
 * Generates HTML input constraints from a Zod schema
 */
function generateConstraints<T extends ZodRawShape>(
	schema: ZodObject<T>,
): FieldConstraints<z.infer<typeof schema>> {
	const constraints: FieldConstraints<z.infer<typeof schema>> = {};

	Object.entries(schema.shape).forEach(([key, field]) => {
		const fieldConstraints: InputConstraint = {};
		const baseType = getBaseType(field as ZodTypeAny);
		const def = baseType._def;

		// Handle required state
		fieldConstraints.required = !isNullishType(field as ZodTypeAny);

		// Handle readonly state
		if (def.typeName === "ZodReadonly") {
			fieldConstraints.readonly = true;
		}

		// Handle array-specific constraints
		if (def.typeName === "ZodArray") {
			if (def.minLength?.value !== undefined) {
				fieldConstraints.minlength = def.minLength.value;
			}
			if (def.maxLength?.value !== undefined) {
				fieldConstraints.maxlength = def.maxLength.value;
			}
		}

		// Handle number-specific constraints
		if (def.typeName === "ZodNumber") {
			// Extract step constraint if it exists
			const stepCheck = def.checks?.find((check: any) => check.kind === "step");
			if (stepCheck) {
				fieldConstraints.step = stepCheck.value;
			}
		}

		// Process common type checks (min, max, regex, etc.)
		if (def.checks?.length > 0) {
			Object.assign(fieldConstraints, processTypeChecks(def.checks));
		}

		// Only add constraints if we have any
		if (Object.keys(fieldConstraints).length > 0) {
			constraints[key as keyof z.infer<typeof schema>] = fieldConstraints;
		}
	});

	return constraints;
}

/**
 * Determines if a field should be treated as null/undefined based on its schema definition
 */
function shouldBeNullish(value: string, field: ZodTypeAny): null | undefined | string {
	if (!value) {
		if (field._def.typeName === "ZodOptional") return undefined;
		if (field._def.typeName === "ZodNullable") return null;
		// If it has a default value, return empty string (will be handled by Zod)
		if (field._def.typeName === "ZodDefault") return "";
	}
	return value;
}

/**
 * Process a single form field value based on its Zod schema type
 */
function processFieldValue(value: FormDataEntryValue, field: ZodTypeAny): unknown {
	if (value instanceof File) {
		// If no file was selected (empty input), return null/undefined based on schema
		if (value.size === 0 && value.name === "") {
			if (field._def.typeName === "ZodOptional") return undefined;
			if (field._def.typeName === "ZodNullable") return null;
			return undefined;
		}
		return value;
	}

	const stringValue = value.toString();

	// Handle ZodEffects (refined types) that might be File or string
	if (
		field._def.typeName === "ZodEffects" &&
		field._def.schema._def.typeName === "ZodUnion"
	) {
		// This handles your z.instanceof(File).or(z.string()) case
		if (stringValue === "") {
			if (isNullishType(field)) {
				if (field._def.typeName === "ZodOptional") return undefined;
				if (field._def.typeName === "ZodNullable") return null;
			}
			return undefined;
		}
		return stringValue;
	}

	// Rest of your existing processFieldValue logic...
	// Handle boolean fields
	if (field._def.typeName === "ZodBoolean") {
		if (stringValue === "on" || stringValue === "true") return true;
		if (stringValue === "" || stringValue === "false") return false;
		return Boolean(stringValue);
	}

	// Handle number fields
	if (field._def.typeName === "ZodNumber") {
		if (!stringValue) return undefined;
		const num = Number(stringValue);
		return isNaN(num) ? undefined : num;
	}

	// Handle string fields with nullish variants
	if (field._def.typeName === "ZodString" || isNullishType(field)) {
		return shouldBeNullish(stringValue, field);
	}

	return stringValue;
}

/**
 * Before parsing the data we need to slightly alter it...
 *
 * For example <input type="checkbox" /> results in a "on" or "" value...
 * Which is fucking stupid in regards to boolean so we need to translate that over.
 *
 * Furthermore it then gets confused with that and a input text being "" which could lead it
 * to inteprid it as a literal "false" value. Also stupid.
 *
 * Second example is with arrays, forms don't "really" support arrays but we make the un-standart
 * decision to mark array values as <name>[] so we then need to parse that as well.
 *
 * We need to fix some of the data for edgecases such as these before we can finish it off in the
 * Zod Parser.
 */
function preprocessFormData<T extends ZodRawShape>(
	formData: FormData,
	schema: ZodObject<T>,
): Record<string, unknown> {
	const result: Record<string, unknown> = {};
	const arrayFields = new Set<string>();

	// First pass: identify array fields
	for (const [key] of formData.entries()) {
		if (key.endsWith("[]")) {
			arrayFields.add(key.slice(0, -2));
		}
	}

	// Second pass: process all fields
	for (const [key, value] of formData.entries()) {
		if (key.endsWith("[]")) {
			const actualKey = key.slice(0, -2);
			const field = schema.shape[actualKey];

			if (!result[actualKey]) {
				result[actualKey] = [];
			}

			if (field && isArrayType(field._def)) {
				const elementType = field._def.type;
				(result[actualKey] as unknown[]).push(processFieldValue(value, elementType));
			} else {
				(result[actualKey] as unknown[]).push(value);
			}
		} else if (!arrayFields.has(key)) {
			const field = schema.shape[key];
			if (field) {
				result[key] = processFieldValue(value, field);
			} else {
				result[key] = value;
			}
		}
	}

	return result;
}

/**
 * Enhanced form handling with type-safe validation
 */
export function useForm<T extends Record<string, unknown>>(
	form: FormValidation<T>,
	options: FormOptions = { confirm: false, reset: false },
) {
	const formState = $state(form);

	function kitEnhance(form: HTMLFormElement) {
		enhance(form, async ({ formElement, formData, action, cancel }) => {
			if (options.confirm) {
				const confirmed = await useDialog.confirm(
					typeof options.confirm === "boolean" ? undefined : options.confirm,
				);

				if (!confirmed) {
					cancel();
					return;
				}
			}

			toast.dismiss();
			toast.loading("Please wait...");
			formState.submitting = true;

			return async ({ result, update }) => {
				formState.submitting = false;
				if (result.type === "success" || result.type === "failure") {
					toast.dismiss();
					// @ts-expect-error - Cool story bro
					const formResultData = result.data as FormResult<T>;

					formState.data = formResultData.form.data;
					formState.valid = formResultData.form.valid;
					formState.errors = formResultData.form.errors;
					formState.constraints = formResultData.form.constraints;

					if (result.type === "success") {
						toast.success(formResultData.message);
					} else {
						toast.error(formResultData.message);
					}
				}

				if (options.reset) {
					await update({ reset: true });
				}
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

export async function validate<T extends ZodRawShape>(
	request: Request,
	schema: ZodObject<T>,
): Promise<FormValidation<z.infer<typeof schema>>> {
	const constraints = generateConstraints(schema);
	try {
		const formData = await request.formData();
		const processedData = preprocessFormData(formData, schema);
		const result = schema.safeParse(processedData);

		if (result.success) {
			return {
				valid: true,
				data: result.data,
				errors: {},
				constraints,
				submitting: false,
			};
		}

		const errors: FormErrors<z.infer<typeof schema>> = {};
		result.error.issues.forEach((issue) => {
			const path = issue.path[0] as keyof z.infer<typeof schema>;
			if (!errors[path]) {
				errors[path] = [];
			}
			errors[path]!.push(issue.message);
		});

		return {
			valid: false,
			data: processedData as z.infer<typeof schema>,
			errors,
			constraints,
			submitting: false,
		};
	} catch (e) {
		const errorMessage = e instanceof Error ? e.message : "Failed to process form data";
		return {
			valid: false,
			data: {} as z.infer<typeof schema>,
			constraints,
			errors: {
				_form: [errorMessage],
			},
			submitting: false,
		};
	}
}
