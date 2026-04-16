import { redirect } from "next/navigation"
import { auth } from "@/auth"
import { getMyBusinesses } from "@/services/businessService"
import type { BusinessDetail } from "@/types/business"
import CreateBusinessForm from "../components/CreateBusinessForm"

export default async function Onboarding() {
  const session = await auth()

  if (!session) {
    redirect("/")
  }

  let businesses: BusinessDetail[] = []
  try {
    businesses = await getMyBusinesses()
  } catch (error) {
    if (error instanceof Error && error.message === "UNAUTHORIZED") {
      redirect("/api/auth/signout-redirect")
    }
    throw error
  }

  if (businesses.length > 0) {
    redirect("/dashboard")
  }

  return (
    <div className="ml-10 mt-6">
      <h1 className="text-3xl font-bold">
        ¡Bienvenido/a a Turno <span className="text-primary-orange">Clave</span>
        , {session.user?.name?.split(" ")[0]}!
      </h1>
      <h2 className="text-xl mt-4 text-gray-600">
        Para comenzar, por favor agrega tu primer negocio.
      </h2>
      <div className="mt-8 mb-16 max-w-lg">
        <CreateBusinessForm />
      </div>
    </div>
  )
}
