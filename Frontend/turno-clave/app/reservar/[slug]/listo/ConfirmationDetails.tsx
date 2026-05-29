"use client"

import {
  CalendarDaysIcon,
  CheckIcon,
  ClockIcon,
  HashtagIcon,
  WrenchScrewdriverIcon,
} from "@heroicons/react/24/outline"
import { useRouter } from "next/navigation"
import { useEffect } from "react"
import { useReservationFlow } from "../ReservationFlowProvider"

function formatDate(date: string) {
  return date.split("-").reverse().join("/")
}

function formatTime(time: string) {
  return time.slice(0, 5)
}

export default function ConfirmationDetails() {
  const router = useRouter()
  const { confirmationDetails, slug } = useReservationFlow()

  useEffect(() => {
    if (!confirmationDetails) {
      router.replace(`/reservar/${slug}`)
    }
  }, [confirmationDetails, router, slug])

  if (!confirmationDetails) {
    return null
  }

  return (
    <div className="w-full max-w-xl space-y-5 px-6 pb-6 text-left">
      <div className="flex justify-center">
        <div className="flex flex-col w-22 h-22 justify-center items-center rounded-full bg-green-500 shadow-md">
          <CheckIcon className="w-18 h-18 text-white" />
        </div>
      </div>
      <p className="text-lg text-center">
        Recibirás un correo a {confirmationDetails.clientEmail} con los detalles
        de tu turno y la opción de cancelarlo.
      </p>

      <div className="space-y-3 rounded-2xl shadow-sm bg-white border border-gray-300 p-4">
        <h2 className="text-lg font-semibold underline text-gray-900">
          Detalles del turno
        </h2>
        <div className="flex items-center gap-2 text-md text-gray-700">
          <CalendarDaysIcon className="w-6 h-6 text-primary-orange shrink-0" />
          <p>
            <span className="font-semibold">Fecha:</span>{" "}
            {formatDate(confirmationDetails.date)}
          </p>
        </div>

        <div className="flex items-center gap-2 text-md text-gray-700">
          <ClockIcon className="w-6 h-6 text-primary-orange shrink-0" />
          <p>
            <span className="font-semibold">Hora:</span>{" "}
            {formatTime(confirmationDetails.startTime)} -{" "}
            {formatTime(confirmationDetails.endTime)}
          </p>
        </div>
        <div className="text-md text-gray-700">
          <div className="flex items-center gap-2">
            <WrenchScrewdriverIcon className="w-6 h-6 text-primary-orange shrink-0" />
            <span className="font-semibold">Servicios:</span>
          </div>

          <ul className="mt-2 pl-8 list-inside list-disc space-y-1">
            {confirmationDetails.services.map((service) => (
              <li key={`${service.serviceExternalId}-${service.startTime}`}>
                {service.serviceName}
                {service.professionalName
                  ? ` - ${service.professionalName}`
                  : ""}
              </li>
            ))}
          </ul>
        </div>
        {confirmationDetails.reservationCode && (
          <div className="flex items-center gap-2 text-md text-gray-700">
            <HashtagIcon className="w-6 h-6 text-primary-orange shrink-0" />
            <p>
              <span className="font-semibold">Código de reserva:</span>{" "}
              {confirmationDetails.reservationCode}
            </p>
          </div>
        )}
      </div>
    </div>
  )
}
