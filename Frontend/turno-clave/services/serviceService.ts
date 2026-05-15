"use server"

import { apiRequest } from "@/lib/api/apiRequest"
import { authenticatedFetch } from "@/lib/api/authenticated-fetch"
import type {
  CreateServiceDTO,
  Service,
  UpdateServiceDTO,
} from "@/types/service"

const ROOT_PATH = "/services"

export async function createService(data: CreateServiceDTO) {
  return apiRequest<Service>(() =>
    authenticatedFetch(`${ROOT_PATH}`, {
      method: "POST",
      body: JSON.stringify(data),
    }),
  )
}

export async function updateService(
  externalId: string,
  data: UpdateServiceDTO,
) {
  return apiRequest<Service>(() =>
    authenticatedFetch(`${ROOT_PATH}/${externalId}`, {
      method: "PUT",
      body: JSON.stringify(data),
    }),
  )
}

export async function getServicesByActiveBusiness() {
  return apiRequest<Service[]>(() =>
    authenticatedFetch(`${ROOT_PATH}/active-business`),
  )
}

export async function deleteService(externalId: string) {
  return apiRequest<Service>(() =>
    authenticatedFetch(`${ROOT_PATH}/${externalId}`, {
      method: "DELETE",
    }),
  )
}
