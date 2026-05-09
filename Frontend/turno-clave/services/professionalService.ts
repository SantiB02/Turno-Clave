import { authenticatedFetch } from "@/lib/api/authenticated-fetch"

const ROOT_PATH = "/professionals"

export async function getProfessionalsByActiveBusiness() {
  try {
    const res = await authenticatedFetch(`${ROOT_PATH}/active-business`)
    if (!res.ok) {
      console.error(
        "Error fetching professionals:",
        res.status,
        await res.text(),
      )
      throw new Error("Error fetching professionals")
    }
    return res.json()
  } catch (error) {
    console.error("Error fetching professionals:", error)
    throw new Error("Error fetching professionals")
  }
}
