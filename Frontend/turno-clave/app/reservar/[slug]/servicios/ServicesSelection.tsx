"use client"

import { useState } from "react"
import BackButton from "@/app/components/BackButton"
import Button from "@/app/components/Button"
import type { Service } from "@/types/service"
import { useReservationBusiness } from "../ReservationBusinessProvider"
import ReservationService from "./ReservationService"
import ServicesTotal from "./ServicesTotal"

export default function ServicesSelection() {
  const [selectedServices, setSelectedServices] = useState<Service[]>([])
  const [isLoadingPage, setIsLoadingPage] = useState(false)

  const handleClickButton = () => {}

  const { business, slug } = useReservationBusiness()

  const onClickService = (service: Service) => {
    if (selectedServices.includes(service)) {
      setSelectedServices(selectedServices.filter((s) => s !== service))
    } else {
      setSelectedServices([...selectedServices, service])
    }
  }

  const services = Array.from(
    new Map(
      business.professionals
        .flatMap((professional) => professional.services)
        .map((service) => [service.externalId, service]),
    ).values(),
  ) as Service[]

  return (
    <>
      <div className="flex relative justify-center items-center">
        <BackButton href={`/reservar/${slug}`} className="absolute left-0" />
        <h1 className="text-2xl font-bold text-dark-blue text-center">
          Elegí tus servicios
        </h1>
      </div>
      <div className="flex flex-col max-w-xl gap-4 my-4">
        {services.length === 0 ? (
          <div>No hay servicios</div>
        ) : (
          services.map((service) => (
            <ReservationService
              key={service.externalId}
              service={service}
              professionals={business.professionals}
              onClick={onClickService}
              selected={selectedServices.some(
                (s) => s.externalId === service.externalId,
              )}
            />
          ))
        )}
        <div className="mt-4">
          <ServicesTotal services={selectedServices} />
        </div>
        <div className="flex justify-center my-2">
          <Button
            href={`/reservar/${business.slug}/servicios`}
            onClick={handleClickButton}
            className="px-6"
            label={isLoadingPage ? "Cargando..." : "Continuar"}
            disabled={isLoadingPage || selectedServices.length === 0}
            size="text-2xl"
          />
        </div>
      </div>
    </>
  )
}
