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
  day: number
  startTime: string
  endTime: string
}

export type WeekAvailability = {
  [key: string]: {
    enabled: boolean
    morning: {
      enabled: boolean
      start: string
      end: string
    }
    afternoon: {
      enabled: boolean
      start: string
      end: string
    }
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
