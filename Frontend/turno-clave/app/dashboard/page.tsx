import { redirect } from "next/navigation"
import { auth } from "@/auth"
import Button from "../components/Button"

export default async function Dashboard() {
  const session = await auth()

  if (!session) {
    redirect("/")
  }

  const TimeGreeting = () => {
    const hours = Number(
      new Intl.DateTimeFormat("es-AR", {
        hour: "numeric",
        hour12: false,
        timeZone: "America/Argentina/Buenos_Aires",
      }).format(new Date()),
    )

    let greeting = ""

    if (hours < 12) {
      greeting = "Buenos días"
    } else if (hours < 18) {
      greeting = "Buenas tardes"
    } else {
      greeting = "Buenas noches"
    }

    return (
      <h1 className="text-2xl">
        ¡{greeting},{" "}
        <span className="text-primary-orange">
          {session.user?.name?.split(" ")[0]}
        </span>
        !
      </h1>
    )
  }

  return (
    <div>
      <div className="flex items-center justify-between">
        <h1 className="font-bold text-4xl mb-9">Panel de Control</h1>
        <Button label="Nuevo turno" href="/dashboard/mis-turnos" />
      </div>
      <TimeGreeting />
    </div>
  )
}
