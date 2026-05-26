"use server"

import { auth, type ExtendedSession, unstable_update } from "@/auth"
import type { MinimalBusiness } from "@/types/business"

export async function addBusinessToSession(business: MinimalBusiness) {
  const session = (await auth()) as ExtendedSession | null

  if (!session) {
    return null
  }

  const businesses = session.businesses ?? []
  const alreadyExists = businesses.some(
    ({ externalId }) => externalId === business.externalId,
  )

  return unstable_update({
    businesses: alreadyExists ? businesses : [...businesses, business],
  })
}
