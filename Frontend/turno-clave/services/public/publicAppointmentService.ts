import { apiFetch } from "@/lib/api/apiClient"
import { apiRequest } from "@/lib/api/apiRequest"
import type {
  IAvailabilitySlotsResponse,
  ISelectionRequest,
} from "@/types/reservation"

const ROOT_PATH = "/api/appointments/public"

export async function getAvailableSlots(request: ISelectionRequest) {
  return apiRequest<IAvailabilitySlotsResponse>(() =>
    apiFetch(`${ROOT_PATH}/available-slots`, undefined, {
      method: "POST",
      body: JSON.stringify(request),
    }),
  )
}
