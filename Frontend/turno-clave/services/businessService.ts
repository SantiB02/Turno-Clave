"use server"

import { authenticatedFetch } from "@/lib/api/authenticated-fetch"
import { rethrowWithFallback, throwResponseError } from "@/lib/api/error-utils"
import type {
  BusinessDetail,
  CreateBusinessDTO,
  UpdateBusinessDTO,
} from "@/types/business"

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

export async function getActiveBusiness(): Promise<BusinessDetail> {
  try {
    const res = await authenticatedFetch(`${ROOT_PATH}/active`)

    if (!res.ok) {
      await throwResponseError(res, "Error fetching active business")
    }

    return res.json()
  } catch (error) {
    console.error("[getActiveBusiness]", error)
    rethrowWithFallback(error, "Error fetching active business")
  }
}

export async function updateBusiness(
  externalId: string,
  data: UpdateBusinessDTO,
): Promise<BusinessDetail> {
  try {
    const res = await authenticatedFetch(`${ROOT_PATH}/${externalId}`, {
      method: "PUT",
      body: JSON.stringify(data),
    })

    if (!res.ok) {
      await throwResponseError(res, "Error updating business")
    }

    return res.json()
  } catch (error) {
    console.error("[updateBusiness]", error)
    rethrowWithFallback(error, "Error updating business")
  }
}
