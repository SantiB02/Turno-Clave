"use server"

import { authenticatedFetch } from "@/lib/api/authenticated-fetch"
import type { BusinessDetail, CreateBusinessDTO } from "@/types/business"
import type { Service } from "@/types/service"

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
    console.error("Error fetching businesses:", error)
    throw new Error("Error fetching businesses")
  }
}

export async function getServicesByActiveBusiness(): Promise<Service[]> {
  try {
    const res = await authenticatedFetch("/businesses/active/services")
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
