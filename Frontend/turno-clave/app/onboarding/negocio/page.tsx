import { redirect } from "next/navigation"
import { auth } from "@/auth"
import { getMyBusinesses } from "@/services/businessService"
import type { BusinessDetail } from "@/types/business"
import NextStepButton from "../NextStepButton"
import OnboardingBusinessForm from "./OnboardingBusinessForm"

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
    <div className="flex flex-col justify-center items-center">
      <h1 className="text-3xl font-bold">
        ¡Te damos la bienvenida a Turno Clave,{" "}
        <span className="text-primary-orange">
          {session.user?.name?.split(" ")[0]}
        </span>
        !
      </h1>
      <div className="mt-8 mb-6 max-w-lg">
        <OnboardingBusinessForm />
      </div>
      <NextStepButton href="/onboarding/ubicacion" />
    </div>
  )
}
