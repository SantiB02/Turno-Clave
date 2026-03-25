import type { Metadata } from "next"
import { Didact_Gothic, Geist_Mono } from "next/font/google"
import "../globals.css"
import {
  BriefcaseIcon,
  ChartBarSquareIcon,
  Cog6ToothIcon,
  HomeIcon,
  UserIcon,
  WrenchScrewdriverIcon,
} from "@heroicons/react/24/outline"
import Link from "next/link"
import type { ReactNode } from "react"

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

export default function DashboardLayout({ children }: { children: ReactNode }) {
  return (
    <div
      className={`min-h-screen flex ${didactGothic.className} ${geistMono.variable} antialiased`}
    >
      <aside className="sticky top-0 h-screen w-24 bg-primary-orange p-5 flex flex-col gap-5">
        <h2 className="text-lg text-white font-bold">Turno Clave</h2>
        <nav className="h-full">
          <div className="flex flex-col h-full justify-between text-sm font-semibold text-white">
            <div className="flex flex-col justify-center gap-2">
              <Link
                title="Inicio"
                href="/dashboard"
                className="block mx-auto py-2 rounded hover:bg-orange-400"
              >
                <HomeIcon className="h-10 w-10 inline-block mr-2" />
              </Link>
              <Link
                title="Mis Servicios"
                href="/wrench"
                className="block mx-auto py-2 rounded hover:bg-orange-400"
              >
                <WrenchScrewdriverIcon className="h-10 w-10 inline-block" />
              </Link>
              <Link
                title="Mi Negocio"
                href="/briefcase"
                className="block mx-auto py-2 rounded hover:bg-orange-400"
              >
                <BriefcaseIcon className="h-10 w-10 inline-block mr-2" />
              </Link>
              <Link
                title="Mis Estadísticas"
                href="/chart"
                className="block mx-auto py-2 rounded hover:bg-orange-400"
              >
                <ChartBarSquareIcon className="h-10 w-10 inline-block mr-2" />
              </Link>
            </div>
            <div>
              <Link
                title="Mi Cuenta"
                href="/user"
                className="block mx-auto py-2 rounded hover:bg-orange-400"
              >
                <UserIcon className="h-10 w-10 inline-block mr-2" />
              </Link>
              <Link
                title="Configuración"
                href="/settings"
                className="block mx-auto py-2 rounded hover:bg-orange-400"
              >
                <Cog6ToothIcon className="h-10 w-10 inline-block mr-2" />
              </Link>
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
