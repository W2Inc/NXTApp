import { applyAction, enhance } from "$app/forms";
import { error, fail, type RequestEvent } from "@sveltejs/kit";
import { toast } from "svelte-sonner";
import type { z, ZodObject, ZodRawShape, ZodTypeAny } from "zod";
import {
	dialog as useDialog,
	type ConfirmProps,
} from "$lib/components/dialog/state.svelte";

interface FormResult<T> {
	message: string;
	form: FormValidation<T>;
}

export interface FormOptions {
	confirm: ConfirmProps | boolean;
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
};

export type FieldConstraints<T> = {
	[K in keyof T]?: InputConstraint;
};

export interface FormValidation<T> {
	valid: boolean;
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

function isNestedObject(def: any): boolean {
	return def.typeName === "ZodObject";
}

function isArrayType(def: any): boolean {
	return def.typeName === "ZodArray";
}

function validateSchema<T extends ZodRawShape>(schema: ZodObject<T>): void {
	Object.entries(schema.shape).forEach(([key, field]) => {
		const def = (field as ZodTypeAny)._def;

		if (isNestedObject(def)) {
			throw new Error(
				`Nested objects are not supported. Field "${key}" contains an object type.`,
			);
		}

		if (isArrayType(def)) {
			const elementDef = def.type._def;
			if (isNestedObject(elementDef)) {
				throw new Error(
					`Arrays of objects are not supported. Field "${key}" contains an array of objects.`,
				);
			}
		}
	});
}

function processFormDataWithArrays(formData: FormData): Record<string, unknown> {
	const result: Record<string, unknown> = {};
	const arrayFields = new Set<string>();

	// First pass: identify array fields
	for (const [key, value] of formData.entries()) {
		if (key.endsWith("[]")) {
			arrayFields.add(key.slice(0, -2));
		}
	}

	// Second pass: process all fields
	for (const [key, value] of formData.entries()) {
		if (key.endsWith("[]")) {
			const actualKey = key.slice(0, -2);
			if (!result[actualKey]) {
				result[actualKey] = [];
			}
			(result[actualKey] as unknown[]).push(value);
		} else if (!arrayFields.has(key)) {
			if (value === "true" || value === "false") {
				result[key] = value === "true";
			} else if (value === "on") {
				// Handle checkbox inputs
				result[key] = true;
			} else if (value === "") {
				// Handle unchecked checkbox
				result[key] = false;
			} else {
				result[key] = value;
			}
		}
	}

	return result;
}

function extractConstraints<T extends ZodRawShape>(
	schema: ZodObject<T>,
): FieldConstraints<z.infer<typeof schema>> {
	const constraints: FieldConstraints<z.infer<typeof schema>> = {};

	Object.entries(schema.shape).forEach(([key, field]) => {
		const def = (field as ZodTypeAny)._def;
		const fieldConstraints: InputConstraint = {};

		if (isArrayType(def)) {
			if (def.minLength?.value !== undefined) {
				fieldConstraints.minlength = def.minLength.value;
			}
			if (def.maxLength?.value !== undefined) {
				fieldConstraints.maxlength = def.maxLength.value;
			}
		} else if (def.checks) {
			def.checks.forEach((check: any) => {
				switch (check.kind) {
					case "min":
						fieldConstraints.min = check.value;
						fieldConstraints.minlength = check.value;
						break;
					case "max":
						fieldConstraints.max = check.value;
						fieldConstraints.maxlength = check.value;
						break;
					case "regex":
						fieldConstraints.pattern = check.regex.source;
						break;
				}
			});
		}

		// TODO: Figure out some better ways to check this
		if (def.defaultValue) {
			fieldConstraints.required = false;
		} else {
			fieldConstraints.required = true;
		}

		if (Object.keys(fieldConstraints).length > 0) {
			constraints[key as keyof z.infer<typeof schema>] = fieldConstraints;
		}
	});

	return constraints;
}

export function useForm<T extends Record<string, unknown>>(
	form: FormValidation<T>,
	options: FormOptions,
) {
	const formState = $state(form);

	function kitEnhance(form: HTMLFormElement) {
		enhance(form, async ({ formElement, formData, action, cancel, submitter }) => {
			if (options.confirm !== undefined) {
				const c = await useDialog.confirm(
					typeof options.confirm === "boolean" ? undefined : options.confirm,
				);

				if (!c) {
					cancel();
					return;
				}
			}

			toast.dismiss();
			toast.loading("Please wait...");

			return async ({ result, update }) => {
				if (result.type === "success") {
					toast.dismiss();
					const formResultData = result.data as unknown as FormResult<T>;
					console.log(result);

					formState.data = formResultData.form.data;
					formState.valid = formResultData.form.valid;
					formState.errors = formResultData.form.errors;
					formState.constraints = formResultData.form.constraints;
					toast.success(formResultData.message);
				}
				if (result.type === "failure") {
					toast.dismiss();

					const formResultData = result.data as unknown as FormResult<T>;
					formState.data = formResultData.form.data;
					formState.valid = formResultData.form.valid;
					formState.errors = formResultData.form.errors;
					formState.constraints = formResultData.form.constraints;
					toast.error(formResultData.message);
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
	validateSchema(schema);

	const constraints = extractConstraints(schema);
	try {
		const formData = await request.formData();
		const data = processFormDataWithArrays(formData);
		const result = schema.safeParse(data);

		if (result.success) {
			return {
				valid: true,
				data: result.data,
				errors: {},
				constraints,
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
			data: data as z.infer<typeof schema>,
			errors,
			constraints,
		};
	} catch (e) {
		if (e instanceof Error) {
			return {
				valid: false,
				data: {} as z.infer<typeof schema>,
				constraints,
				errors: {
					_form: [e.message],
				},
			};
		}
		return {
			valid: false,
			data: {} as z.infer<typeof schema>,
			constraints,
			errors: {
				_form: ["Failed to process form data"],
			},
		};
	}
}
