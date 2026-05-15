"use server"

import { apiRequest } from "@/lib/api/apiRequest"
import { authenticatedFetch } from "@/lib/api/authenticated-fetch"
import type { ApiResult } from "@/types/apiResult"
import type {
  BusinessAvailabilityDTO,
  UpdateBusinessAvailabilitiesDTO,
} from "@/types/business"

const ROOT_PATH = "/businesses"

export async function updateBusinessAvailabilities(
  businessExternalId: string,
  data: UpdateBusinessAvailabilitiesDTO,
): Promise<ApiResult<BusinessAvailabilityDTO[]>> {
  return apiRequest<BusinessAvailabilityDTO[]>(() =>
    authenticatedFetch(`${ROOT_PATH}/${businessExternalId}/availabilities`, {
      method: "PUT",
      body: JSON.stringify(data),
    }),
  )
}
