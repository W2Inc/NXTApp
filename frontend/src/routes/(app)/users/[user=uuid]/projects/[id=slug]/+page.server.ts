import api from "$lib/api/api";
import type { components } from "$lib/api/types";
import type { PageServerLoad } from "./$types";

	//ai={50} tester={20} remote={20} onsite={10}
	//
interface MeterMakeup {
	auto: number;
	async: number;
	peer: number;
	self: number
}

export const load: PageServerLoad = async ({ parent, locals }) => {
	const data = await parent();

	const reviewMakeup: MeterMakeup = {
		async: data.reviews.filter((value) => value.type == "async").length,
		auto: data.reviews.filter((value) => value.type == "auto").length,
		self: data.reviews.filter((value) => value.type == "self").length,
		peer: data.reviews.filter((value) => value.type == "peer").length
	}

	return {
		project: data.project,
		usersProject: data.usersProject,
		reviews: data.reviews,
		user: data.user,
		reviewMakeup
	};
};
