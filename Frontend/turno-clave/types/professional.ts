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

export type ProfessionalAvailability = {
  externalId: string
  dayOfWeek: number
  startTime: string
  endTime: string
}
