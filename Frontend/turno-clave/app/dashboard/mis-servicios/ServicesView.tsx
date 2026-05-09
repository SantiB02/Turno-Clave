"use client"

import { useEffect, useState } from "react"
import type { Professional } from "@/types/professional"
import type { Service } from "@/types/service"
import AddService from "./AddService"
import ServiceAccordion from "./ServiceAccordion"

type ServicesListProps = {
  services: Service[]
  professionals: Professional[]
}

export default function ServicesView({
  services,
  professionals,
}: ServicesListProps) {
  const [serviceList, setServiceList] = useState(services)

  useEffect(() => {
    setServiceList(services)
  }, [services])

  const handleServiceDeleted = (serviceExternalId: string) => {
    setServiceList((currentServices) =>
      currentServices.filter(
        (service) => service.externalId !== serviceExternalId,
      ),
    )
  }

  const handleServiceCreated = (service: Service) => {
    setServiceList((currentServices) => [...currentServices, service])
  }

  const handleServiceUpdated = (updatedService: Service) => {
    setServiceList((currentServices) =>
      currentServices.map((service) =>
        service.externalId === updatedService.externalId ? updatedService : service,
      ),
    )
  }

  return (
    <div>
      <h1 className="font-bold text-4xl mb-9">Mis Servicios</h1>
      <div className="mt-6">
        {serviceList.length === 0 ? (
          <p className="text-gray-500">No tienes servicios registrados.</p>
        ) : (
          <div className="space-y-3">
            {serviceList.map((s) => (
              <ServiceAccordion
                key={s.externalId}
                service={s}
                professionals={professionals}
                onDeleteSuccess={handleServiceDeleted}
                onUpdateSuccess={handleServiceUpdated}
              />
            ))}
          </div>
        )}
      </div>
      <AddService
        professionals={professionals}
        onCreateSuccess={handleServiceCreated}
      />
    </div>
  )
}
