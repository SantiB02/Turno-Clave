import { redirect } from "next/navigation"
import UserAvatar from "@/app/components/UserAvatar"
import { auth } from "@/auth"

export default async function MiCuenta() {
  const session = await auth()

  if (!session) {
    redirect("/")
  }

  return (
    <div>
      <h1 className="font-bold text-4xl mb-9">Mi Cuenta</h1>
      <div className="mb-6">
        <UserAvatar height={100} width={100} />
      </div>
      <ul>
        <li className="mb-4">
          <strong>Nombre:</strong> {session?.user?.name || "No disponible"}
        </li>
        <li className="mb-4">
          <strong>Email:</strong> {session?.user?.email || "No disponible"}
        </li>
      </ul>
    </div>
  )
}
