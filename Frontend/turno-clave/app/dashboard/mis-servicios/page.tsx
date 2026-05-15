import { getProfessionalsByActiveBusiness } from "@/services/professionalService"
import { getServicesByActiveBusiness } from "@/services/serviceService"
import type { Professional } from "@/types/professional"
import type { Service } from "@/types/service"
import ServicesView from "./ServicesView"

export const dynamic = "force-dynamic"

export default async function MisServicios() {
  let services: Service[] = []
  let servicesError: string | null = null

  const servicesResult = await getServicesByActiveBusiness()

  if (servicesResult.ok) {
    services = servicesResult.data
  } else {
    servicesError = servicesResult.message
  }

  let professionals: Professional[] = []
  let professionalsError: string | null = null

  const professionalsResult = await getProfessionalsByActiveBusiness()

  if (professionalsResult.ok) {
    professionals = professionalsResult.data
  } else {
    professionalsError = professionalsResult.message
  }

  return (
    <ServicesView
      services={services}
      professionals={professionals}
      servicesError={servicesError}
      professionalsError={professionalsError}
    />
  )
}
