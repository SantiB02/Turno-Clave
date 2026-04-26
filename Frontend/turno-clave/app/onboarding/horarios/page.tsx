import { redirect } from "next/navigation"
import { auth } from "@/auth"
import OnboardingAvailabilitiesForm from "./OnboardingAvailabilitiesForm"

export default async function OnboardingAvailabilities() {
  const session = await auth()

  if (!session) {
    redirect("/")
  }

  return (
    <div className="flex flex-col justify-center items-center">
      <h1 className="text-3xl font-bold">
        ¿Cuáles son los días y horarios de atención?
      </h1>
      <div className="mt-8 mb-6 max-w-lg">
        <OnboardingAvailabilitiesForm />
      </div>
    </div>
  )
}
