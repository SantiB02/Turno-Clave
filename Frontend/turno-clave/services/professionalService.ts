"use server"

import { authenticatedFetch } from "@/lib/api/authenticated-fetch"
import { rethrowWithFallback, throwResponseError } from "@/lib/api/error-utils"
import type { Professional } from "@/types/professional"

const ROOT_PATH = "/professionals"

export async function getProfessionalsByActiveBusiness(): Promise<
  Professional[]
> {
  try {
    const res = await authenticatedFetch(`${ROOT_PATH}/active-business`)

    if (!res.ok) {
      await throwResponseError(res, "Error fetching professionals")
    }

    return res.json()
  } catch (error) {
    console.error("[getProfessionalsByActiveBusiness]", error)
    rethrowWithFallback(error, "Error fetching professionals")
  }
}
