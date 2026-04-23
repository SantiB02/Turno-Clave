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
}

export type CreateBusinessFromOnboardingDTO = {
  name: string
  country: string
  state: string
  city: string
  address: string
  phone: string
  email: string
  timeZone: string
}
