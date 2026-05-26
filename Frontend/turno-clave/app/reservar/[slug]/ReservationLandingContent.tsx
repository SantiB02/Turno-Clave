"use client"

import {
  BuildingStorefrontIcon,
  CalendarDaysIcon,
  CreditCardIcon,
  MapPinIcon,
  PhoneIcon,
} from "@heroicons/react/24/outline"
import Image from "next/image"
import { useState } from "react"
import Button from "@/app/components/Button"
import { PAYMENT_METHOD_LABELS } from "@/enums/paymentMethods"
import { useReservationFlow } from "./ReservationFlowProvider"

export default function ReservationLandingContent() {
  const [isLoadingPage, setIsLoadingPage] = useState(false)
  const { business } = useReservationFlow()

  const handleClickButton = () => {
    setIsLoadingPage(true)
  }

  return (
    <div className="mt-4">
      <div className=" border border-gray-200 rounded-lg shadow-sm/25">
        <div className="relative h-20 bg-orange-300 rounded-t-lg">
          <Image
            alt="Logo de negocio"
            src={
              business.logoUrl.length > 0
                ? business.logoUrl
                : "/default-avatar.png"
            }
            width={90}
            height={90}
            className="rounded-full absolute left-1/2 top-full -translate-x-1/2 -translate-y-1/2 object-cover scale-105"
          />
        </div>
        <div className="flex flex-col p-4 mt-6">
          <h1 className="text-xl mt-2 mb-2">{business.name}</h1>
          <div className="flex flex-col gap-3">
            <div className="flex items-center gap-2">
              <BuildingStorefrontIcon
                width={25}
                height={25}
                className="text-primary-orange"
              />
              <p>{business.description}</p>
            </div>
            <div className="flex items-center gap-2">
              <MapPinIcon
                width={25}
                height={25}
                className="text-primary-orange"
              />
              <p>
                {business.address}, {business.city}, {business.state},{" "}
                {business.country}
              </p>
            </div>
            <div className="flex items-center gap-2">
              <PhoneIcon
                width={23}
                height={23}
                className="text-primary-orange"
              />
              <p>{business.phone}</p>
            </div>
            <div className="flex items-center gap-2">
              <CreditCardIcon
                width={23}
                height={23}
                className="text-primary-orange"
              />
              <p>
                {business.paymentMethods
                  .map((pm) => PAYMENT_METHOD_LABELS[pm])
                  .join(", ")}
              </p>
            </div>
          </div>
        </div>
      </div>

      <div className=" mt-10 flex flex-col items-center justify-center">
        <div>
          <h2 className="text-2xl font-bold text-dark-blue text-center">
            Reserva tu turno
          </h2>
          <p className="text-lg text-center">De forma rapida y sencilla</p>
        </div>
      </div>

      <div className="flex justify-center my-4">
        <Button
          href={`/reservar/${business.slug}/servicios`}
          onClick={handleClickButton}
          className="px-6"
          label={isLoadingPage ? "Cargando..." : "Sacar turno"}
          disabled={isLoadingPage}
          size="text-2xl"
          icon={<CalendarDaysIcon width={30} height={30} />}
        />
      </div>
    </div>
  )
}
