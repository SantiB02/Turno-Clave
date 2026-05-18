import type { Metadata } from "next"
import { Didact_Gothic, Geist_Mono } from "next/font/google"
import "../globals.css"
import Link from "next/link"
import { redirect } from "next/navigation"
import type { ReactNode } from "react"
import { auth } from "@/auth"
import { getMyBusinesses } from "@/services/businessService"
import type { BusinessDetail } from "@/types/business"
import Footer from "../components/Footer"
import HelpFloatingButton from "../components/HelpFloatingButton"
import DashboardMobileNav from "./DashboardMobileNav"
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

  if (session.user?.email === "doetesting02@gmail.com") {
    redirect("/onboarding")
  }

  const result = await getMyBusinesses()

  let businesses: BusinessDetail[] = []

  if (result.ok) {
    businesses = result.data
  }

  if (businesses.length === 0) {
    redirect("/onboarding")
  }

  return (
    <div
      className={`min-h-screen flex flex-col lg:flex-row ${didactGothic.className} ${geistMono.variable} antialiased`}
    >
      <HelpFloatingButton />
      <DashboardMobileNav user={session.user} />
      <aside className="sticky top-0 hidden h-screen w-24 flex-col overflow-y-auto bg-primary-orange p-5 scrollbar-thin scrollbar-thumb-gray-100 scrollbar-track-orange-400 lg:flex">
        <Link
          href="/"
          className="text-lg text-center mb-4 text-white font-bold"
        >
          Turno Clave
        </Link>
        <SidebarNav user={session.user} />
      </aside>

      <div className="flex flex-col flex-1">
        <section className="min-h-screen flex-1 p-4 sm:p-6">
          <main className="">{children}</main>
        </section>
        <Footer />
      </div>
    </div>
  )
}
