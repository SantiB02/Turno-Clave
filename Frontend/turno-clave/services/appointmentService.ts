"use server"

import { apiRequest } from "@/lib/api/apiRequest"
import { authenticatedFetch } from "@/lib/api/authenticated-fetch"
import type { Appointment } from "@/types/appointment"

const ROOT_PATH = "/appointments"

function toQueryDate(date: Date | string) {
  return typeof date === "string" ? date : date.toISOString()
}

export async function getMyAppointments(
  fromDate: Date | string,
  toDate: Date | string,
) {
  const params = new URLSearchParams({
    fromDate: toQueryDate(fromDate),
    toDate: toQueryDate(toDate),
  })

  return apiRequest<Appointment[]>(() =>
    authenticatedFetch(`${ROOT_PATH}/mine?${params.toString()}`),
  )
}
