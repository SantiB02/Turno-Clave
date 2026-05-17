"use client"

import Image from "next/image"
import Link from "next/link"
import { usePathname, useRouter } from "next/navigation"
import OrangeWavesBottom from "./components/OrangeWavesBottom"

export default function NotFound() {
  const router = useRouter()
  const pathname = usePathname()

  const isBookingRoute = pathname.startsWith("/reservar/")

  return (
    <div>
      <Link href="/" className="flex absolute pl-4 pt-2">
        <Image
          src="/header-logo-300x100.png"
          alt="Turno Clave Logo"
          width={180}
          height={180}
          className="mr-2"
        />
      </Link>
      <div className="min-h-screen flex flex-col items-center px-3 justify-center">
        <h1 className="text-4xl text-center font-bold">
          Error 404 - Página no encontrada
        </h1>
        <p className="text-gray-600 mt-4">
          Lo sentimos, la página que estás buscando no existe.
        </p>
        {isBookingRoute ? (
          <Link
            href="/"
            className="mt-6 px-4 hover:bg-primary-orange/80 py-2 bg-primary-orange text-white rounded transition"
          >
            Ir al inicio
          </Link>
        ) : (
          <button
            type="button"
            onClick={() => router.back()}
            className="mt-6 px-4 hover:bg-primary-orange/80 py-2 bg-primary-orange text-white rounded transition"
          >
            Volver a la página anterior
          </button>
        )}
      </div>
      <OrangeWavesBottom />
    </div>
  )
}
