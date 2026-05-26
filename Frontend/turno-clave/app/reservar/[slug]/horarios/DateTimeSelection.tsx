"use client"

import Calendar from "react-calendar"
import "react-calendar/dist/Calendar.css"
import { useRouter } from "next/navigation"
import { useEffect, useState } from "react"
import { getAvailableSlots } from "@/services/public/publicAppointmentService"
import type { IAvailabilitySlot, ISelectionRequest } from "@/types/reservation"
import { useReservationFlow } from "../ReservationFlowProvider"

function getInitialSearchRange() {
  const searchFromDate = new Date()
  const searchToDate = new Date()

  searchToDate.setDate(searchToDate.getDate() + 30)

  return {
    searchFromDate: searchFromDate.toISOString().split("T")[0],
    searchToDate: searchToDate.toISOString().split("T")[0],
  }
}

export function DateTimeSelection() {
  const router = useRouter()
  const { selectedServices, selectedProfessionalsByService, slug, business } =
    useReservationFlow()
  const [searchRange] = useState(() => getInitialSearchRange())

  const [availableSlots, setAvailableSlots] = useState<{
    searchFromDate: string
    searchToDate: string
    availableSlots: IAvailabilitySlot[]
  }>(() => {
    return {
      ...searchRange,
      availableSlots: [],
    }
  })
  const [selectedDate, setSelectedDate] = useState<Date | null>(null)
  const [slotsForDate, setSlotsForDate] = useState<IAvailabilitySlot[]>([])
  const [isLoadingSlots, setIsLoadingSlots] = useState(false)

  useEffect(() => {
    async function fetchAvailableSlots() {
      setIsLoadingSlots(true)

      if (selectedServices.length === 0 || !business.externalId) {
        router.push(`/reservar/${slug}/servicios`)
        return
      }

      const request: ISelectionRequest = {
        businessExternalId: business.externalId,
        services: selectedServices.map((service) => ({
          serviceExternalId: service.externalId,
          professionalExternalId:
            selectedProfessionalsByService[service.externalId] ?? null,
        })),
        searchFromDate: searchRange.searchFromDate,
        searchToDate: searchRange.searchToDate,
      }
      const response = await getAvailableSlots(request)
      if (!response.ok) {
        router.push(`/reservar/${slug}/servicios`)
        return
      }

      setAvailableSlots(response.data)
    }

    console.log("SELECTED SERVICES AND PROFESSIONALS:", {
      selectedServices,
      selectedProfessionalsByService,
    })
    // fetchAvailableSlots()
  }, [
    business.externalId,
    router,
    searchRange.searchFromDate,
    searchRange.searchToDate,
    selectedProfessionalsByService,
    selectedServices,
    slug,
  ])

  const handleDateClick = (date: Date) => {
    setSelectedDate(date)
    // we must avoid toISOString because it converts the date to UTC, we need the local date only
    const dateOnly = date.toISOString().split("T")[0]
    const slots = availableSlots.availableSlots.filter(
      (s) => s.date === dateOnly,
    )
    setSlotsForDate(slots)
  }

  const handleSlotClick = (_slot: IAvailabilitySlot) => {
    // Handle slot selection logic here
  }

  const getTileClass = ({ date }: { date: Date }) => {
    const dateStr = date.toISOString().split("T")[0]
    const hasSlots = availableSlots.availableSlots.some(
      (s) => s.date === dateStr,
    )
    return hasSlots ? "available-date" : ""
  }

  return (
    <div className="flex flex-col items-center gap-6">
      <h2>Selecciona una fecha</h2>
      <Calendar
        locale="ES"
        onChange={(date) => handleDateClick(date as Date)}
        value={selectedDate}
        tileClassName={getTileClass}
        minDate={new Date(`${availableSlots.searchFromDate}T00:00:00`)}
        maxDate={new Date(`${availableSlots.searchToDate}T23:59:59`)}
      />

      {slotsForDate.length > 0 && (
        <div className="slots-container">
          <h3>
            Horarios disponibles para {selectedDate?.toLocaleDateString()}
          </h3>
          {slotsForDate.map((slot) => (
            <div key={`${slot.date}-${slot.startTime}`} className="slot-card">
              <div>
                <strong>
                  {slot.startTime} - {slot.endTime}
                </strong>
                <p className="duration">{slot.totalDurationMinutes} minutos</p>
              </div>
              <div className="slot-details">
                {slot.serviceDetails.map((service) => (
                  <p key={service.serviceExternalId}>
                    {service.serviceName} ({service.durationMinutes}min)
                    {service.assignedProfessionalName && (
                      <span> - {service.assignedProfessionalName}</span>
                    )}
                  </p>
                ))}
              </div>
              <button type="button" onClick={() => handleSlotClick(slot)}>
                Seleccionar
              </button>
            </div>
          ))}
        </div>
      )}
    </div>
  )
}
