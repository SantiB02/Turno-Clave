import { redirect } from "next/navigation"
import { auth } from "@/auth"

export default async function Dashboard() {
  const session = await auth()

  if (!session) {
    redirect("/")
  }

  console.log("SESSION:", session)

  return (
    <div>
      <div className="flex items-center justify-between">
        <h1 className="font-bold text-4xl mb-9">Panel de Control</h1>
        <button
          type="button"
          className="px-4 py-2 bg-primary-orange text-white rounded"
        >
          Nuevo turno
        </button>
      </div>

      <h1 className="text-2xl">
        ¡Bienvenido/a,{" "}
        <span className="text-primary-orange">{session?.user?.name}</span>!
      </h1>
      <p>
        Lorem ipsum dolor sit amet consectetur adipisicing elit. Ipsa modi
        magnam soluta aperiam optio, suscipit aliquid inventore deleniti esse
        minima tempore neque fugit. Eaque doloribus quaerat non quidem pariatur
        eos.
      </p>
    </div>
  )
}
