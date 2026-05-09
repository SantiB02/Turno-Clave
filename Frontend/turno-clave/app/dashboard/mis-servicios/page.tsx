import { getProfessionalsByActiveBusiness } from "@/services/professionalService"
import { getServicesByActiveBusiness } from "@/services/serviceService"
import type { Professional } from "@/types/professional"
import type { Service } from "@/types/service"
import ServicesView from "./ServicesView"

export const dynamic = "force-dynamic"

export default async function MisServicios() {
  const services: Service[] = await getServicesByActiveBusiness()
  const professionals: Professional[] = await getProfessionalsByActiveBusiness()

  return <ServicesView services={services} professionals={professionals} />
}
