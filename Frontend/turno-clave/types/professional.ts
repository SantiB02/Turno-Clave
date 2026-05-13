export type Professional = {
  externalId: string
  name: string
  businessExternalId: string
  businessName: string
}

export type ServiceProfessional = {
  externalId: string
  name: string
}

export type NestedProfessionalAvailability = {
  externalId: string
  dayOfWeek: number
  startTime: string
  endTime: string
}

export type ProfessionalAvailability = NestedProfessionalAvailability & {
  professionalExternalId: string
}
