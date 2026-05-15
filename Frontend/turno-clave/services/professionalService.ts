"use server"

import { authenticatedFetch } from "@/lib/api/authenticated-fetch"
import { rethrowWithFallback, throwResponseError } from "@/lib/api/error-utils"
import type {
  CreateProfessionalDTO,
  Professional,
  UpdateProfessionalDTO,
} from "@/types/professional"

const ROOT_PATH = "/professionals"

export async function getProfessionalsByActiveBusiness(): Promise<
  Professional[]
> {
  try {
    const res = await authenticatedFetch(`${ROOT_PATH}/active-business`)

    if (!res.ok) {
      await throwResponseError(res, "Error obteniendo profesionales")
    }

    return res.json()
  } catch (error) {
    console.error("[getProfessionalsByActiveBusiness]", error)
    rethrowWithFallback(error, "Error obteniendo profesionales")
  }
}

export async function createProfessional(
  data: CreateProfessionalDTO,
): Promise<Professional> {
  try {
    const res = await authenticatedFetch(`${ROOT_PATH}`, {
      method: "POST",
      body: JSON.stringify(data),
    })

    if (!res.ok) {
      await throwResponseError(res, "Error creando professional")
    }

    return res.json()
  } catch (error) {
    console.error("[createProfessional]", error)
    rethrowWithFallback(error, "Error creando professional")
  }
}

export async function updateProfessional(
  externalId: string,
  data: UpdateProfessionalDTO,
): Promise<Professional> {
  try {
    const res = await authenticatedFetch(`${ROOT_PATH}/${externalId}`, {
      method: "PUT",
      body: JSON.stringify(data),
    })

    if (!res.ok) {
      await throwResponseError(res, "Error actualizando profesional")
    }

    return res.json()
  } catch (error) {
    console.error("[updateProfessional]", error)
    rethrowWithFallback(error, "Error actualizando profesional")
  }
}
