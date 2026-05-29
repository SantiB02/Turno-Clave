import { apiRequest } from "@/lib/api/apiRequest"
import type {
  IAvailabilitySlotsResponse,
  ICreateAppointment,
  ISelectionRequest,
} from "@/types/reservation"

const ROOT_PATH = "/api/appointments/public"

export async function getAvailableSlots(request: ISelectionRequest) {
  return apiRequest<IAvailabilitySlotsResponse>(() =>
    fetch(`${ROOT_PATH}/available-slots`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(request),
    }),
  )
}

export async function createAppointment(request: ICreateAppointment) {
  return apiRequest<unknown>(() =>
    fetch(`${ROOT_PATH}`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(request),
    }),
  )
}
