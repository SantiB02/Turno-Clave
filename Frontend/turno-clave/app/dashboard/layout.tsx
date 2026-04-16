import type { Metadata } from "next"
import { Didact_Gothic, Geist_Mono } from "next/font/google"
import "../globals.css"
import {
  BriefcaseIcon,
  CalendarDaysIcon,
  ChartBarSquareIcon,
  Cog6ToothIcon,
  HomeIcon,
  QuestionMarkCircleIcon,
  WrenchScrewdriverIcon,
} from "@heroicons/react/24/outline"
import Link from "next/link"
import { redirect } from "next/navigation"
import type { ReactNode } from "react"
import { auth } from "@/auth"
import { getMyBusinesses } from "@/services/businessService"
import type { BusinessDetail } from "@/types/business"
import HelpFloatingButton from "../components/HelpFloatingButton"
import SignOutIcon from "../components/SignOutIcon"
import UserAvatar from "../components/UserAvatar"

const didactGothic = Didact_Gothic({
  weight: ["400"],
  subsets: ["latin"],
})

const geistMono = Geist_Mono({
  variable: "--font-geist-mono",
  subsets: ["latin"],
})

export const metadata: Metadata = {
  title: "Turno Clave - Agenda de Turnos para Negocios",
  description: "Ordena tu tiempo, potencia tu negocio.",
}

export default async function DashboardLayout({
  children,
}: {
  children: ReactNode
}) {
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

  if (businesses.length === 0) {
    redirect("/onboarding")
  }

  return (
    <div
      className={`min-h-screen flex ${didactGothic.className} ${geistMono.variable} antialiased`}
    >
      <HelpFloatingButton />
      <aside className="sticky mr-6 top-0 h-screen w-24 bg-primary-orange p-5 flex flex-col overflow-y-auto scrollbar-thin scrollbar-thumb-gray-100 scrollbar-track-orange-400">
        <Link href="/" className="text-lg text-white font-bold">
          Turno Clave
        </Link>
        <nav className="h-full">
          <div className="flex flex-col h-full gap-4 text-sm font-semibold text-white">
            <div className="flex flex-col justify-center gap-2">
              <Link
                title="Inicio"
                href="/dashboard"
                className="block mx-auto py-2 rounded hover:bg-orange-400"
              >
                <HomeIcon className="h-10 w-10 inline-block mr-2" />
              </Link>
              <Link
                title="Mis Turnos"
                href="/dashboard/mis-turnos"
                className="block mx-auto py-2 rounded hover:bg-orange-400"
              >
                <CalendarDaysIcon className="h-10 w-10 inline-block mr-2" />
              </Link>
              <Link
                title="Mis Servicios"
                href="/dashboard/mis-servicios"
                className="block mx-auto py-2 rounded hover:bg-orange-400"
              >
                <WrenchScrewdriverIcon className="h-10 w-10 inline-block mr-2" />
              </Link>
              <Link
                title="Mis Negocios"
                href="/dashboard/mis-negocios"
                className="block mx-auto py-2 rounded hover:bg-orange-400"
              >
                <BriefcaseIcon className="h-10 w-10 inline-block mr-2" />
              </Link>
              <Link
                title="Mis Estadísticas"
                href="/dashboard/mis-estadisticas"
                className="block mx-auto py-2 rounded hover:bg-orange-400"
              >
                <ChartBarSquareIcon className="h-10 w-10 inline-block mr-2" />
              </Link>
            </div>
            <div className="flex flex-col gap-2 mt-auto">
              <Link
                title="Mi Cuenta"
                href="/dashboard/mi-cuenta"
                className="block mx-auto py-2 rounded hover:bg-orange-400"
              >
                <div className="rounded-full mr-2 h-10 w-10 bg-gray-300 flex items-center justify-center">
                  <UserAvatar />
                </div>
              </Link>
              <Link
                title="Configuración"
                href="/dashboard/configuracion"
                className="block mx-auto py-2 rounded hover:bg-orange-400"
              >
                <Cog6ToothIcon className="h-10 w-10 inline-block mr-2" />
              </Link>
              {/* <Link
                title="Ayuda"
                href="/dashboard/ayuda"
                className="block mx-auto py-2 rounded hover:bg-orange-400"
              >
                <QuestionMarkCircleIcon className="h-10 w-10 inline-block mr-2" />
              </Link> */}
              <SignOutIcon />
            </div>
          </div>
        </nav>
      </aside>

      <section className="flex-1 p-6">
        <main className="">{children}</main>
      </section>
    </div>
  )
}
