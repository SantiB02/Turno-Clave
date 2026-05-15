"use server"

import { apiRequest } from "@/lib/api/apiRequest"
import { authenticatedFetch } from "@/lib/api/authenticated-fetch"
import type {
  BusinessDetail,
  CreateBusinessDTO,
  MinimalBusiness,
  UpdateBusinessDTO,
} from "@/types/business"

const ROOT_PATH = "/businesses"

export async function createBusiness(data: CreateBusinessDTO) {
  return apiRequest<MinimalBusiness>(() =>
    authenticatedFetch(`${ROOT_PATH}`, {
      method: "POST",
      body: JSON.stringify(data),
    }),
  )
}

export async function getMyBusinesses() {
  return apiRequest<BusinessDetail[]>(() =>
    authenticatedFetch(`${ROOT_PATH}/mine`),
  )
}

export async function getActiveBusiness() {
  return apiRequest<BusinessDetail>(() =>
    authenticatedFetch(`${ROOT_PATH}/active`),
  )
}

export async function updateBusiness(
  externalId: string,
  data: UpdateBusinessDTO,
) {
  return apiRequest<BusinessDetail>(() =>
    authenticatedFetch(`${ROOT_PATH}/${externalId}`, {
      method: "PUT",
      body: JSON.stringify(data),
    }),
  )
}
