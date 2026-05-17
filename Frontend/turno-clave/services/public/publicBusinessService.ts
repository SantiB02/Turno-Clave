"use server"

import { apiFetch } from "@/lib/api/apiClient"
import { apiRequest } from "@/lib/api/apiRequest"
import type { PublicBusinessDetail } from "@/types/business"

const ROOT_PATH = "/businesses/public"

export async function getBusinessBySlug(slug: string) {
  return apiRequest<PublicBusinessDetail>(() =>
    apiFetch(`${ROOT_PATH}/${slug}`),
  )
}
