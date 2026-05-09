"use server"

import { authenticatedFetch } from "@/lib/api/authenticated-fetch"
import type { CreateServiceDTO, Service } from "@/types/service"

const ROOT_PATH = "/services"

export async function createService(data: CreateServiceDTO) {
  const response = await authenticatedFetch(`${ROOT_PATH}`, {
    method: "POST",
    body: JSON.stringify(data),
  })

  if (!response.ok) {
    const errorText = await response.text()
    console.error(`[createService] Error: Status ${response.status}`, errorText)
    throw new Error(
      `Error creating service: ${response.status} ${errorText || "Unknown error"}`,
    )
  }

  return response.json()
}

export async function getServicesByActiveBusiness(): Promise<Service[]> {
  try {
    const res = await authenticatedFetch(`${ROOT_PATH}/active-business`)
    if (!res.ok) {
      console.error("Error fetching services:", res.status, await res.text())
      throw new Error("Error fetching services")
    }
    return res.json()
  } catch (error) {
    console.error("Error fetching services:", error)
    throw new Error("Error fetching services")
  }
}
