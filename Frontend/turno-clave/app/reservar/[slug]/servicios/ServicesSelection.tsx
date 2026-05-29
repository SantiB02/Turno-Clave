"use client"

import { useState } from "react"
import Button from "@/app/components/Button"
import type { Service } from "@/types/service"
import ReservationHeader from "../../ReservationHeader"
import { useReservationFlow } from "../ReservationFlowProvider"
import ReservationService from "./ReservationService"
import ServicesTotal from "./ServicesTotal"

export default function ServicesSelection() {
  const [isLoadingPage, setIsLoadingPage] = useState(false)

  const handleClickButton = () => {
    setIsLoadingPage(true)
  }

  const {
    business,
    slug,
    selectedServices,
    setSelectedServices,
    selectedProfessionalsByService,
    setSelectedProfessionalsByService,
    setSelectedSlot,
  } = useReservationFlow()

  const onClickService = (service: Service) => {
    const isSelected = selectedServices.some(
      (selectedService) => selectedService.externalId === service.externalId,
    )

    if (isSelected) {
      setSelectedSlot(null)
      setSelectedServices((prevSelected) =>
        prevSelected.filter((s) => s.externalId !== service.externalId),
      )
      setSelectedProfessionalsByService((prevSelected) => {
        const nextSelected = { ...prevSelected }
        delete nextSelected[service.externalId]
        return nextSelected
      })
      return
    }

    setSelectedSlot(null)
    setSelectedServices((prevSelected) => [...prevSelected, service])
    setSelectedProfessionalsByService((prevSelected) => ({
      ...prevSelected,
      [service.externalId]: prevSelected[service.externalId] ?? null,
    }))
  }

  const onChangeProfessional = (
    serviceExternalId: string,
    professionalExternalId: string | null,
  ) => {
    setSelectedSlot(null)
    setSelectedProfessionalsByService((prevSelected) => ({
      ...prevSelected,
      [serviceExternalId]: professionalExternalId,
    }))
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
      <ReservationHeader
        backButtonUrl={`/reservar/${slug}`}
        title="Elegí tus servicios"
        currentStep={0}
      />

      <div className="flex flex-col gap-4">
        {services.length === 0 ? (
          <div>No hay servicios</div>
        ) : (
          services.map((service) => (
            <ReservationService
              key={service.externalId}
              service={service}
              professionals={business.professionals.filter((professional) =>
                professional.services.some(
                  (professionalService) =>
                    professionalService.externalId === service.externalId,
                ),
              )}
              onClick={onClickService}
              onChangeProfessional={onChangeProfessional}
              selectedProfessionalExternalId={
                selectedProfessionalsByService[service.externalId] ?? null
              }
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
            href={`/reservar/${business.slug}/horarios`}
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
