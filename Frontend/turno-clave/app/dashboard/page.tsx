import { redirect } from "next/navigation"
import { auth } from "@/auth"
import Button from "../components/Button"

export default async function Dashboard() {
  const session = await auth()

  if (!session) {
    redirect("/")
  }

  return (
    <div>
      <div className="flex items-center justify-between">
        <h1 className="font-bold text-4xl mb-9">Panel de Control</h1>
        <Button label="Nuevo turno" href="/dashboard/mis-turnos" />
      </div>

      <h1 className="text-2xl">
        ¡Bienvenido/a,{" "}
        <span className="text-primary-orange">{session?.user?.name}</span>!
      </h1>
    </div>
  )
}
