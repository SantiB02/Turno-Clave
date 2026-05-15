"use server"

import { authenticatedFetch } from "@/lib/api/authenticated-fetch"
import { rethrowWithFallback, throwResponseError } from "@/lib/api/error-utils"
import type {
  BusinessAvailabilityDTO,
  UpdateBusinessAvailabilitiesDTO,
} from "@/types/business"

const ROOT_PATH = "/businesses"

export async function updateBusinessAvailabilities(
  businessExternalId: string,
  data: UpdateBusinessAvailabilitiesDTO,
): Promise<BusinessAvailabilityDTO[]> {
  try {
    const res = await authenticatedFetch(
      `${ROOT_PATH}/${businessExternalId}/availabilities`,
      {
        method: "PUT",
        body: JSON.stringify(data),
      },
    )

    if (!res.ok) {
      await throwResponseError(
        res,
        "Error actualizando disponibilidades de negocio",
      )
    }

    return res.json()
  } catch (error) {
    console.error("[updateBusinessAvailabilities]", error)
    rethrowWithFallback(error, "Error actualizando disponibilidades de negocio")
  }
}
