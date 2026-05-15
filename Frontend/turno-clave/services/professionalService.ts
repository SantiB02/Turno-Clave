"use server"

import { apiRequest } from "@/lib/api/apiRequest"
import { authenticatedFetch } from "@/lib/api/authenticated-fetch"
import type {
  CreateProfessionalDTO,
  Professional,
  UpdateProfessionalDTO,
} from "@/types/professional"

const ROOT_PATH = "/professionals"

export async function getProfessionalsByActiveBusiness() {
  return apiRequest<Professional[]>(() =>
    authenticatedFetch(`${ROOT_PATH}/active-business`),
  )
}

export async function createProfessional(data: CreateProfessionalDTO) {
  return apiRequest<Professional>(() =>
    authenticatedFetch(`${ROOT_PATH}`, {
      method: "POST",
      body: JSON.stringify(data),
    }),
  )
}

export async function updateProfessional(
  externalId: string,
  data: UpdateProfessionalDTO,
) {
  return apiRequest<Professional>(() =>
    authenticatedFetch(`${ROOT_PATH}/${externalId}`, {
      method: "PUT",
      body: JSON.stringify(data),
    }),
  )
}

export async function deleteProfessional(externalId: string) {
  return apiRequest<Professional>(() =>
    authenticatedFetch(`${ROOT_PATH}/${externalId}`, {
      method: "DELETE",
    }),
  )
}
