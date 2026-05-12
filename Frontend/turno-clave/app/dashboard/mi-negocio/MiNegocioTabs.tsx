"use client"

import { useRouter, useSearchParams } from "next/navigation"
import type { BusinessDetail } from "@/types/business"
import InformacionTab from "./InformacionTab"

const tabs = [
  { id: "informacion", label: "Información" },
  { id: "horarios", label: "Horarios" },
  { id: "profesionales", label: "Profesionales" },
  { id: "mi-aplicacion", label: "Mi Aplicación" },
] as const

type MiNegocioTabsProps = {
  business: BusinessDetail
}

export default function MiNegocioTabs({ business }: MiNegocioTabsProps) {
  const router = useRouter()
  const searchParams = useSearchParams()

  const activeTab = searchParams.get("tab") || "informacion"

  function changeTab(tab: string) {
    const params = new URLSearchParams(searchParams.toString())

    params.set("tab", tab)

    router.replace(`?${params.toString()}`)
  }

  return (
    <div>
      <div className="border-t border-gray-300" />

      <div className="flex gap-8 border-b border-gray-300 px-2">
        {tabs.map((tab) => (
          <button
            key={tab.id}
            type="button"
            onClick={() => changeTab(tab.id)}
            className={`
              py-4 cursor-pointer text-lg border-b-2 transition-colors
              ${
                activeTab === tab.id
                  ? "border-primary-orange text-primary-orange font-bold"
                  : "border-transparent text-gray-500 hover:text-black"
              }
            `}
          >
            {tab.label}
          </button>
        ))}
      </div>

      <div className="py-6">
        {activeTab === "informacion" && <InformacionTab business={business} />}

        {activeTab === "horarios" && <div></div>}

        {activeTab === "profesionales" && <div></div>}

        {activeTab === "mi-aplicacion" && <div></div>}
      </div>
    </div>
  )
}
