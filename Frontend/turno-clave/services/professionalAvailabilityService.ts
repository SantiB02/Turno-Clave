"use server"

import { apiRequest } from "@/lib/api/apiRequest"
import { authenticatedFetch } from "@/lib/api/authenticated-fetch"
import type {
  ProfessionalAvailability,
  UpdateProfessionalAvailabilitiesDTO,
} from "@/types/professional"

const ROOT_PATH = "/professionals"

export async function updateProfessionalAvailabilities(
  professionalExternalId: string,
  data: UpdateProfessionalAvailabilitiesDTO,
) {
  return apiRequest<ProfessionalAvailability[]>(() =>
    authenticatedFetch(
      `${ROOT_PATH}/${professionalExternalId}/availabilities`,
      {
        method: "PUT",
        body: JSON.stringify(data),
      },
    ),
  )
}
