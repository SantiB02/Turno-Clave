"use server"

import { authenticatedFetch } from "@/lib/api/authenticated-fetch"
import { rethrowWithFallback, throwResponseError } from "@/lib/api/error-utils"
import type { CreateServiceDTO, Service, UpdateServiceDTO } from "@/types/service"

const ROOT_PATH = "/services"

export async function createService(data: CreateServiceDTO): Promise<Service> {
  try {
    const response = await authenticatedFetch(`${ROOT_PATH}`, {
      method: "POST",
      body: JSON.stringify(data),
    })

    if (!response.ok) {
      await throwResponseError(response, "Error creating service")
    }

    return response.json()
  } catch (error) {
    console.error("[createService]", error)
    rethrowWithFallback(error, "Error creating service")
  }
}

export async function updateService(
  externalId: string,
  data: UpdateServiceDTO,
): Promise<Service> {
  try {
    const response = await authenticatedFetch(`${ROOT_PATH}/${externalId}`, {
      method: "PUT",
      body: JSON.stringify(data),
    })

    if (!response.ok) {
      await throwResponseError(response, "Error updating service")
    }

    return response.json()
  } catch (error) {
    console.error("[updateService]", error)
    rethrowWithFallback(error, "Error updating service")
  }
}

export async function getServicesByActiveBusiness(): Promise<Service[]> {
  try {
    const res = await authenticatedFetch(`${ROOT_PATH}/active-business`)

    if (!res.ok) {
      await throwResponseError(res, "Error fetching services")
    }

    return res.json()
  } catch (error) {
    console.error("[getServicesByActiveBusiness]", error)
    rethrowWithFallback(error, "Error fetching services")
  }
}

export async function deleteService(externalId: string) {
  try {
    const res = await authenticatedFetch(`${ROOT_PATH}/${externalId}`, {
      method: "DELETE",
    })

    if (!res.ok) {
      await throwResponseError(res, "Error deleting service")
    }
  } catch (error) {
    console.error("[deleteService]", error)
    rethrowWithFallback(error, "Error deleting service")
  }
}
