// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

import { z } from "zod";
import { error, fail } from "@sveltejs/kit";
import type { PageServerLoad } from "./$types";
import { problem, success, validate } from "$lib/utils/form.svelte";
import { Constants, isEmptyFile } from "$lib/utils";
import { keycloak } from "$lib/oauth";
import { S3_BUCKET, S3_ENDPOINT } from "$env/static/private";

// ============================================================================

const schema = z.object({
	id: z.string().uuid().readonly(),
	login: z.string().readonly(),
	firstName: z.string().readonly(),
	lastName: z.string().readonly(),
	displayName: z.string().min(4).max(128).nullish(),
	markdown: z.string().min(4).max(2048).nullish(),
	website: z.string().url().startsWith("https://").nullish(),
	twitter: z.string().url().startsWith("https://twitter.com/").nullish(),
	linkedin: z.string().url().startsWith("https://linkedin.com/").nullish(),
	github: z.string().url().startsWith("https://github.com/").nullish(),
	image: z
		.instanceof(File, { message: "Please upload a file." })
		// .refine((f) => f.size < 100_000, "Max 100 kB upload size.")
		.or(z.string().url())
		.optional(),
});

// ============================================================================

export const load: PageServerLoad = async ({ locals, request }) => {
	const session = (await locals.session()) ?? error(401, "No session");
	const form = await validate(request, schema);

	const [userData, userBio] = await Promise.all([
		locals.api.GET("/users/current"),
		locals.api.GET("/users/{id}/bio", {
			params: { path: { id: session.user_id } },
		}),
	]);

	if (userData.error || !userData.data || userBio.error || !userBio) {
		error(500, "Failed to fetch user data.");
	}

	const data = userData.data;
	const bio = userBio.data ?? "";

	console.log(data.displayName, data)

	form.data = {
		id: data.id,
		login: data.login,
		displayName: data.displayName,
		firstName: session.given_name,
		lastName: session.family_name,
		markdown: bio,
		image: data.avatarUrl ?? Constants.FALLBACK_IMG,
		website: data.details?.websiteUrl,
		linkedin: data.details?.linkedinUrl,
		twitter: data.details?.twitterUrl,
		github: data.details?.githubUrl,
	};

	return { form };
};

export const actions = {
	default: async ({ request, locals }) => {
		console.log("WTF")
		const session = (await locals.session()) ?? error(401);
		const form = await validate(request, schema);

		if (!form.valid) {
			return problem(400, `${JSON.stringify(form.errors)}`, { form });
		}

		// Handle file upload...
		if (form.data.image instanceof File && form.data.image.size > 0) {
			const ext = form.data.image.name.split(".").pop() ?? "jpg";
			const fileName = `${session.user_id}.${ext}`;
			const file = Bun.s3.file(`${session.user_id}.${ext}`);
			await file.write(form.data.image, {
				acl: "public-read",
				bucket: "images",
			});

			form.data.image = `${S3_ENDPOINT}/${S3_BUCKET}/`;
			const { error } = await locals.api.PATCH("/users/{id}", {
				params: { path: { id: session.user_id } },
				body: {
					displayName: form.data.displayName,
					avatarUrl: `${S3_ENDPOINT}/${S3_BUCKET}/${fileName}`
				},
			});

			console.log(error)
		}

		// NOTE(W2): Undefine this field as it can't be serialized...
		delete form.data.image;
		return success("Profile updated", { form });
	},
};
