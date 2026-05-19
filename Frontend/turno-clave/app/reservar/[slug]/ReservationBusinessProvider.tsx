"use client"

import { createContext, type PropsWithChildren, useContext } from "react"
import type { PublicBusinessDetail } from "@/types/business"

type ReservationBusinessContextType = {
  business: PublicBusinessDetail
  slug: string
}

const ReservationBusinessContext =
  createContext<ReservationBusinessContextType | null>(null)

type ReservationBusinessProviderProps = PropsWithChildren<{
  business: PublicBusinessDetail
  slug: string
}>

export function ReservationBusinessProvider({
  business,
  slug,
  children,
}: ReservationBusinessProviderProps) {
  return (
    <ReservationBusinessContext.Provider
      value={{
        business,
        slug,
      }}
    >
      {children}
    </ReservationBusinessContext.Provider>
  )
}

export function useReservationBusiness() {
  const context = useContext(ReservationBusinessContext)

  if (!context) {
    throw new Error(
      "useReservationBusiness debe usarse dentro de ReservationBusinessProvider",
    )
  }

  return context
}
