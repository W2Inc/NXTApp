
import type { LayoutServerLoad } from "./$types"

export const load: LayoutServerLoad = async ({locals, fetch}) => {
  const session = await locals.auth()

	if (session) {
		try {
			const response = await fetch("http://localhost:3000/users/current", {
				signal: AbortSignal.timeout(2000),
				headers: {
					Authorization: `Bearer ${session?.access_token}`
				}
			});

			return {
				session,
				data: response.json()
			}
		} catch (error) {
		}


	}

	// console.log(session, response);

  return {
    session,
		data: null
  }
}
