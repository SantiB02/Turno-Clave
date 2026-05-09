export type Service = {
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
