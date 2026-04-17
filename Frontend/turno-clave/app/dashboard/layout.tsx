import type { Metadata } from "next"
import { Didact_Gothic, Geist_Mono } from "next/font/google"
import "../globals.css"
import Link from "next/link"
import { redirect } from "next/navigation"
import type { ReactNode } from "react"
import { auth } from "@/auth"
import { getMyBusinesses } from "@/services/businessService"
import type { BusinessDetail } from "@/types/business"
import HelpFloatingButton from "../components/HelpFloatingButton"
import SidebarNav from "./SidebarNav"

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
        <Link
          href="/"
          className="text-lg text-center mb-4 text-white font-bold"
        >
          Turno Clave
        </Link>
        <SidebarNav user={session.user} />
      </aside>

      <section className="flex-1 p-6">
        <main className="">{children}</main>
      </section>
    </div>
  )
}
