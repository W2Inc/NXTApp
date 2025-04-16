import { z } from "zod";
import { error, fail } from "@sveltejs/kit";
import type { PageServerLoad } from "./$types";
import { problem, success, validate } from "$lib/utils/form.svelte";
import { Constants } from "$lib/utils";
import { keycloak } from "$lib/oauth";
import { logger } from "$lib/logger";
import { page } from "$app/state";
import { PUBLIC_S3_ENDPOINT } from "$env/static/public";

const schema = z.object({
	id: z.string().uuid().readonly(),
	login: z.string().readonly(),
	firstName: z.string().nullish(),
	lastName: z.string().nullish(),
	displayName: z
		.string()
		.min(4)
		.max(128)
		.regex(
			/^[a-zA-Z0-9]+(?:[_-][a-zA-Z0-9]+)*$/,
			"Display name can only contain letters, numbers and dashes",
		)
		.nullish(),
	markdown: z.string().min(4).max(2048).nullish(),
	website: z.string().url().startsWith("https://").nullish(),
	twitter: z.string().url().startsWith("https://x.com/").nullish(),
	linkedin: z.string().url().startsWith("https://linkedin.com/").nullish(),
	github: z.string().url().startsWith("https://github.com/").nullish(),
	image: z
		.union([
			z.string(),
			z
				.instanceof(File, { message: "Please upload a file." })
				.refine((file) => file.size < 100_000, "Max 100 kB upload size."),
		])
		.optional(),
});

export const load: PageServerLoad = async ({ locals, request, url }) => {
	const session = (await locals.session()) ?? error(401, "No session");
	const form = await validate(request, schema);

	// Example fetching user data
	const [userData, userBio] = await Promise.all([
		locals.api.GET("/users/current"),
		locals.api.GET("/users/{id}/bio", {
			parseAs: "text",
			params: { path: { id: session.user_id } },
		}),
	]);
	if (userData.error || !userData.data || userBio.error || !userBio) {
		throw error(500, "Failed to fetch user data.");
	}

	const data = userData.data;
	const bio = userBio.data ?? "";

	// Populate form data with existing values
	form.data = {
		id: data.id,
		login: data.login,
		displayName: data.displayName,
		firstName: data.details?.firstName ?? session.given_name,
		lastName: data.details?.lastName ?? session.family_name,
		markdown: bio,
		image: data.avatarUrl ?? Constants.FALLBACK_IMG,
		website: data.details?.websiteUrl,
		linkedin: data.details?.linkedinUrl,
		twitter: data.details?.twitterUrl,
		github: data.details?.githubUrl,
	};

	logger.debug(`Form data on ${url}`, form.data);

	return { form };
};

export const actions = {
	default: async ({ request, locals, setHeaders, url }) => {
		const session = (await locals.session()) ?? error(401, "No session");
		const form = await validate(request, schema);
		let avatarUrl: string | undefined;

		logger.debug(`Submitted Form => ${url}`, form.data);

		try {
			if (!form.valid) {
				logger.debug("Invalid form", form.errors);
				return problem(400, "Unable to update profile", { form });
			}
			if (form.data.image instanceof File && form.data.image.size > 0) {
				if (session.avatarUrl) {
					await Bun.s3.delete(session.avatarUrl);
				}
				const ext = form.data.image.name.split(".").pop() ?? "png";
				const fileName = `${session.user_id}.${ext}`;
				const file = Bun.s3.file(fileName);
				await file.write(form.data.image, { acl: "public-read", bucket: "images" });
				avatarUrl = `${PUBLIC_S3_ENDPOINT}/${PUBLIC_S3_ENDPOINT}/${fileName}`;
			}

			const [userUpdate, detailsUpdate] = await Promise.all([
				locals.api.PATCH("/users/{id}", {
					params: { path: { id: session.user_id } },
					body: { displayName: form.data.displayName, avatarUrl },
				}),
				locals.api.PUT("/users/{id}/details", {
					params: { path: { id: session.user_id } },
					body: {
						bio: form.data.markdown,
						firstName: form.data.firstName,
						lastName: form.data.lastName,
						websiteUrl: form.data.website,
						twitterUrl: form.data.twitter,
						linkedinUrl: form.data.linkedin,
						githubUrl: form.data.github,
					},
				}),
				// locals.api.PUT("/users/{id}/bio", {
				// 	params: { path: { id: session.user_id } },
				// 	body: form.data.markdown
				// }),
			]);

			if (userUpdate.error || detailsUpdate.error) {
				return problem(400, "Unable to update profile", { form });
			}
			return success("Profile updated", { form });
		} catch {
			return problem(400, "Unable to update profile", { form });
		} finally {
			setHeaders({ "Cache-Control": "no-cache, must-revalidate, max-age=0" });
			form.data.image = avatarUrl ?? session.avatarUrl;
		}
	},
};
