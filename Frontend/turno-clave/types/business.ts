import type { PaymmentMethod } from "@/enums/paymentMethods"
import type { Professional } from "./professional"

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
  paymentMethods: PaymmentMethod[]
  address: string
  city: string
  state: string
  country: string
  isPublicLinkEnabled: boolean
  availabilities: BusinessAvailabilityDTO[]
}

export type PublicBusinessDetail = {
  externalId: string
  name: string
  slug: string
  description: string
  logoUrl: string
  email: string
  phone: string
  paymentMethods: PaymmentMethod[]
  address: string
  city: string
  state: string
  country: string
  isPublicLinkEnabled: boolean
  availabilities: BusinessAvailabilityDTO[]
  professionals: Professional[]
}

export type CreateBusinessDTO = {
  name: string
  description?: string
  logoUrl?: string
  email: string
  phone: string
  country: string
  state: string
  city: string
  address: string
  timeZone: string
  availabilities: CreateBusinessAvailabilityDTO[]
}

export type UpdateBusinessDTO = {
  name: string
  description: string
  paymentMethods: PaymmentMethod[]
  phone: string
  country: string
  state: string
  city: string
  address: string
  timeZone: string
  availabilities?: CreateBusinessAvailabilityDTO[]
}

export type UpdateBusinessPublicLinkStatusDTO = {
  isPublicLinkEnabled: boolean
}

// ----- BUSINESS AVAILABILITY -----

export type CreateBusinessAvailabilityDTO = {
  dayOfWeek: number
  startTime: string
  endTime: string
}

export type UpdateBusinessAvailabilityDTO = CreateBusinessAvailabilityDTO

export type UpdateBusinessAvailabilitiesDTO = {
  availabilities: UpdateBusinessAvailabilityDTO[]
}

export type BusinessAvailabilityDTO = {
  externalId: string
  dayOfWeek: number | string
  startTime: string
  endTime: string
}

export type ShiftKey = "morning" | "afternoon"

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
