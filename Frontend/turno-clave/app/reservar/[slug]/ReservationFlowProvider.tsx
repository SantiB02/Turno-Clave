"use client"

import {
  createContext,
  type Dispatch,
  type PropsWithChildren,
  type SetStateAction,
  useContext,
  useState,
} from "react"
import type { PublicBusinessDetail } from "@/types/business"
import type { Service } from "@/types/service"

type ReservationFlowContextType = {
  business: PublicBusinessDetail
  slug: string
  selectedServices: Service[]
  setSelectedServices: Dispatch<SetStateAction<Service[]>>
  selectedProfessionalsByService: Record<string, string | null>
  setSelectedProfessionalsByService: Dispatch<
    SetStateAction<Record<string, string | null>>
  >
}

const ReservationFlowContext = createContext<ReservationFlowContextType | null>(
  null,
)

type ReservationFlowProviderProps = PropsWithChildren<{
  business: PublicBusinessDetail
  slug: string
}>

export function ReservationFlowProvider({
  business,
  slug,
  children,
}: ReservationFlowProviderProps) {
  const [selectedServices, setSelectedServices] = useState<Service[]>([])
  const [selectedProfessionalsByService, setSelectedProfessionalsByService] =
    useState<Record<string, string | null>>({})

  return (
    <ReservationFlowContext.Provider
      value={{
        business,
        slug,
        selectedServices,
        setSelectedServices,
        selectedProfessionalsByService,
        setSelectedProfessionalsByService,
      }}
    >
      {children}
    </ReservationFlowContext.Provider>
  )
}

export function useReservationFlow() {
  const context = useContext(ReservationFlowContext)

  if (!context) {
    throw new Error(
      "useReservationFlow debe usarse dentro de ReservationFlowProvider",
    )
  }

  return context
}
