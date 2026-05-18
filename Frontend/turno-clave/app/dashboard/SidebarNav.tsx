"use client"
import {
  BuildingStorefrontIcon,
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
  variant?: "desktop" | "mobile"
  onNavigate?: () => void
}

const navigationItems = [
  {
    title: "Inicio",
    href: "/dashboard",
    Icon: HomeIcon,
  },
  {
    title: "Mis Turnos",
    href: "/dashboard/mis-turnos",
    Icon: CalendarDaysIcon,
  },
  {
    title: "Mi Negocio",
    href: "/dashboard/mi-negocio",
    Icon: BuildingStorefrontIcon,
  },
  {
    title: "Mis Servicios",
    href: "/dashboard/mis-servicios",
    Icon: WrenchScrewdriverIcon,
  },
  {
    title: "Mis Estadísticas",
    href: "/dashboard/mis-estadisticas",
    Icon: ChartBarSquareIcon,
  },
]

export default function SidebarNav({
  user,
  variant = "desktop",
  onNavigate,
}: Props) {
  const pathname = usePathname()
  const isMobile = variant === "mobile"

  const isActive = (path: string) => pathname === path

  const itemClassName = (path: string) =>
    isMobile
      ? `flex items-center gap-3 rounded-xl px-4 py-3 transition ${
          isActive(path)
            ? "bg-white text-orange-500"
            : "bg-dark-blue text-white hover:bg-orange-400/70"
        }`
      : `flex items-center justify-center py-2 rounded transition ${
          isActive(path)
            ? "bg-white text-orange-500"
            : "hover:bg-orange-400 text-white"
        }`

  return (
    <nav className={isMobile ? "w-full" : "h-full"}>
      <div
        className={`text-sm font-semibold text-white ${
          isMobile ? "space-y-6" : "flex h-full flex-col gap-4"
        }`}
      >
        <div className={`flex flex-col ${isMobile ? "gap-3" : "gap-2"}`}>
          {navigationItems.map(({ title, href, Icon }) => (
            <Link
              key={href}
              title={title}
              href={href}
              onClick={onNavigate}
              className={itemClassName(href)}
            >
              <Icon className={isMobile ? "h-6 w-6 shrink-0" : "h-10 w-10"} />
              {isMobile ? <span>{title}</span> : null}
            </Link>
          ))}
        </div>

        <div
          className={`flex flex-col ${isMobile ? "gap-3" : "mt-auto gap-2"}`}
        >
          <Link
            title="Mi Cuenta"
            href="/dashboard/mi-cuenta"
            onClick={onNavigate}
            className={itemClassName("/dashboard/mi-cuenta")}
          >
            <div
              className={`rounded-full bg-gray-300 flex items-center justify-center ${
                isMobile ? "h-8 w-8" : "h-10 w-10"
              }`}
            >
              <UserAvatar user={user} />
            </div>
            {isMobile ? <span>Mi Cuenta</span> : null}
          </Link>

          <Link
            title="Configuración"
            href="/dashboard/configuracion"
            onClick={onNavigate}
            className={itemClassName("/dashboard/configuracion")}
          >
            <Cog6ToothIcon
              className={isMobile ? "h-6 w-6 shrink-0" : "h-10 w-10"}
            />
            {isMobile ? <span>Configuración</span> : null}
          </Link>

          <SignOutIcon variant={variant} onOpenChange={onNavigate} />
        </div>
      </div>
    </nav>
  )
}
