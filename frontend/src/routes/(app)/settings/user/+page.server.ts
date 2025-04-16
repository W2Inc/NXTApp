// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import type { PageServerLoad } from "./$types";
import { formValueToS3, problem, success, type FormState, type PageMixFormBundle } from "$lib/utils/api.svelte";
import { error as kitError, redirect } from "@sveltejs/kit";
import { Constants, ensure } from "$lib/utils";
import { logger } from "$lib/logger";


// ============================================================================

export type FormBundle = PageMixFormBundle<
	BackendTypes["UserPatchRequestDTO"],
	BackendTypes["UserDetailsPutRequestDTO"],
	{
		avatarUrl?: string | File;
		markdown?: string;
	}
>;

// ============================================================================

export const load: PageServerLoad = async ({ locals }) => {
	const session = await locals.session() ?? kitError(401);
	const { data, error } = await locals.api.GET("/users/current");
	if (error || !data) {
		logger.error("Failed to fetch user data", error);
		kitError(500);
	}

	return {
		form: {
			data: {
				firstName: data.details?.firstName,
				lastName: data.details?.lastName,
				displayName: data.displayName,
				email: data.details?.email,
				markdown: data.details.markdown,
				websiteUrl: data.details?.websiteUrl,
				twitterUrl: data.details?.twitterUrl,
				linkedinUrl: data.details?.linkedinUrl,
				githubUrl: data.details?.githubUrl,
				avatarUrl: data.avatarUrl ?? session.avatarUrl ?? Constants.FALLBACK_IMG,
			},
			errors: {},
			isLoading: false,
		} as FormState<FormBundle>,
	}
};

// ============================================================================

export const actions = {
	default: async ({ request, locals, setHeaders, url }) => {
		const session = await locals.session() ?? kitError(401);
		const form = await request.formData();

		// const avatarUrl = form.get("avatarUrl");
		// if (avatarUrl && avatarUrl instanceof File && avatarUrl.size > 0) {

		// }

		let [image, issue] = await ensure(formValueToS3<FormBundle>(form, "avatarUrl"));


		setHeaders({ "Cache-Control": "no-cache, must-revalidate, max-age=0" });

		const processAvatar = async (avatar: string | File | null) => {
			return "";
		};

		const [userUpdate, detailsUpdate] = await Promise.all([
			locals.api.PATCH("/users/{id}", {
				params: { path: { id: session.user_id } },
				body: {
					displayName: form.get("displayName")?.toString(),
					// avatarUrl: await processAvatar(form.get("avatarUrl")),
				},
			}),
			locals.api.PUT("/users/{id}/details", {
				params: { path: { id: session.user_id } },
				body: {
					firstName: form.get("firstName")?.toString(),
					lastName: form.get("lastName")?.toString(),
					websiteUrl: form.get("websiteUrl")?.toString(),
					bio: form.get("markdown")?.toString(),
					twitterUrl: form.get("twitterUrl")?.toString(),
					linkedinUrl: form.get("linkedinUrl")?.toString(),
					githubUrl: form.get("githubUrl")?.toString(),
				},
			}),
		]);

		if (userUpdate.error || detailsUpdate.error) {
			const error = userUpdate.error || detailsUpdate.error;
			return problem({
				status: error?.status ?? 422,
				title: error?.title ?? "Something went wrong...",
				// @ts-ignore
				errors: error?.errors ?? {},
			});
		}

		// Upload

		return success("Account details updated!");
	}
};
