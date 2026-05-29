"use client"

import {
  createContext,
  type Dispatch,
  type PropsWithChildren,
  type SetStateAction,
  useContext,
  useEffect,
  useState,
} from "react"
import type { PublicBusinessDetail } from "@/types/business"
import type {
  IAvailabilitySlot,
  IReservationConfirmationDetails,
} from "@/types/reservation"
import type { Service } from "@/types/service"

type StoredReservationFlow = {
  selectedServiceExternalIds: string[]
  selectedProfessionalsByService: Record<string, string | null>
  selectedSlot: IAvailabilitySlot | null
  confirmationDetails: IReservationConfirmationDetails | null
}

type ReservationFlowContextType = {
  business: PublicBusinessDetail
  slug: string
  isHydrated: boolean
  selectedServices: Service[]
  setSelectedServices: Dispatch<SetStateAction<Service[]>>
  selectedProfessionalsByService: Record<string, string | null>
  setSelectedProfessionalsByService: Dispatch<
    SetStateAction<Record<string, string | null>>
  >
  selectedSlot: IAvailabilitySlot | null
  setSelectedSlot: Dispatch<SetStateAction<IAvailabilitySlot | null>>
  clientEmail: string
  setClientEmail: Dispatch<SetStateAction<string>>
  confirmationDetails: IReservationConfirmationDetails | null
  setConfirmationDetails: Dispatch<
    SetStateAction<IReservationConfirmationDetails | null>
  >
}

const ReservationFlowContext = createContext<ReservationFlowContextType | null>(
  null,
)

type ReservationFlowProviderProps = PropsWithChildren<{
  business: PublicBusinessDetail
  slug: string
}>

function getStorageKey(slug: string) {
  return `reservation-flow:${slug}`
}

function getBusinessServices(business: PublicBusinessDetail) {
  return Array.from(
    new Map(
      business.professionals
        .flatMap((professional) => professional.services)
        .map((service) => [service.externalId, service]),
    ).values(),
  ) as Service[]
}

export function ReservationFlowProvider({
  business,
  slug,
  children,
}: ReservationFlowProviderProps) {
  const [selectedServices, setSelectedServices] = useState<Service[]>([])
  const [selectedProfessionalsByService, setSelectedProfessionalsByService] =
    useState<Record<string, string | null>>({})
  const [selectedSlot, setSelectedSlot] = useState<IAvailabilitySlot | null>(
    null,
  )
  const [isHydrated, setIsHydrated] = useState(false)
  const [clientEmail, setClientEmail] = useState("")
  const [confirmationDetails, setConfirmationDetails] =
    useState<IReservationConfirmationDetails | null>(null)

  useEffect(() => {
    const storedValue = window.sessionStorage.getItem(getStorageKey(slug))

    if (!storedValue) {
      setIsHydrated(true)
      return
    }

    try {
      const parsed = JSON.parse(storedValue) as StoredReservationFlow
      const businessServices = getBusinessServices(business)
      const selectedServiceIds = new Set(parsed.selectedServiceExternalIds)

      setSelectedServices(
        businessServices.filter((service) =>
          selectedServiceIds.has(service.externalId),
        ),
      )
      setSelectedProfessionalsByService(
        parsed.selectedProfessionalsByService ?? {},
      )
      setSelectedSlot(parsed.selectedSlot ?? null)
      setConfirmationDetails(parsed.confirmationDetails ?? null)
      setClientEmail(parsed.confirmationDetails?.clientEmail ?? "")
    } catch {
      window.sessionStorage.removeItem(getStorageKey(slug))
    } finally {
      setIsHydrated(true)
    }
  }, [business, slug])

  useEffect(() => {
    if (!isHydrated) {
      return
    }

    const dataToStore: StoredReservationFlow = {
      selectedServiceExternalIds: selectedServices.map(
        (service) => service.externalId,
      ),
      selectedProfessionalsByService,
      selectedSlot,
      confirmationDetails,
    }

    window.sessionStorage.setItem(
      getStorageKey(slug),
      JSON.stringify(dataToStore),
    )
  }, [
    isHydrated,
    selectedProfessionalsByService,
    selectedServices,
    selectedSlot,
    confirmationDetails,
    slug,
  ])

  return (
    <ReservationFlowContext.Provider
      value={{
        business,
        slug,
        isHydrated,
        selectedServices,
        setSelectedServices,
        selectedProfessionalsByService,
        setSelectedProfessionalsByService,
        selectedSlot,
        setSelectedSlot,
        clientEmail,
        setClientEmail,
        confirmationDetails,
        setConfirmationDetails,
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
