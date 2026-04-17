"use client"
import {
  BriefcaseIcon,
  CalendarDaysIcon,
  ChartBarSquareIcon,
  Cog6ToothIcon,
  HomeIcon,
  WrenchScrewdriverIcon,
} from "@heroicons/react/24/outline"
import Link from "next/link"
import { usePathname } from "next/navigation"
import type { User } from "next-auth"
import SignOutIcon from "../components/SignOutIcon"
import UserAvatar from "../components/UserAvatar"

type Props = {
  user?: User
}

export default function SidebarNav({ user }: Props) {
  const pathname = usePathname()

  const isActive = (path: string) => pathname === path

  return (
    <nav className="h-full">
      <div className="flex flex-col h-full gap-4 text-sm font-semibold text-white">
        <div className="flex flex-col justify-center gap-2">
          <Link
            title="Inicio"
            href="/dashboard"
            className={`flex items-center justify-center py-2 rounded ${
              isActive("/dashboard")
                ? "bg-white text-orange-500"
                : "hover:bg-orange-400 text-white"
            }`}
          >
            <HomeIcon className="h-10 w-10" />
          </Link>
          <Link
            title="Mis Turnos"
            href="/dashboard/mis-turnos"
            className={`flex items-center justify-center py-2 rounded ${
              isActive("/dashboard/mis-turnos")
                ? "bg-white text-orange-500"
                : "hover:bg-orange-400 text-white"
            }`}
          >
            <CalendarDaysIcon className="h-10 w-10" />
          </Link>
          <Link
            title="Mis Servicios"
            href="/dashboard/mis-servicios"
            className={`flex items-center justify-center py-2 rounded ${
              isActive("/dashboard/mis-servicios")
                ? "bg-white text-orange-500"
                : "hover:bg-orange-400 text-white"
            }`}
          >
            <WrenchScrewdriverIcon className="h-10 w-10" />
          </Link>
          <Link
            title="Mis Negocios"
            href="/dashboard/mis-negocios"
            className={`flex items-center justify-center py-2 rounded ${
              isActive("/dashboard/mis-negocios")
                ? "bg-white text-orange-500"
                : "hover:bg-orange-400 text-white"
            }`}
          >
            <BriefcaseIcon className="h-10 w-10" />
          </Link>
          <Link
            title="Mis Estadísticas"
            href="/dashboard/mis-estadisticas"
            className={`flex items-center justify-center py-2 rounded ${
              isActive("/dashboard/mis-estadisticas")
                ? "bg-white text-orange-500"
                : "hover:bg-orange-400 text-white"
            }`}
          >
            <ChartBarSquareIcon className="h-10 w-10" />
          </Link>
        </div>
        <div className="flex flex-col gap-2 mt-auto">
          <Link
            title="Mi Cuenta"
            href="/dashboard/mi-cuenta"
            className={`flex items-center justify-center py-2 rounded ${
              isActive("/dashboard/mi-cuenta")
                ? "bg-white text-orange-500"
                : "hover:bg-orange-400 text-white"
            }`}
          >
            <div className="rounded-full h-10 w-10 bg-gray-300 flex items-center justify-center">
              <UserAvatar user={user} />
            </div>
          </Link>
          <Link
            title="Configuración"
            href="/dashboard/configuracion"
            className={`flex items-center justify-center py-2 rounded ${
              isActive("/dashboard/configuracion")
                ? "bg-white text-orange-500"
                : "hover:bg-orange-400 text-white"
            }`}
          >
            <Cog6ToothIcon className="h-10 w-10" />
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
  )
}
