import type { MinimalService } from "./service"

export type Professional = {
  externalId: string
  name: string
  businessExternalId: string
  businessName: string
  availabilities: NestedProfessionalAvailability[]
  services: MinimalService[]
}

export type CreateProfessionalDTO = {
  name: string
  serviceExternalIds: string[]
}

export type UpdateProfessionalDTO = CreateProfessionalDTO

export type ServiceProfessional = {
  externalId: string
  name: string
}

// ----- PROFESSIONAL AVAILABILITY -----

export type NestedProfessionalAvailability = {
  externalId: string
  dayOfWeek: number | string
  startTime: string
  endTime: string
}

export type ProfessionalAvailability = NestedProfessionalAvailability & {
  professionalExternalId: string
}

export type UpdateProfessionalAvailabilityDTO = {
  dayOfWeek: number
  startTime: string
  endTime: string
}

export type UpdateProfessionalAvailabilitiesDTO = {
  availabilities: UpdateProfessionalAvailabilityDTO[]
}
