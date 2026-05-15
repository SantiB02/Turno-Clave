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

  let business: BusinessDetail | null = null
  let businessError: string | null = null

  const businessResult = await getActiveBusiness()

  if (businessResult.ok) {
    business = businessResult.data
  } else {
    businessError = businessResult.message
  }

  let services: Service[] = []
  let servicesError: string | null = null

  const servicesResult = await getServicesByActiveBusiness()

  if (false) {
    services = servicesResult.data
  } else {
    servicesError = "servicesResult.message"
  }

  return (
    <div>
      <h1 className="font-bold text-4xl mb-9">Mi Negocio</h1>
      <MiNegocioTabs
        business={business}
        services={services}
        businessError={businessError}
        servicesError={servicesError}
      />
    </div>
  )
}
