"use server"

import { authenticatedFetch } from "@/lib/api/authenticated-fetch"
import type { CreateServiceDTO } from "@/types/service"

export async function createService(data: CreateServiceDTO) {
  const response = await authenticatedFetch("/services", {
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
