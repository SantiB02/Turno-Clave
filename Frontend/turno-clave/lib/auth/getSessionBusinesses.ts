import { auth, type ExtendedSession } from "@/auth"
import { getMyBusinesses } from "@/services/businessService"
import type { MinimalBusiness } from "@/types/business"

type SessionBusinessesResult = {
  session: ExtendedSession | null
  businesses: MinimalBusiness[]
  isVerified: boolean
}

export async function getSessionBusinesses(): Promise<SessionBusinessesResult> {
  const session = (await auth()) as ExtendedSession | null

  if (!session) {
    return {
      session: null,
      businesses: [],
      isVerified: false,
    }
  }

  if ((session.businesses?.length ?? 0) > 0) {
    return {
      session,
      businesses: session.businesses ?? [],
      isVerified: true,
    }
  }

  const businessesResult = await getMyBusinesses()

  if (!businessesResult.ok) {
    return {
      session,
      businesses: session.businesses ?? [],
      isVerified: false,
    }
  }

  return {
    session,
    businesses: businessesResult.data.map(({ externalId, name, slug }) => ({
      externalId,
      name,
      slug,
    })),
    isVerified: true,
  }
}
