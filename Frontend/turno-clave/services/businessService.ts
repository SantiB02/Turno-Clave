"use server"

import { authenticatedFetch } from "@/lib/api/authenticated-fetch"
import type { BusinessDetail, CreateBusinessDTO } from "@/types/business"

export async function createBusiness(data: CreateBusinessDTO) {
  const res = await authenticatedFetch("/businesses", {
    method: "POST",
    body: JSON.stringify(data),
  })

  if (!res.ok) {
    const errorText = await res.text()
    console.error(`[createBusiness] Error: Status ${res.status}`, errorText)
    throw new Error(
      `Error creating business: ${res.status} ${errorText || "Unknown error"}`,
    )
  }

  return res.json()
}

export async function getMyBusinesses(): Promise<BusinessDetail[]> {
  try {
    const res = await authenticatedFetch("/businesses/mine")

    if (!res.ok) {
      console.error("Error fetching businesses:", res.status, await res.text())
      throw new Error("Error fetching businesses")
    }

    return res.json()
  } catch (error) {
    if (error instanceof Error && error.message === "UNAUTHORIZED") {
      throw new Error("UNAUTHORIZED")
    }
    throw error
  }
}
