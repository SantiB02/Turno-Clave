export type Professional = {
  externalId: string
  name: string
  businessExternalId: string
  businessName: string
  availabilities: NestedProfessionalAvailability[]
}

export type ServiceProfessional = {
  externalId: string
  name: string
}

// ----- PROFESSIONAL AVAILABILITY -----

export type NestedProfessionalAvailability = {
  externalId: string
  dayOfWeek: number
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
