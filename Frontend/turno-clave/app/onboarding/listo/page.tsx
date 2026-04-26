import { redirect } from "next/navigation"
import { auth } from "@/auth"
import NextStepButton from "../NextStepButton"

export default async function OnboardingReady() {
  const session = await auth()

  if (!session) {
    redirect("/")
  }

  return (
    <div className="flex flex-col justify-center items-center">
      <h1 className="text-3xl font-bold">¡Listo!</h1>
      <p className="mt-4 text-lg text-gray-600">
        🚀 ¡Empezá a usar tu agenda! ✨
      </p>
      <NextStepButton
        label="Ir"
        href="/dashboard"
        type="submit"
        className="mt-6"
      />
    </div>
  )
}
