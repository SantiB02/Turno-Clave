import AddItemButton from "@/app/components/AddItemButton"
import type { Service } from "@/types/service"
import AddService from "./AddService"
import ServiceAccordion from "./ServiceAccordion"

type ServicesListProps = {
  services: Service[]
}

export default function ServicesView({ services }: ServicesListProps) {
  return (
    <div>
      <h1 className="font-bold text-4xl mb-9">Mis Servicios</h1>
      <div className="mt-6">
        {services.length === 0 ? (
          <p className="text-gray-500">No tienes servicios registrados.</p>
        ) : (
          <div className="space-y-3">
            {services.map((s) => (
              <ServiceAccordion key={s.externalId} service={s} />
            ))}
          </div>
        )}
      </div>
      <AddService />
    </div>
  )
}
