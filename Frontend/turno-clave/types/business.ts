export type MinimalBusiness = {
  externalId: string
  name: string
  slug: string
}

export type BusinessDetail = {
  externalId: string
  name: string
  slug: string
  description: string
  logoUrl: string
  email: string
  phone: string
  address: string
  city: string
  state: string
  country: string
}

export type BusinessAvailability = {
  dayOfWeek: number
  startTime: string
  endTime: string
}

export type WeekAvailability = {
  [key: string]: {
    enabled: boolean
    start: string
    end: string
  }
}

export type CreateBusinessDTO = {
  name: string
  description?: string
  logoUrl?: string
  email: string
  phone: string
  address: string
  city: string
  state: string
  country: string
  timeZone: string
  availabilities: BusinessAvailability[]
}
