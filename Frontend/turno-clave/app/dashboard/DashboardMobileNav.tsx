"use client"

import { Bars3Icon, XMarkIcon } from "@heroicons/react/24/outline"
import Link from "next/link"
import type { User } from "next-auth"
import { useState } from "react"
import SidebarNav from "./SidebarNav"

type DashboardMobileNavProps = {
  user?: User
}

export default function DashboardMobileNav({ user }: DashboardMobileNavProps) {
  const [isOpen, setIsOpen] = useState(false)

  const closeMenu = () => {
    setIsOpen(false)
  }

  return (
    <div className="sticky top-0 z-50 lg:hidden">
      <div className="border-b border-orange-400 bg-primary-orange text-white shadow-sm">
        <div className="flex items-center justify-between px-4 py-4">
          <Link href="/" className="text-lg font-bold">
            Turno Clave
          </Link>

          <button
            type="button"
            aria-expanded={isOpen}
            aria-label={isOpen ? "Cerrar menu" : "Abrir menu"}
            onClick={() => setIsOpen((open) => !open)}
            className="rounded-lg border border-white/30 p-2 transition"
          >
            {isOpen ? (
              <XMarkIcon className="h-6 w-6" />
            ) : (
              <Bars3Icon className="h-6 w-6" />
            )}
          </button>
        </div>
      </div>

      {isOpen ? (
        <div className="border-b border-orange-400 bg-primary-orange px-4 pb-6 pt-4 shadow-2xl">
          <SidebarNav user={user} variant="mobile" onNavigate={closeMenu} />
        </div>
      ) : null}
    </div>
  )
}
