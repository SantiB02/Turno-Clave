import { getServicesByActiveBusiness } from "@/services/businessService"
import type { Service } from "@/types/service"
import ServicesView from "./ServicesView"

export default async function MisServicios() {
  const services: Service[] = await getServicesByActiveBusiness()

  return <ServicesView services={services} />
}
