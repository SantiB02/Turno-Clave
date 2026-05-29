"use client"

import Calendar from "react-calendar"
import "react-calendar/dist/Calendar.css"
import { useRouter } from "next/navigation"
import { type FormEvent, useEffect, useState } from "react"
import Button from "@/app/components/Button"
import ModalForm from "@/app/components/ModalForm"
import { getAvailableSlots } from "@/services/public/publicAppointmentService"
import type { IAvailabilitySlot, ISelectionRequest } from "@/types/reservation"
import ReservationHeader from "../../ReservationHeader"
import { useReservationFlow } from "../ReservationFlowProvider"

function getInitialSearchRange() {
  const searchFromDate = new Date()
  const searchToDate = new Date()

  searchToDate.setDate(searchToDate.getDate() + 7)

  return {
    searchFromDate: searchFromDate.toISOString().split("T")[0],
    searchToDate: searchToDate.toISOString().split("T")[0],
  }
}

function getSlotIdentifier(slot: IAvailabilitySlot) {
  const serviceSignature = slot.serviceDetails
    .map(
      (service) =>
        `${service.serviceExternalId}-${service.assignedProfessionalExternalId ?? "no-professional"}-${service.serviceStartTime}-${service.serviceEndTime}`,
    )
    .join("|")

  return `${slot.date}-${slot.startTime}-${slot.endTime}-${serviceSignature}`
}

export function DateTimeSelection() {
  const router = useRouter()
  const {
    selectedServices,
    selectedProfessionalsByService,
    selectedSlot,
    setSelectedSlot,
    slug,
    business,
    isHydrated,
  } = useReservationFlow()
  const [searchRange] = useState(() => getInitialSearchRange())
  const SLOTS_PER_PAGE = 12

  const [currentPage, setCurrentPage] = useState(1)

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
  const [isLoadingSlots, setIsLoadingSlots] = useState(true)
  const [isLoadingPage, setIsLoadingPage] = useState(false)
  const [isModalOpen, setIsModalOpen] = useState(false)
  const [timeLeft, setTimeLeft] = useState("5:00")
  const [errorMessage, setErrorMessage] = useState<string | null>(null)

  useEffect(() => {
    async function fetchAvailableSlots() {
      if (!isHydrated) {
        return
      }

      setErrorMessage(null)

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
        setErrorMessage(response.message)
        setIsLoadingSlots(false)
        return
      }

      setAvailableSlots(response.data)
      setIsLoadingSlots(false)
    }

    fetchAvailableSlots()
  }, [
    business.externalId,
    isHydrated,
    router,
    searchRange.searchFromDate,
    searchRange.searchToDate,
    selectedProfessionalsByService,
    selectedServices,
    slug,
  ])

  useEffect(() => {
    if (availableSlots.availableSlots.length === 0) return

    const dateToUse =
      selectedSlot &&
      availableSlots.availableSlots.some(
        (slot) => getSlotIdentifier(slot) === getSlotIdentifier(selectedSlot),
      )
        ? selectedSlot.date
        : availableSlots.availableSlots[0].date

    const parsedDate = new Date(`${dateToUse}T00:00:00`)

    setSelectedDate(parsedDate)

    const slots = availableSlots.availableSlots.filter(
      (slot) => slot.date === dateToUse,
    )

    setSlotsForDate(slots)
  }, [availableSlots, selectedSlot])

  useEffect(() => {
    const targetTime = Date.now() + 5 * 60 * 1000 // 5 minutes from now

    const interval = setInterval(() => {
      const now = Date.now()
      const difference = targetTime - now

      if (difference <= 0) {
        setTimeLeft("00:00")
        clearInterval(interval)
        router.push(`/reservar/${slug}/servicios`)
      } else {
        const minutes = Math.floor(difference / (1000 * 60))
        const seconds = Math.floor((difference % (1000 * 60)) / 1000)
        setTimeLeft(
          `${minutes.toString().padStart(2, "0")}:${seconds
            .toString()
            .padStart(2, "0")}`,
        )
      }
    }, 1000)

    return () => clearInterval(interval)
  }, [router, slug])

  const handleDateClick = (date: Date) => {
    setSelectedSlot(null)
    setSelectedDate(date)
    setCurrentPage(1)

    const dateOnly = date.toISOString().split("T")[0]

    const slots = availableSlots.availableSlots.filter(
      (s) => s.date === dateOnly,
    )
    setSlotsForDate(slots)
  }

  const handleSlotClick = (_slot: IAvailabilitySlot) => {
    setSelectedSlot(_slot)
  }

  const getTileClass = ({ date }: { date: Date }) => {
    const dateStr = date.toISOString().split("T")[0]
    const hasSlots = availableSlots.availableSlots.some(
      (s) => s.date === dateStr,
    )
    return hasSlots ? "available-date" : ""
  }

  const handleClickButton = () => {
    setIsModalOpen(true)
  }

  const handleSubmit = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault()
    setIsLoadingPage(true)
    router.push(`/reservar/${business.slug}/datos-personales`)
  }

  const totalPages = Math.ceil(slotsForDate.length / SLOTS_PER_PAGE)

  const paginatedSlots = slotsForDate.slice(
    (currentPage - 1) * SLOTS_PER_PAGE,
    currentPage * SLOTS_PER_PAGE,
  )

  return (
    <>
      <ReservationHeader
        backButtonUrl={`/reservar/${slug}/servicios`}
        title="Elegí el día y la hora"
        currentStep={1}
      />
      <div className="flex flex-col items-center gap-6">
        {errorMessage && (
          <div className="w-full rounded-lg border border-red-200 bg-red-50 px-4 py-3 text-red-700">
            {errorMessage}
          </div>
        )}

        <div className="rounded-lg bg-stone-50 px-4 py-3 text-center text-md text-gray-700">
          <p>
            Tiempo restante:{" "}
            <span className="text-primary-orange font-bold">{timeLeft}</span>
          </p>
        </div>

        <Calendar
          className={`reservation-calendar unselectable ${
            isLoadingSlots ? "pointer-events-none opacity-60" : ""
          }`}
          locale="ES"
          onChange={(date) => {
            if (isLoadingSlots) return
            handleDateClick(date as Date)
          }}
          value={selectedDate}
          tileClassName={getTileClass}
          minDate={new Date(`${availableSlots.searchFromDate}T00:00:00`)}
          maxDate={new Date(`${availableSlots.searchToDate}T23:59:59`)}
        />

        {isLoadingSlots && <p>Cargando horarios disponibles...</p>}
        {!isLoadingSlots && slotsForDate.length === 0 && selectedDate && (
          <p className="text-gray-500">
            No hay horarios disponibles para esta fecha.
          </p>
        )}
        {slotsForDate.length > 0 && (
          <div className="w-full max-w-2xl unselectable rounded-3xl bg-stone-50 px-4 py-5 sm:px-6">
            <h3 className="mb-4 text-center text-lg font-semibold text-gray-900">
              Turnos disponibles para{" "}
              <span className="text-primary-orange">
                {selectedDate?.toLocaleDateString("es-AR", {
                  weekday: "long",
                  year: "numeric",
                  month: "long",
                  day: "numeric",
                })}
              </span>
            </h3>
            <div className="grid grid-cols-3 justify-center justify-items-center gap-4 sm:grid-cols-3 md:grid-cols-[repeat(4,minmax(0,0.2fr))]">
              {paginatedSlots.map((slot) => (
                <button
                  key={getSlotIdentifier(slot)}
                  type="button"
                  onClick={() => handleSlotClick(slot)}
                  className={`w-full max-w-[4.5rem] cursor-pointer rounded-lg border border-primary-orange px-4 py-3 text-center text-base font-semibold text-primary-orange shadow-sm transition hover:-translate-y-0.5 hover:bg-primary-orange hover:text-white ${
                    selectedSlot &&
                    getSlotIdentifier(selectedSlot) === getSlotIdentifier(slot)
                      ? "bg-primary-orange text-white"
                      : "bg-white"
                  }`}
                >
                  {/* We don't need to show seconds */}
                  {slot.startTime.split(":").slice(0, 2).join(":")}
                </button>
              ))}
            </div>
            {totalPages > 1 && (
              <div className="mt-5 flex items-center justify-center gap-3">
                <button
                  type="button"
                  disabled={currentPage === 1}
                  onClick={() => setCurrentPage((prev) => prev - 1)}
                  className="rounded-lg cursor-pointer disabled:cursor-default border border-gray-300 bg-white px-4 py-2 text-sm font-medium disabled:opacity-50"
                >
                  Anterior
                </button>

                <span className="text-sm font-medium text-gray-700">
                  Página {currentPage} de {totalPages}
                </span>

                <button
                  type="button"
                  disabled={currentPage === totalPages}
                  onClick={() => setCurrentPage((prev) => prev + 1)}
                  className="rounded-lg cursor-pointer disabled:cursor-default border border-gray-300 bg-white px-4 py-2 text-sm font-medium disabled:opacity-50"
                >
                  Siguiente
                </button>
              </div>
            )}
          </div>
        )}
        <div className="flex justify-center my-2">
          <Button
            onClick={handleClickButton}
            className="px-6"
            label={isLoadingPage ? "Cargando..." : "Continuar"}
            disabled={
              isLoadingPage || selectedServices.length === 0 || !selectedSlot
            }
            size="text-2xl"
          />
        </div>
      </div>
      <ModalForm
        title="Confirmar turno"
        open={isModalOpen}
        submitLabel="Confirmar"
        onSubmit={handleSubmit}
        onClose={() => setIsModalOpen(false)}
      >
        {selectedSlot && (
          <div className="space-y-2">
            <p>
              <span className="font-semibold">Fecha:</span>{" "}
              {selectedSlot.date.split("-").reverse().join("/")}
            </p>
            <p>
              <span className="font-semibold">Hora:</span>{" "}
              {selectedSlot.startTime.split(":").slice(0, 2).join(":")} -{" "}
              {selectedSlot.endTime.split(":").slice(0, 2).join(":")}
            </p>
            <div>
              <span className="font-semibold">Servicios:</span>
              <ul className="list-disc list-inside">
                {selectedSlot.serviceDetails.map((service) => (
                  <li
                    key={`${service.serviceExternalId}-${service.assignedProfessionalExternalId}`}
                  >
                    {service.serviceName}{" "}
                    {service.assignedProfessionalName &&
                      `(Profesional: ${service.assignedProfessionalName})`}
                  </li>
                ))}
              </ul>
            </div>
          </div>
        )}
      </ModalForm>
    </>
  )
}
