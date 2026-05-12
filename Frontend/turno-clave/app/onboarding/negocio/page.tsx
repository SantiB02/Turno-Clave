import { redirect } from "next/navigation"
import { auth } from "@/auth"
import OnboardingBusinessForm from "./OnboardingBusinessForm"

export default async function OnboardingBusiness() {
  const session = await auth()

  if (!session) {
    redirect("/")
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
    </div>
  )
}
