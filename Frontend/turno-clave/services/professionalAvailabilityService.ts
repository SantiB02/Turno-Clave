"use server"

import { authenticatedFetch } from "@/lib/api/authenticated-fetch"
import { rethrowWithFallback, throwResponseError } from "@/lib/api/error-utils"
import type {
  ProfessionalAvailability,
  UpdateProfessionalAvailabilitiesDTO,
} from "@/types/professional"

const ROOT_PATH = "/professionals"

export async function updateProfessionalAvailabilities(
  professionalExternalId: string,
  data: UpdateProfessionalAvailabilitiesDTO,
): Promise<ProfessionalAvailability[]> {
  try {
    const res = await authenticatedFetch(
      `${ROOT_PATH}/${professionalExternalId}/availabilities`,
      {
        method: "PUT",
        body: JSON.stringify(data),
      },
    )

    if (!res.ok) {
      await throwResponseError(
        res,
        "Error updating professional availabilities",
      )
    }

    return res.json()
  } catch (error) {
    console.error("[updateProfessionalAvailabilities]", error)
    rethrowWithFallback(error, "Error updating professional availabilities")
  }
}
