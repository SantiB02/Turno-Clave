"use client"

import { createContext, type PropsWithChildren, useContext } from "react"
import type { PublicBusinessDetail } from "@/types/business"

const ReservationBusinessContext = createContext<PublicBusinessDetail | null>(
  null,
)

type ReservationBusinessProviderProps = PropsWithChildren<{
  business: PublicBusinessDetail
}>

export function ReservationBusinessProvider({
  business,
  children,
}: ReservationBusinessProviderProps) {
  return (
    <ReservationBusinessContext.Provider value={business}>
      {children}
    </ReservationBusinessContext.Provider>
  )
}

export function useReservationBusiness() {
  const business = useContext(ReservationBusinessContext)

  if (!business) {
    throw new Error(
      "useReservationBusiness debe usarse dentro de ReservationBusinessProvider",
    )
  }

  return business
}
