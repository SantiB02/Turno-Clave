import { notFound } from "next/navigation"
import { cache } from "react"
import { getBusinessBySlug } from "@/services/public/publicBusinessService"
import type { PublicBusinessDetail } from "@/types/business"

export const getReservationBusiness = cache(
  async (slug: string): Promise<PublicBusinessDetail> => {
    const businessResult = await getBusinessBySlug(slug)

    if (!businessResult.ok) {
      notFound()
    }

    return businessResult.data
  },
)
