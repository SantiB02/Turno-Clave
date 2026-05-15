import { redirect } from "next/navigation"
import { auth } from "@/auth"
import { getActiveBusiness } from "@/services/businessService"
import { getServicesByActiveBusiness } from "@/services/serviceService"
import type { BusinessDetail } from "@/types/business"
import type { Service } from "@/types/service"
import MiNegocioTabs from "./MiNegocioTabs"

export default async function MiNegocio() {
  const session = await auth()

  if (!session) {
    redirect("/")
  }

  const business: BusinessDetail = await getActiveBusiness()
  const services: Service[] = await getServicesByActiveBusiness()

  return (
    <div>
      <h1 className="font-bold text-4xl mb-9">Mi Negocio</h1>
      <MiNegocioTabs business={business} services={services} />
    </div>
  )
}
