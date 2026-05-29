"use client"

import { useRouter } from "next/navigation"
import { type ChangeEvent, useEffect, useState } from "react"
import Button from "@/app/components/Button"
import { createAppointment } from "@/services/public/publicAppointmentService"
import type { IClientInfo, ICreateAppointment } from "@/types/reservation"
import ReservationHeader from "../../ReservationHeader"
import { useReservationFlow } from "../ReservationFlowProvider"

type PersonalDataForm = {
  name: string
  email: string
  phone: string
  notes: string
}

const INITIAL_FORM: PersonalDataForm = {
  name: "",
  email: "",
  phone: "",
  notes: "",
}

function buildUtcDateTime(date: string, time: string) {
  return new Date(`${date}T${time}`).toISOString()
}

export default function ReservaDatosPersonales() {
  const router = useRouter()
  const {
    slug,
    business,
    isHydrated,
    selectedServices,
    selectedSlot,
    setSelectedServices,
    setSelectedProfessionalsByService,
    setSelectedSlot,
    setClientEmail,
    setConfirmationDetails,
  } = useReservationFlow()
  const [formData, setFormData] = useState(INITIAL_FORM)
  const [errorMessage, setErrorMessage] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [isReservationCompleted, setIsReservationCompleted] = useState(false)

  useEffect(() => {
    if (!isHydrated || isReservationCompleted) {
      return
    }

    if (selectedServices.length === 0) {
      router.push(`/reservar/${slug}/servicios`)
      return
    }

    if (!selectedSlot) {
      router.push(`/reservar/${slug}/horarios`)
    }
  }, [
    isHydrated,
    isReservationCompleted,
    router,
    selectedServices.length,
    selectedSlot,
    slug,
  ])

  const handleChange = (
    event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
  ) => {
    const { name, value } = event.target

    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }))
  }

  const handleSubmit = async (event: React.SubmitEvent<HTMLFormElement>) => {
    event.preventDefault()

    if (!selectedSlot) {
      setErrorMessage("Primero tenés que seleccionar un horario.")
      return
    }

    const missingProfessional = selectedSlot.serviceDetails.some(
      (service) => !service.assignedProfessionalExternalId,
    )

    if (missingProfessional) {
      setErrorMessage(
        "No pudimos identificar el profesional de uno de los servicios. Volvé a elegir el horario.",
      )
      return
    }

    const appointmentItems = selectedSlot.serviceDetails.map((service) => ({
      serviceExternalId: service.serviceExternalId,
      professionalExternalId: service.assignedProfessionalExternalId ?? "",
      startTime: service.serviceStartTime,
      endTime: service.serviceEndTime,
      notes: null,
    }))

    setIsSubmitting(true)
    setErrorMessage(null)

    const client: IClientInfo = {
      name: formData.name.trim(),
      email: formData.email.trim(),
      phone: formData.phone.trim(),
      notes: formData.notes.trim() || null,
    }

    const payload: ICreateAppointment = {
      businessExternalId: business.externalId,
      client,
      startDateTime: buildUtcDateTime(
        selectedSlot.date,
        selectedSlot.startTime,
      ),
      endDateTime: buildUtcDateTime(selectedSlot.date, selectedSlot.endTime),
      items: appointmentItems,
      notes: formData.notes.trim() || null,
    }

    const response = await createAppointment(payload)

    if (!response.ok) {
      setErrorMessage(response.message)
      setIsSubmitting(false)
      return
    }

    const responseData =
      response.data && typeof response.data === "object" ? response.data : null
    const reservationCode =
      responseData &&
      "reservationCode" in responseData &&
      typeof responseData.reservationCode === "string"
        ? responseData.reservationCode
        : responseData &&
            "code" in responseData &&
            typeof responseData.code === "string"
          ? responseData.code
          : responseData &&
              "confirmationCode" in responseData &&
              typeof responseData.confirmationCode === "string"
            ? responseData.confirmationCode
            : null

    setClientEmail(client.email)
    setConfirmationDetails({
      clientEmail: client.email,
      reservationCode,
      date: selectedSlot.date,
      startTime: selectedSlot.startTime,
      endTime: selectedSlot.endTime,
      services: selectedSlot.serviceDetails.map((service) => ({
        serviceExternalId: service.serviceExternalId,
        serviceName: service.serviceName,
        professionalName: service.assignedProfessionalName ?? null,
        startTime: service.serviceStartTime,
        endTime: service.serviceEndTime,
      })),
    })
    setIsReservationCompleted(true)
    setSelectedServices([])
    setSelectedProfessionalsByService({})
    setSelectedSlot(null)
    setFormData(INITIAL_FORM)
    setIsSubmitting(false)

    router.replace(`/reservar/${slug}/listo`)
  }

  if (
    !isHydrated ||
    (!isReservationCompleted &&
      (selectedServices.length === 0 || !selectedSlot))
  ) {
    return null
  }

  return (
    <div className="m-4">
      <div className="mx-auto max-w-xl">
        <ReservationHeader
          currentStep={2}
          title="Cargá tus datos"
          backButtonUrl={`/reservar/${slug}/horarios`}
        />

        <div className="space-y-6 rounded-3xl bg-stone-50 p-6 shadow-sm">
          <div className="space-y-2 rounded-2xl bg-white p-4">
            <h2 className="text-lg font-semibold text-gray-900">
              Resumen del turno
            </h2>
            <p className="text-sm text-gray-700">
              <span className="font-semibold">Fecha:</span>{" "}
              {selectedSlot?.date.split("-").reverse().join("/")}
            </p>
            <p className="text-sm text-gray-700">
              <span className="font-semibold">Hora:</span>{" "}
              {selectedSlot?.startTime.slice(0, 5)} -{" "}
              {selectedSlot?.endTime.slice(0, 5)}
            </p>
          </div>

          {errorMessage && (
            <div className="rounded-xl border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
              {errorMessage}
            </div>
          )}

          <form onSubmit={handleSubmit} className="space-y-4">
            <div className="space-y-1">
              <label
                htmlFor="name"
                className="text-sm font-medium text-gray-800"
              >
                Nombre y apellido
              </label>
              <input
                id="name"
                name="name"
                type="text"
                value={formData.name}
                onChange={handleChange}
                required
                autoComplete="name"
                className="w-full rounded-xl border border-gray-300 bg-white px-4 py-3 text-gray-900 outline-none transition focus:border-primary-orange"
              />
            </div>

            <div className="space-y-1">
              <label
                htmlFor="email"
                className="text-sm font-medium text-gray-800"
              >
                Email
              </label>
              <input
                id="email"
                name="email"
                type="email"
                value={formData.email}
                onChange={handleChange}
                required
                autoComplete="email"
                className="w-full rounded-xl border border-gray-300 bg-white px-4 py-3 text-gray-900 outline-none transition focus:border-primary-orange"
              />
            </div>

            <div className="space-y-1">
              <label
                htmlFor="phone"
                className="text-sm font-medium text-gray-800"
              >
                Teléfono
              </label>
              <input
                id="phone"
                name="phone"
                type="tel"
                value={formData.phone}
                onChange={handleChange}
                required
                autoComplete="tel"
                className="w-full rounded-xl border border-gray-300 bg-white px-4 py-3 text-gray-900 outline-none transition focus:border-primary-orange"
              />
            </div>

            <div className="space-y-1">
              <label
                htmlFor="notes"
                className="text-sm font-medium text-gray-800"
              >
                Notas adicionales
              </label>
              <textarea
                id="notes"
                name="notes"
                value={formData.notes}
                onChange={handleChange}
                rows={4}
                placeholder="Opcional"
                className="w-full rounded-xl border border-gray-300 bg-white px-4 py-3 text-gray-900 outline-none transition focus:border-primary-orange"
              />
            </div>

            {isSubmitting && (
              <div className="rounded-xl border border-blue-200 bg-blue-50 px-4 py-3 text-sm text-blue-700">
                Procesando tu reserva...
              </div>
            )}

            <div className="flex justify-center pt-2">
              <Button
                type="submit"
                className="px-6"
                label={isSubmitting ? "Reservando..." : "Confirmar reserva"}
                disabled={isSubmitting}
                size="text-2xl"
              />
            </div>
          </form>
        </div>
      </div>
    </div>
  )
}
