"use client"

import { useRouter, useSearchParams } from "next/navigation"
import { useEffect, useState } from "react"
import { getProfessionalsByActiveBusiness } from "@/services/professionalService"
import type { BusinessDetail } from "@/types/business"
import type { Professional } from "@/types/professional"
import type { Service } from "@/types/service"
import HorariosTab from "./HorariosTab"
import InformacionTab from "./InformacionTab"
import ProfesionalesTab from "./ProfesionalesTab"

const tabs = [
  { id: "informacion", label: "Información" },
  { id: "horarios", label: "Horarios" },
  { id: "profesionales", label: "Profesionales" },
  { id: "mi-aplicacion", label: "Mi Aplicación" },
] as const

type MiNegocioTabsProps = {
  business: BusinessDetail
  services: Service[]
}

export default function MiNegocioTabs({
  business,
  services,
}: MiNegocioTabsProps) {
  const router = useRouter()
  const searchParams = useSearchParams()

  const [professionals, setProfessionals] = useState<Professional[]>([])
  const [loadingProfessionals, setLoadingProfessionals] =
    useState<boolean>(false)

  const activeTab = searchParams.get("tab") || "informacion"

  useEffect(() => {
    async function loadProfessionals() {
      try {
        setLoadingProfessionals(true)

        const data: Professional[] = await getProfessionalsByActiveBusiness()

        setProfessionals(data)
      } finally {
        setLoadingProfessionals(false)
      }
    }

    loadProfessionals()
  }, [])

  function changeTab(tab: string) {
    const params = new URLSearchParams(searchParams.toString())

    params.set("tab", tab)

    if (tab !== "horarios") {
      params.delete("professional")
    }

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
                  : "border-transparent text-gray-500 hover:text-primary-orange"
              }
            `}
          >
            {tab.label}
          </button>
        ))}
      </div>

      <div className="py-6">
        {activeTab === "informacion" && <InformacionTab business={business} />}

        {activeTab === "horarios" && (
          <HorariosTab
            business={business}
            professionals={professionals}
            setProfessionals={setProfessionals}
          />
        )}

        {activeTab === "profesionales" && (
          <ProfesionalesTab
            professionals={professionals}
            setProfessionals={setProfessionals}
            businessServices={services}
            loadingProfessionals={loadingProfessionals}
          />
        )}

        {activeTab === "mi-aplicacion" && <div></div>}
      </div>
    </div>
  )
}
