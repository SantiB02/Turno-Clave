"use server"

import { authenticatedFetch } from "@/lib/api/authenticated-fetch"
import { rethrowWithFallback, throwResponseError } from "@/lib/api/error-utils"
import type { BusinessDetail, CreateBusinessDTO } from "@/types/business"

const ROOT_PATH = "/businesses"

export async function createBusiness(data: CreateBusinessDTO) {
  try {
    const res = await authenticatedFetch(`${ROOT_PATH}`, {
      method: "POST",
      body: JSON.stringify(data),
    })

    if (!res.ok) {
      await throwResponseError(res, "Error creating business")
    }

    return res.json()
  } catch (error) {
    console.error("[createBusiness]", error)
    rethrowWithFallback(error, "Error creating business")
  }
}

export async function getMyBusinesses(): Promise<BusinessDetail[]> {
  try {
    const res = await authenticatedFetch(`${ROOT_PATH}/mine`)

    if (!res.ok) {
      await throwResponseError(res, "Error fetching businesses")
    }

    return res.json()
  } catch (error) {
    console.error("[getMyBusinesses]", error)
    rethrowWithFallback(error, "Error fetching businesses")
  }
}
