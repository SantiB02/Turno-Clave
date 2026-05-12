import { redirect } from "next/navigation"
import { auth } from "@/auth"
import { getActiveBusiness } from "@/services/businessService"
import type { BusinessDetail } from "@/types/business"
import MiNegocioTabs from "./MiNegocioTabs"

export default async function MiNegocio() {
  const session = await auth()

  if (!session) {
    redirect("/")
  }

  const business: BusinessDetail = await getActiveBusiness()

  return (
    <div>
      <h1 className="font-bold text-4xl mb-9">Mi Negocio</h1>
      <MiNegocioTabs business={business} />
    </div>
  )
}
