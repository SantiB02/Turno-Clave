import type { ServiceProfessional } from "./professional"

export type Service = {
  externalId: string
  name: string
  description: string
  price: number
  durationMinutes: number
  professionals: ServiceProfessional[]
}

export type MinimalService = {
  externalId: string
  name: string
  description: string
  price: number
  durationMinutes: number
}

export type CreateServiceDTO = {
  name: string
  description: string
  professionalExternalIds: string[]
  price: number
  durationMinutes: number
}

export type UpdateServiceDTO = CreateServiceDTO
